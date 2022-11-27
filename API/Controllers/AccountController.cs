using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController : BaseApiControllers
    {
        public UserManager<AppUser> _UserManager;
        private readonly SignInManager<AppUser> _signInManager;
        public ITokenService _TokenService { get; }
        private readonly IMapper _mapper;
        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,ITokenService tokenService,IMapper mapper)
        {
            _mapper = mapper;
            _TokenService = tokenService;
            _signInManager = signInManager;
            _UserManager = userManager;
        }
            [Authorize]
            [HttpGet]
           public async Task<ActionResult<UserDto>> GetCurrentUser(){
              var email= User.FindFirstValue(ClaimTypes.Email);

              var user = await _UserManager.FindByEmailAsync(email);
           return new UserDto{
                  Email = user.Email,
                  Token =_TokenService.CreateToken(user),
                  DisplayName = user.DisplayName,

            };

           }
           [HttpGet("emailexists")]
           public async Task<ActionResult<bool>> CheckEmailExistAsync([FromQuery] string email){

            return await _UserManager.FindByEmailAsync(email) !=null;
           }
             [Authorize]
           [HttpGet("address")]
           public async Task<ActionResult<AddressDto>> GetUserAdress(){
              

               var user = await _UserManager.FindByEmailAddressAsync(HttpContext.User);
               return _mapper.Map<Address,AddressDto>(user.Address);


           }
         [Authorize]
           [HttpPut("address")]
           public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address){
               
                 var user = await _UserManager.FindByEmailAddressAsync(HttpContext.User);

                 user.Address = _mapper.Map<AddressDto,Address>(address);

                 var result = await _UserManager.UpdateAsync(user);
                 if(result.Succeeded) return Ok(_mapper.Map<Address,AddressDto>(user.Address));

                 return BadRequest("Problem updating the user");


           }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto){

            var user = await _UserManager.FindByEmailAsync(loginDto.Email);

            if(user == null) return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user,loginDto.Password,false);

            if(!result.Succeeded) return Unauthorized(new ApiResponse(401));

            return new UserDto{
                  Email = user.Email,
                  Token =_TokenService.CreateToken(user),
                  DisplayName = user.DisplayName,

            };

            
        }
        [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto){

        if(CheckEmailExistAsync(registerDto.Email).Result.Value){

            return new BadRequestObjectResult(new ApiValidationErrorResponse{Errors = new []{"Email address is in use"}});
        }
        var user = new AppUser {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            UserName = registerDto.Email,

        };

        var result = await _UserManager.CreateAsync(user,registerDto.Password);
        if(!result.Succeeded) return BadRequest(new ApiResponse(401));

        return new UserDto{
             DisplayName = user.DisplayName,
             Token = _TokenService.CreateToken(user),
             Email =user.Email

        };

    }




    }
   
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
using Microsoft.AspNetCore.WebUtilities;

namespace API.Controllers
{
    public class AccountController : BaseApiControllers
    {
        public UserManager<AppUser> _UserManager;
        private readonly SignInManager<AppUser> _signInManager;
        public ITokenService _TokenService { get; }
        private readonly IMapper _mapper;
        private int number2 =0;
         Random random = new Random();
       
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
    [HttpPost("ForgotPassword")]
public async Task<ActionResult<ForgotPasswordDto>> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
{
    if (!ModelState.IsValid)
        return BadRequest();

    var user = await _UserManager.FindByEmailAsync(forgotPasswordDto.Email);
    if (user == null)
        return BadRequest("Invalid Request");

    var token = await _UserManager.GeneratePasswordResetTokenAsync(user);
    var param = new Dictionary<string, string>
    {
        {"token", token },
        {"email", forgotPasswordDto.Email }
    };

    var callback = QueryHelpers.AddQueryString(forgotPasswordDto.ClientURI, param);
    
        int number = random.Next(20, 20000);
         number2=number;
        
        Console.WriteLine(number2);
         MailAddress from = new MailAddress("celikarda812@gmail.com");
                MailAddress to = new MailAddress(user.Email);
                MailMessage msg = new MailMessage(from, to);

                msg.IsBodyHtml = true;
                msg.Subject = "şifre Sıfırlama";
                msg.Body += "<h2> Merhaba," + user.DisplayName + "<br> <br>"
                    + "Şifre Değiştirme isteğiniz Alınmıştır." + "<br> <br>" +
                     " Kodunuz: "
                    + number2
                    + "<br> <br>"
                    + "linke tıklayarak şifrenizi değiştirebilirsiniz : </br> "
                    + callback;

                SmtpClient Client = new SmtpClient();
                Client.Port = 587;
                Client.Host = "smtp.gmail.com";
                Client.EnableSsl = true;
                Client.Timeout = 10000;
                Client.DeliveryMethod = SmtpDeliveryMethod.Network;
                Client.UseDefaultCredentials = false;
                Client.Credentials = new NetworkCredential("celikarda812@gmail.com", "umowdeqmuezaxmbb");
                Client.Send(msg);
              

   return new ForgotPasswordDto {
      Email = user.Email,
      ClientURI = callback,
      token = token

   };
            
    
}


[HttpPost("ResetPassword")]
public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordDto resetPasswordDto)
{
    if (!ModelState.IsValid)
        return BadRequest();

    var user = await _UserManager.FindByEmailAsync(resetPasswordDto.Email);
    if (user == null)
        return BadRequest("Invalid Request");

   
var resetPassResult = await _UserManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
    if (!resetPassResult.Succeeded)
    {
        var errors = resetPassResult.Errors.Select(e => e.Description);

        return BadRequest(new { Errors = errors });
    }

    return Ok();

   


  

    
}




    }
   
}
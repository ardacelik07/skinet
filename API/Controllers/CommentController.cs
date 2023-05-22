using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Extensions;
using Core.Entities;
using Core.Entities.Identity;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CommentController: BaseApiControllers
    {
          public UserManager<AppUser> _UserManager;
    
       private readonly StoreContext2  storeContext;

       public CommentController(StoreContext2  storeContext2,UserManager<AppUser> userManager){

                storeContext = storeContext2;
                _UserManager = userManager;
       }
      
        [HttpGet("yorumlar")]
        public  ActionResult<List<comment>> GetAll(){


            var comments =  storeContext.comment.ToList();

            return Ok(comments);
        }
              [Authorize]
           [HttpPost("addcomment")]
           public async Task<ActionResult<commentsdto>> UpdateUserAddress(commentsdto comment){
               
                 var user = await _UserManager.FindByEmailAddressAsync(HttpContext.User);

                 var yorum = new comment{

                      
                      Name = user.DisplayName,
                      Comment = comment.comments    
                 };

                 var result =  storeContext.comment.Add(yorum);

                 storeContext.SaveChanges();
             

                return Ok(yorum);

           }


        

     }
       
    }

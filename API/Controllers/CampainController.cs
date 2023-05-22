using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
      public class CampainController :BaseApiControllers
    {
       private readonly StoreContext2  storeContext;

       public CampainController(StoreContext2  storeContext2){

                storeContext = storeContext2;
       }

        [HttpGet("kampanyalar")]
        public  ActionResult<List<CampainController>> GetAll(){


            var kampanyalar =  storeContext.Campains.ToList();

            return Ok(kampanyalar);
        }

     }
}
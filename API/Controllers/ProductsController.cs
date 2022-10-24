using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController :ControllerBase
    {
        private StoreContext arda;

        public StoreContext Context { get; }
        public ProductsController(StoreContext context)
        {
          arda = context;
        }

        [HttpGet]

        public  async  Task<ActionResult<List<Product>>> GetProducts(){
            var products =  await arda.Products.ToListAsync();
            return Ok(products);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id){
            
            return await arda.Products.FindAsync(id);
        }
    }
}
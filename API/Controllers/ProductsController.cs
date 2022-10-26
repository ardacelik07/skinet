using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    
    [ApiController]
   [Route("api/[controller]")]
    public class ProductsController :ControllerBase
    {
        private readonly IProductRepository repos;

        public StoreContext Context { get; }
        public ProductsController(IProductRepository repo)
        {
                 repos =repo;
        }

        [HttpGet]

        public  async  Task<ActionResult<List<Product>>> GetProducts(){
            var products =  await repos.GetProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id){
            
            return await repos.GetProductByIdAsync(id);
        } 

         [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands(){
            
            return Ok(await repos.GetProductBrandsAsync());
        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes(){
            
            return Ok(await repos.GetProductTypesAsync());
        } 

    }
}
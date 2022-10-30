using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    
    [ApiController]
   [Route("api/[controller]")]
    public class ProductsController :ControllerBase
    {
        
        private readonly IGenericRepository<Product> ProductsRepos;
        private readonly IGenericRepository<ProductBrand> ProductsBrandRepos;
        private readonly IGenericRepository<ProductType> ProductsTypeRepos;

        private readonly IMapper mappers;
     
        public ProductsController(IGenericRepository<Product> ProductsRepo,IGenericRepository<ProductBrand> ProductsBrandRepo,IGenericRepository<ProductType> ProductsTypeRepo, IMapper mapper)
        {
            mappers = mapper;
            ProductsRepos = ProductsRepo;
           
            ProductsBrandRepos = ProductsBrandRepo;
            ProductsTypeRepos = ProductsTypeRepo;
        
                
        }

        [HttpGet]

        public  async  Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts(){
           
           var spec = new ProductsWithTypesAndBrandsSpecification();
            var products =  await ProductsRepos.ListAsync(spec);
            return Ok(mappers
            .Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>((IReadOnlyList<Product>)products));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id){
               var spec = new ProductsWithTypesAndBrandsSpecification(id);

                var product = await ProductsRepos.GetEntityWithSpec(spec);

                return mappers.Map<Product, ProductToReturnDto>(product);
        } 

         [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands(){
            
            return Ok(await ProductsBrandRepos.ListAllAsync());
        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes(){
            
            return Ok(await ProductsTypeRepos.ListAllAsync());
        } 

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    
   // [ApiController]
   //[Route("api/[controller]")]
    public class ProductsController :BaseApiControllers
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

        public  async  Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
           [FromQuery]ProductSpecParams Productparams ){
           
           var spec = new ProductsWithTypesAndBrandsSpecification(Productparams);

           var countSpec = new ProductWithFiltersForCountSpecificication(Productparams);
           var totalItems = await ProductsRepos.CountAsync(countSpec);
            var products =  await ProductsRepos.ListAsync(spec);
            var data =mappers
            .Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>((IReadOnlyList<Product>)products);
            return Ok(new Pagination<ProductToReturnDto>(Productparams.PageIndex,Productparams.PageSize,totalItems,data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
         [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id){
               var spec = new ProductsWithTypesAndBrandsSpecification(id);

                var product = await ProductsRepos.GetEntityWithSpec(spec);
                if(product == null) return NotFound(new ApiResponse(404));

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
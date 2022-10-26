using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

#nullable disable
namespace Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext con;
        public ProductRepository(StoreContext context)
        {
            con = context;
        }

        public  async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
            return await con.ProductBrand.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {

            return await con.Products
             .Include(p => p.ProductType)
            .Include(p => p.ProductBrand)
            .FirstOrDefaultAsync(p => p.Id == id);
             
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {    
             return await con.Products
             .Include(p => p.ProductType)
            .Include(p => p.ProductBrand)
            .ToListAsync();
        }

        public  async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
        {
            return await con.ProductType.ToListAsync();
        }
    }
}
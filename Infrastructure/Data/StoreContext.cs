using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

 #nullable disable

namespace Infrastructure.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductBrand> ProductBrand { get; set; }
 
       public DbSet<ProductType> ProductType { get; set; }
       protected override void OnModelCreating(ModelBuilder modelBuilder){
           
           base.OnModelCreating(modelBuilder);
           modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

       }
       


    }
}
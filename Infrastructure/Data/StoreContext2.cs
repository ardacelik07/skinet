using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
#nullable disable
namespace Infrastructure.Data
{
    public class StoreContext2 :DbContext
    {
         public StoreContext2(DbContextOptions<StoreContext2> options) : base(options)
        {
        }

         public DbSet<Campains> Campains { get; set; }

           public DbSet<comment> comment { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        

    }
    }
}
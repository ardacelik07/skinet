using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;
#nullable disable
namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext cons;
        public GenericRepository(StoreContext con)
        {
            cons = con;
        }

        public async Task<T> GetByIdAsync(int id)
        {
           return await cons.Set<T>().FindAsync(id);
        }

     

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await cons.Set<T>().ToListAsync();
        }
             public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }
        public  async Task<IReadOnlyCollection<T>> ListAsync(ISpecification<T> spec)
        {
                   return await ApplySpecification(spec).ToListAsync();
        }

        private IQueryable<T>   ApplySpecification(ISpecification<T> spec){

             return SpecificationEvaluator<T>.GetQuery(cons.Set<T>().AsQueryable(), spec);
        }


    }
}
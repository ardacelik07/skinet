using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
#nullable disable


namespace Infrastructure.Data
{
    public class UnitOfWork : IUnıtOfWork
    {
        public StoreContext _Context { get; }
        private Hashtable _repositories;
        public UnitOfWork(StoreContext context)
        {
            _Context = context;
        }

        public async Task<int> Complete()
        {
            return await _Context.SaveChangesAsync();
        }

        public void Dispose()
        {
           _Context.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if(_repositories == null) _repositories =new Hashtable();

            var type = typeof(TEntity).Name;
            if(! _repositories.ContainsKey(type))
            {
                var RepositoryType = typeof(GenericRepository<>);
                var repositoryInstance = Activator.CreateInstance(RepositoryType.MakeGenericType(typeof(TEntity)), _Context);
                _repositories.Add(type,repositoryInstance);
            }
            return (IGenericRepository<TEntity>) _repositories[type];
        }
    }
}
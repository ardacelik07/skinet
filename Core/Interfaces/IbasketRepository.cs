using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IbasketRepository
    {
        Task<CustomerBasket> GetBasketAsync(String basketId);
        Task<CustomerBasket>  UpdateBasketAsync(CustomerBasket basket);

        Task<bool>  DeleteBasketAsync(string basketId);
        

    }
}
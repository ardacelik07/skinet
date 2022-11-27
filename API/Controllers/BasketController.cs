using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BasketController : BaseApiControllers
    {
        private readonly IbasketRepository _basketRepo;
        private readonly IMapper _mapper;
        public BasketController(IbasketRepository basketRepository,IMapper mapper)
        {
            _mapper = mapper;
            _basketRepo = basketRepository;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string id){
            var basket = await _basketRepo.GetBasketAsync(id);
            return Ok(basket ?? new CustomerBasket(id));
        }
        [HttpPost]
        public async  Task<ActionResult<CustomerBasket>>  UpdateBasket(CustomerBasketDto basket){
             var CustomerBasket = _mapper.Map<CustomerBasketDto,CustomerBasket>(basket);
            var UpdatedBasket =await _basketRepo.UpdateBasketAsync(CustomerBasket);
            return Ok(UpdatedBasket);
        }
        [HttpDelete]

        public async Task DeleteBasketAsync(string id){

            await _basketRepo.DeleteBasketAsync(id);
        }



    }
}
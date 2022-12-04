using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiControllers
    {
        public IOrderService _OrderService { get; }
        public IMapper _Mapper { get; }
        public OrdersController(IOrderService orderService,IMapper mapper)
        {
            _Mapper = mapper;
            _OrderService = orderService;
        }


        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto){

          var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?
          .Value;       

          var address  = _Mapper.Map<AddressDto,Address> (orderDto.shippingAddress);

          var order = await _OrderService.CreateOrderAsync(email,orderDto.DeliveryMethod,orderDto.BasketId,address); 
          if(order == null) return BadRequest(new ApiResponse(400,"problem creating order"));

          return Ok(order);
        }
        [HttpGet]

        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser(){

             var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?
          .Value;
          var orders = await _OrderService.GetOrdersForUserAsync(email);

          return Ok(_Mapper.Map<IReadOnlyList<Order>,IReadOnlyList<OrderToReturnDto>>(orders));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id){
           var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?
          .Value;
          var order = await _OrderService.GetOrderByIdAsync(id,email);

          if(order == null) return NotFound(new ApiResponse(404));

          return _Mapper.Map<Order,OrderToReturnDto>(order);

        }
        [HttpGet("deliveryMethods")]

        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods(){

            return Ok( await _OrderService.GetDeliveryMethodsAsync());
        }
    }
}
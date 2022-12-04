using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
#nullable disable


namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
       
       
        public IbasketRepository _BasketRepo { get; }
     
        public IUnıtOfWork _UnitOfWork { get; }
        public OrderService(IbasketRepository basketRepo,IUnıtOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
           
            _BasketRepo = basketRepo;
        }
           

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
        {
            var basket = await _BasketRepo.GetBasketAsync(basketId);

            var items = new List<OrderItem>();

            foreach(var item in basket.Items){
                 var productItem = await _UnitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                 var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name,productItem.PictureUrl);
                 var OrderItem = new OrderItem(itemOrdered,productItem.Price,item.Quantity);
                 items.Add(OrderItem);

            }
            var deliveryMethod = await _UnitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            var subtotal = items.Sum(item => item.Price * item.Quantity);

            var order = new Order(items,buyerEmail,shippingAddress,deliveryMethod,subtotal);

            _UnitOfWork.Repository<Order>().Add(order);

            var result = await _UnitOfWork.Complete();
            if(result <= 0) return null;

            await _BasketRepo.DeleteBasketAsync(basketId);
            return order;
            
           
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _UnitOfWork.Repository<DeliveryMethod>().ListAllAsync();
        }
        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)

        {
            var spec = new OrderWithItemsAndOrderingSpecification(id,buyerEmail);

            return await _UnitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec  = new  OrderWithItemsAndOrderingSpecification(buyerEmail);

            return (IReadOnlyList<Order>)await _UnitOfWork.Repository<Order>().ListAsync(spec);
        }
    }
}
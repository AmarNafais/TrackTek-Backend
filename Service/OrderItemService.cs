using Data.Entities;
using Data.Repositories;
using Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IOrderItemService
    {
        long AddOrderItem(CreateOrderItemDTO dTO);
        object GetOrderItem(int id);
        object GetAllOrderItems();
        void UpdateOrderItem(UpdateOrderItemDTO dTO);
        void DeleteOrderItem(int id);
    }

    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderItemService(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        public long AddOrderItem(CreateOrderItemDTO dTO)
        {
            var newOrderItem = new OrderItem
            {
                OrderId = dTO.OrderId,
                GarmentId = dTO.GarmentId,
                Quantity = dTO.Quantity,
                Size = dTO.Size,
            };

            return _orderItemRepository.Add(newOrderItem);
        }

        public object GetOrderItem(int id)
        {
            var orderItem = _orderItemRepository.GetById(id);
            return new
            {
                orderItem.Id,
                orderItem.OrderId,
                orderItem.GarmentId,
                orderItem.Quantity,
                orderItem.Size
            };
        }

        public object GetAllOrderItems()
        {
            var orderItems = _orderItemRepository.GetAll();
            return orderItems.Select(orderItem => new
            {
                orderItem.Id,
                orderItem.OrderId,
                orderItem.GarmentId,
                orderItem.Quantity,
                orderItem.Size
            });
        }

        public void UpdateOrderItem(UpdateOrderItemDTO dTO)
        {
            var orderItemToBeUpdated = _orderItemRepository.GetById(dTO.Id);
            if (orderItemToBeUpdated != null)
            {
                orderItemToBeUpdated.OrderId = dTO.OrderId;
                orderItemToBeUpdated.GarmentId = dTO.GarmentId;
                orderItemToBeUpdated.Quantity = dTO.Quantity;
                orderItemToBeUpdated.Size = dTO.Size;

                _orderItemRepository.Update(orderItemToBeUpdated);
            }


        }

        public void DeleteOrderItem(int id)
        {
            var orderItem = _orderItemRepository.GetById(id);
            if (orderItem != null)
            {
                _orderItemRepository.Delete(id);
            }

        }
    }
}

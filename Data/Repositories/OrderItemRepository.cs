using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IOrderItemRepository
    {
        long Add(OrderItem orderItem);
        OrderItem GetById(int id);
        IEnumerable<OrderItem> GetAll();
        void Update(OrderItem orderItem);
        void Delete(int id);
    }

    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly Repository _repository;

        public OrderItemRepository(Repository repository)
        {
            _repository = repository;
        }

        public long Add(OrderItem orderItem)
        {
            _repository.OrderItems.Add(orderItem);
            _repository.SaveChanges();
            return orderItem.Id;
        }

        public OrderItem GetById(int id)
        {
            return _repository.OrderItems.FirstOrDefault(o => o.Id == id)
                ?? throw new InvalidOperationException($"OrderItem with ID {id} not found.");
        }
        public IEnumerable<OrderItem> GetAll()
        {
            return _repository.OrderItems;
        }

        public void Update(OrderItem orderItem)
        {
            var orderItemToBeUpdated = _repository.OrderItems.FirstOrDefault(o => o.Id == orderItem.Id)
                ?? throw new InvalidOperationException($"OrderItem with ID {orderItem.Id} not found");

            orderItemToBeUpdated.OrderId = orderItem.OrderId;
            orderItemToBeUpdated.GarmentId = orderItem.GarmentId;
            orderItemToBeUpdated.Quantity = orderItem.Quantity;
            orderItemToBeUpdated.Size = orderItem.Size;

            _repository.SaveChanges();
        }

        public void Delete(int id)
        {
            var orderItemToDelete = _repository.OrderItems.FirstOrDefault(o => o.Id == id)
                ?? throw new InvalidOperationException($"OrderItem with ID {id} not found.");
            _repository.OrderItems.Remove(orderItemToDelete);
            _repository.SaveChanges();
        }

    }
}

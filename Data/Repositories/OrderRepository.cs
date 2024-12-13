using Data.Entities;

namespace Data.Repositories
{
    public interface IOrderRepository
    {
        long Add(Order order);
        Order GetById(int id);
        IEnumerable<Order> GetAll();
        void Update(Order order);
        void Delete(int id);
    }

    public class OrderRepository : IOrderRepository
    {
        private readonly Repository _repository;

        public OrderRepository(Repository repository)
        {
            _repository = repository;
        }

        public long Add(Order order)
        {
            _repository.Orders.Add(order);
            _repository.SaveChanges();
            return order.Id;
        }

        public Order GetById(int id)
        {
            return _repository.Orders.FirstOrDefault(o => o.Id == id)
                ?? throw new InvalidOperationException($"Order with ID {id} not found.");
        }

        public IEnumerable<Order> GetAll()
        {
            return _repository.Orders;
        }

        public void Update(Order order)
        {
            var orderToUpdate = _repository.Orders.FirstOrDefault(o => o.Id == order.Id)
                ?? throw new InvalidOperationException($"Order with ID {order.Id} not found.");

            orderToUpdate.CustomerId = order.CustomerId;
            orderToUpdate.OrderDate = order.OrderDate;
            orderToUpdate.DueDate = order.DueDate;
            orderToUpdate.TotalCost = order.TotalCost;
            orderToUpdate.OrderStatus = order.OrderStatus;
            orderToUpdate.UserId = order.UserId;
            orderToUpdate.GarmentId = order.GarmentId;
            orderToUpdate.Quantity = order.Quantity;
            orderToUpdate.Size = order.Size;

            _repository.SaveChanges();
        }

        public void Delete(int id)
        {
            var orderToDelete = _repository.Orders.FirstOrDefault(o => o.Id == id)
                ?? throw new InvalidOperationException($"Order with ID {id} not found.");

            _repository.Orders.Remove(orderToDelete);
            _repository.SaveChanges();
        }
    }
}

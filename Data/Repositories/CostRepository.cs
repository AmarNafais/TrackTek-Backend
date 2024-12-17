using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface ICostRepository
    {
        void Add(Cost cost);
        void Update(Cost cost);
        void Delete(int id);
        Cost GetById(int id);
        Cost GetByOrderId(int id);
        IEnumerable<Cost> GetAll();
    }
    public class CostRepository : ICostRepository
    {
        private readonly Repository _repository;
        public CostRepository(Repository repository)
        {
            _repository = repository;
        }

        public void Add(Cost cost)
        {
            _repository.Costs.Add(cost);
            _repository.SaveChanges();
        }

        public Cost GetById(int id)
        {
            return _repository.Costs.FirstOrDefault(c => c.Id == id);
        }

        public Cost GetByOrderId(int orderId)
        {
            return _repository.Costs.FirstOrDefault(c => c.OrderId == orderId);
        }

        public IEnumerable<Cost> GetAll()
        {
            return _repository.Costs.ToList();
        }

        public void Update(Cost cost)
        {
            _repository.Costs.Update(cost);
            _repository.SaveChanges();
        }

        public void Delete(int id)
        {
            var cost = _repository.Costs.FirstOrDefault(c => c.Id == id);
            if (cost != null)
            {
                _repository.Costs.Remove(cost);
                _repository.SaveChanges();
            }
        }
    }
}

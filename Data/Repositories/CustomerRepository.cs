using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface ICustomerRepository
    {
        long AddCustomer(Customer customer);
        Customer GetCustomerById(int id);
        IEnumerable<Customer> GetAllCustomers();
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(int id);
    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly Repository _repository;

        public CustomerRepository(Repository repository)
        {
            _repository = repository;
        }

        public long AddCustomer(Customer customer)
        {
            var existingCustomer = _repository.Customers.Any(c => c.CustomerEmail == customer.CustomerEmail);
            if (existingCustomer)
            {
                throw new InvalidOperationException("Email already exists!");
            }

            _repository.Customers.Add(customer);
            _repository.SaveChanges();
            return customer.Id;
        }

        public Customer GetCustomerById(int id)
        {
            return _repository.Customers.FirstOrDefault(c => c.Id == id)
                ?? throw new InvalidOperationException($"Customer with ID {id} not found.");
        }
        public IEnumerable<Customer> GetAllCustomers()
        {
            return _repository.Customers;
        }

        public void UpdateCustomer(Customer customer)
        {
            var customerToBeUpdated = _repository.Customers.FirstOrDefault(c => c.Id == customer.Id)
                ?? throw new InvalidOperationException($"Customer with ID {customer.Id} not found");

            customerToBeUpdated.CustomerName = customer.CustomerName;
            customerToBeUpdated.CustomerEmail = customer.CustomerEmail;
            customerToBeUpdated.ContactNumber = customer.ContactNumber;
            customerToBeUpdated.Address = customer.Address;

            _repository.SaveChanges();
        }

        public void DeleteCustomer(int id)
        {
            var customerToDelete = _repository.Customers.FirstOrDefault(c => c.Id == id)
                ?? throw new InvalidOperationException($"Customer with ID {id} not found.");
            _repository.Customers.Remove(customerToDelete);
            _repository.SaveChanges();
        }

    }
}

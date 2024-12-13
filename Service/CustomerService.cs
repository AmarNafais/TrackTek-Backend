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
    public interface ICustomerService
    {
        long AddCustomer(CreateCustomerDTO createCustomerDTO);
        object GetCustomer(int id);
        object GetAllCustomers();
        void UpdateCustomer(UpdateCustomerDTO updateCustomerDTO);
        void DeleteCustomer(int id);
    }

    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;   
        }

        public long AddCustomer(CreateCustomerDTO createCustomerDTO)
        {
            var newCustomer = new Customer
            {
                CustomerName = createCustomerDTO.CustomerName,
                CustomerEmail = createCustomerDTO.CustomerEmail,
                ContactNumber = createCustomerDTO.ContactNumber,
                Address = createCustomerDTO.Address,
                IsActive = createCustomerDTO.IsActive
            };

            return _customerRepository.Add(newCustomer);
        }

        public object GetCustomer(int id)
        {
            var customer = _customerRepository.GetById(id);
            return new
            {
                customer.Id,
                customer.CustomerName,
                customer.CustomerEmail,
                customer.ContactNumber,
                customer.Address,
                customer.IsActive
            };
        }

        public object GetAllCustomers()
        {
            var customers = _customerRepository.GetAll();
            return customers.Select(customer => new
            {
                customer.Id,
                customer.CustomerName,
                customer.CustomerEmail,
                customer.ContactNumber,
                customer.Address,
                customer.IsActive
            });
        }

        public void UpdateCustomer(UpdateCustomerDTO updateCustomerDTO)
        {
            var customerToBeUpdated = _customerRepository.GetById(updateCustomerDTO.Id);
            if (customerToBeUpdated != null)
            {
                customerToBeUpdated.CustomerName = updateCustomerDTO.CustomerName;
                customerToBeUpdated.CustomerEmail = updateCustomerDTO.CustomerEmail;
                customerToBeUpdated.ContactNumber = updateCustomerDTO.ContactNumber;
                customerToBeUpdated.Address = updateCustomerDTO.Address;
                customerToBeUpdated.IsActive = updateCustomerDTO.IsActive;

                _customerRepository.Update(customerToBeUpdated);
            }

            
        }

        public void DeleteCustomer(int id)
        {
            var customer = _customerRepository.GetById(id);
            if (customer != null)
            {
                _customerRepository.Delete(id);
            }
            
        }
    }
}

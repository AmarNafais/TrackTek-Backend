using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface ISupplierRepository
    {
        long Add(Supplier supplier);
        Supplier GetById(int id);
        IEnumerable<Supplier> GetAll();
        void Update(Supplier supplier);
        void Delete(int id);
    }

    public class SupplierRepository : ISupplierRepository
    {
        private readonly Repository _repository;

        public SupplierRepository(Repository repository)
        {
            _repository = repository;
        }

        public long Add(Supplier supplier)
        {
            var existingSupplier = _repository.Suppliers.Any(s => s.Email == supplier.Email);
            if (existingSupplier)
            {
                throw new InvalidOperationException("Email already Exists");
            }

            _repository.Suppliers.Add(supplier);
            _repository.SaveChanges();
            return supplier.Id;
        }

        public Supplier GetById(int id)
        {
            return _repository.Suppliers.FirstOrDefault(s => s.Id == id)
                ?? throw new InvalidOperationException($"Supplier with ID {id} not found.");
        }

        public IEnumerable<Supplier> GetAll()
        {
            return _repository.Suppliers;
        }

        public void Update(Supplier supplier)
        {
            var supplierToUpdate = _repository.Suppliers.FirstOrDefault(s => s.Id == supplier.Id)
                ?? throw new InvalidOperationException($"Supplier with ID {supplier.Id} not found.");

            supplierToUpdate.Name = supplierToUpdate.Name;
            supplierToUpdate.Contact = supplierToUpdate.Contact;
            supplierToUpdate.Email = supplierToUpdate.Email;
            supplierToUpdate.Address = supplierToUpdate.Address;
            _repository.SaveChanges();
        }

        public void Delete(int id)
        {
            var supplierToUpdate = _repository.Suppliers.FirstOrDefault(s => s.Id == id)
                ?? throw new InvalidOperationException($"Supplier with ID {id} not found.");
            _repository.Suppliers.Remove(supplierToUpdate);
            _repository.SaveChanges();
        }
    }
}
    

    


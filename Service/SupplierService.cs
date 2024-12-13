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
    public interface ISupplierService
    {
        long AddSupplier(CreateSupplierDTO createSupplierDTO);
        object GetSupplier(int id);
        object GetAllSuppliers();
        void UpdateSupplier(UpdateSupplierDTO updateSupplierDTO);
        void DeleteSupplier(int id);
    }

    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierService(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public long AddSupplier(CreateSupplierDTO createSupplierDTO)
        {
            var newSupplier = new Supplier
            {
                Name = createSupplierDTO.Name,
                Contact = createSupplierDTO.Contact,
                Email = createSupplierDTO.Email,
                Address = createSupplierDTO.Address,
            };

            return _supplierRepository.Add(newSupplier);
        }

        public object GetSupplier(int id)
        {
            var supplier = _supplierRepository.GetById(id);
            return new
            {
                supplier.Name,
                supplier.Contact,
                supplier.Email,
                supplier.Address
            };
        }

        public object GetAllSuppliers()
        {
            var suppliers = _supplierRepository.GetAll();
            return suppliers.Select(supplier => new
            {
                supplier.Id,
                supplier.Name,
                supplier.Contact,
                supplier.Email,
                supplier.Address
            });
        }

        public void UpdateSupplier(UpdateSupplierDTO updateSupplierDTO)
        {
            var supplierToBeUpdated = _supplierRepository.GetById(updateSupplierDTO.Id);
            if (supplierToBeUpdated != null)
            {
                supplierToBeUpdated.Name = updateSupplierDTO.Name;
                supplierToBeUpdated.Contact = updateSupplierDTO.Contact;
                supplierToBeUpdated.Email = updateSupplierDTO.Email;
                supplierToBeUpdated.Address = updateSupplierDTO.Address;

                _supplierRepository.Update(supplierToBeUpdated);
            }

            
        }

        public void DeleteSupplier(int id)
        {
            var supplier = _supplierRepository.GetById(id);
            if (supplier != null)
            {
                _supplierRepository.Delete(id);
            }

        }
    }
}

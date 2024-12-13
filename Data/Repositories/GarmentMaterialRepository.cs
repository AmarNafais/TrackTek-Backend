using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IGarmentMaterialRepository
    {
        long Add(GarmentMaterial garmentMaterial);
        GarmentMaterial GetById(int id);
        IEnumerable<GarmentMaterial> GetAll();
        void Update(GarmentMaterial garmentMaterial);
        void Delete(int id);
    }

    public class GarmentMaterialRepository : IGarmentMaterialRepository
    {
        private readonly Repository _repository;

        public GarmentMaterialRepository(Repository repository)
        {
            _repository = repository;
        }

        public long Add(GarmentMaterial garmentMaterial)
        {
            _repository.GarmentMaterials.Add(garmentMaterial);
            _repository.SaveChanges();
            return garmentMaterial.Id;
        }

        public GarmentMaterial GetById(int id)
        {
            return _repository.GarmentMaterials.FirstOrDefault(g => g.Id == id)
                ?? throw new InvalidOperationException($"GarmentMaterial with ID {id} not found.");
        }
        public IEnumerable<GarmentMaterial> GetAll()
        {
            return _repository.GarmentMaterials;
        }

        public void Update(GarmentMaterial garmentMaterial)
        {
            var garmentMaterialToBeUpdated = _repository.GarmentMaterials.FirstOrDefault(c => c.Id == garmentMaterial.Id)
                ?? throw new InvalidOperationException($"GarmentMaterial with ID {garmentMaterial.Id} not found");

            garmentMaterialToBeUpdated.GarmentId = garmentMaterial.GarmentId;
            garmentMaterialToBeUpdated.MaterialId = garmentMaterial.MaterialId;
            garmentMaterialToBeUpdated.RequiredQuantity = garmentMaterial.RequiredQuantity;
            garmentMaterialToBeUpdated.UnitOfMeasurement = garmentMaterial.UnitOfMeasurement;

            _repository.SaveChanges();
        }

        public void Delete(int id)
        {
            var garmentMaterialToDelete = _repository.Customers.FirstOrDefault(g => g.Id == id)
                ?? throw new InvalidOperationException($"GarmentMaterial with ID {id} not found.");
            _repository.Customers.Remove(garmentMaterialToDelete);
            _repository.SaveChanges();
        }

    }
}

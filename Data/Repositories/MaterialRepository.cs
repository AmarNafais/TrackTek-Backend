using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IMaterialRepository
    {
        long Add(Material material);
        Material GetById(int id);
        IEnumerable<Material> GetAll();
        void Update(Material customer);
        void Delete(int id);
    }

    public class MaterialRepository : IMaterialRepository
    {
        private readonly Repository _repository;

        public MaterialRepository(Repository repository)
        {
            _repository = repository;
        }

        public long Add(Material material)
        {
            var existingMaterial = _repository.Materials.Any(m => m.Name == material.Name);
            if (existingMaterial)
            {
                throw new InvalidOperationException("Email already exists!");
            }

            _repository.Materials.Add(material);
            _repository.SaveChanges();
            return material.Id;
        }

        public Material GetById(int id)
        {
            return _repository.Materials.FirstOrDefault(m => m.Id == id)
                ?? throw new InvalidOperationException($"Material with ID {id} not found.");
        }
        public IEnumerable<Material> GetAll()
        {
            return _repository.Materials;
        }

        public void Update(Material material)
        {
            var materialToBeUpdated = _repository.Materials.FirstOrDefault(m => m.Id == material.Id)
                ?? throw new InvalidOperationException($"Material with ID {material.Id} not found");

            materialToBeUpdated.Name = material.Name;
            materialToBeUpdated.UnitCost = material.UnitCost;
            materialToBeUpdated.QuantityInStock = material.QuantityInStock;
            materialToBeUpdated.Unit = material.Unit;

            _repository.SaveChanges();
        }

        public void Delete(int id)
        {
            var materialToDelete = _repository.Materials.FirstOrDefault(m => m.Id == id)
                ?? throw new InvalidOperationException($"Material with ID {id} not found.");
            _repository.Materials.Remove(materialToDelete);
            _repository.SaveChanges();
        }

    }
}

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
    public interface IMaterialService
    {
        long AddMaterial(CreateMaterialDTO dTO);
        object GetMaterial(int id);
        object GetAllMaterials();
        void UpdateMaterial(UpdateMaterialDTO dTO);
        void DeleteMaterial(int id);
    }

    public class MaterialService : IMaterialService
    {
        private readonly IMaterialRepository _materialRepository;

        public MaterialService(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
        }

        public long AddMaterial(CreateMaterialDTO dTO)
        {
            var newMaterial = new Material
            {
                Name = dTO.Name,
                UnitCost = dTO.UnitCost,
                QuantityInStock = dTO.QuantityInStock,
                Unit = dTO.Unit
            };

            return _materialRepository.Add(newMaterial);
        }

        public object GetMaterial(int id)
        {
            var material = _materialRepository.GetById(id);
            return new
            {
                material.Id,
                material.Name,
                material.UnitCost,
                material.QuantityInStock,
                material.Unit
            };
        }

        public object GetAllMaterials()
        {
            var materials = _materialRepository.GetAll();
            return materials.Select(material => new
            {
                material.Id,
                material.Name,
                material.UnitCost,
                material.QuantityInStock,
                material.Unit
            });
        }

        public void UpdateMaterial(UpdateMaterialDTO dTO)
        {
            var materialToBeUpdated = _materialRepository.GetById(dTO.Id);
            if (materialToBeUpdated != null)
            {
                materialToBeUpdated.Name = dTO.Name;
                materialToBeUpdated.UnitCost = dTO.UnitCost;
                materialToBeUpdated.QuantityInStock = dTO.QuantityInStock;
                materialToBeUpdated.Unit = dTO.Unit;
                
                _materialRepository.Update(materialToBeUpdated);
            }


        }

        public void DeleteMaterial(int id)
        {
            var material = _materialRepository.GetById(id);
            if (material != null)
            {
                _materialRepository.Delete(id);
            }

        }
    }
}

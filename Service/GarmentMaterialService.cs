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
    public interface IGarmentMaterialService
    {
        long AddGarmentMaterial(CreateGarmentMaterialDTO dTO);
        object GetGarmentMaterial(int id);
        object GetAllGarmentMaterials();
        void UpdateGarmentMaterial(UpdateGarmentMaterialDTO dTO);
        void DeleteGarmentMaterial(int id);
    }

    public class GarmentMaterialService : IGarmentMaterialService
    {
        private readonly IGarmentMaterialRepository _garmentMaterialRepository;

        public GarmentMaterialService(IGarmentMaterialRepository garmentMaterialRepository)
        {
            _garmentMaterialRepository = garmentMaterialRepository;
        }

        public long AddGarmentMaterial(CreateGarmentMaterialDTO dTO)
        {
            var newGarmentMaterial = new GarmentMaterial
            {
                GarmentId = dTO.GarmentId,
                MaterialId = dTO.MaterialId,
                RequiredQuantity = dTO.RequiredQuantity,
                UnitOfMeasurement = dTO.UnitOfMeasurement
            };

            return _garmentMaterialRepository.Add(newGarmentMaterial);
        }

        public object GetGarmentMaterial(int id)
        {
            var garmentMaterial = _garmentMaterialRepository.GetById(id);
            return new
            {
                garmentMaterial.Id,
                garmentMaterial.GarmentId,
                garmentMaterial.MaterialId,
                garmentMaterial.RequiredQuantity,
                garmentMaterial.UnitOfMeasurement
            };
        }

        public object GetAllGarmentMaterials()
        {
            var garmentMaterials = _garmentMaterialRepository.GetAll();
            return garmentMaterials.Select(garmentMaterial => new
            {
                garmentMaterial.Id,
                garmentMaterial.GarmentId,
                garmentMaterial.MaterialId,
                garmentMaterial.RequiredQuantity,
                garmentMaterial.UnitOfMeasurement
            });
        }

        public void UpdateGarmentMaterial(UpdateGarmentMaterialDTO dTO)
        {
            var garmentMaterialToBeUpdated = _garmentMaterialRepository.GetById(dTO.Id);
            if (garmentMaterialToBeUpdated != null)
            {
                garmentMaterialToBeUpdated.GarmentId = dTO.GarmentId;
                garmentMaterialToBeUpdated.MaterialId = dTO.MaterialId;
                garmentMaterialToBeUpdated.RequiredQuantity = dTO.RequiredQuantity;
                garmentMaterialToBeUpdated.UnitOfMeasurement = dTO.UnitOfMeasurement;

                _garmentMaterialRepository.Update(garmentMaterialToBeUpdated);
            }
        }

        public void DeleteGarmentMaterial(int id)
        {
            var garmentMaterial = _garmentMaterialRepository.GetById(id);
            if (garmentMaterial != null)
            {
                _garmentMaterialRepository.Delete(id);
            }

        }
    }
}

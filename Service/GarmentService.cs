using Data.Entities;
using Data.Entities.Enums;
using Data.Repositories;
using Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IGarmentService
    {
        long AddGarment(CreateGarmentDTO dTO);
        object GetGarment(int id);
        object GetAllGarments();
        void UpdateGarment(UpdateGarmentDTO dTO);
        void DeleteGarment(int id);
    }

    public class GarmentService : IGarmentService
    {
        private readonly IGarmentRepository _garmentRepository;

        public GarmentService(IGarmentRepository garmentRepository)
        {
            _garmentRepository = garmentRepository;
        }

        public long AddGarment(CreateGarmentDTO dTO)
        {
            Enum.TryParse(dTO.CategoryType, out Category category);
            Enum.TryParse(dTO.GarmentStatus, out Status.GarmentStatus garmentStatus);
            
            var newGarment = new Garment
            {
                Name = dTO.Name,
                Design = dTO.Design,
                CategoryType = category,
                Sizes = dTO.Sizes,
                BasePrice = dTO.BasePrice,
                GarmentStatus = garmentStatus,
                LaborHoursPerUnit = dTO.LaborHoursPerUnit,
                HourlyLaborRate = dTO.HourlyLaborRate
            };

            return _garmentRepository.Add(newGarment);
        }

        public object GetGarment(int id)
        {
            var garment = _garmentRepository.GetById(id);
            return new
            {
                garment.Id,
                garment.Name,
                garment.Design,
                Category = garment.CategoryType.ToString(),
                garment.Sizes,
                garment.BasePrice,
                GarmentStatus = garment.GarmentStatus.ToString(),
                garment.LaborHoursPerUnit,
                garment.HourlyLaborRate
            };
        }

        public object GetAllGarments()
        {
            var garments = _garmentRepository.GetAll();
            return garments.Select(garment => new
            {
                garment.Id,
                garment.Name,
                garment.Design,
                Category = garment.CategoryType.ToString(),
                garment.Sizes,
                garment.BasePrice,
                GarmentStatus = garment.GarmentStatus.ToString(),
                garment.LaborHoursPerUnit,
                garment.HourlyLaborRate
            });
        }

        public void UpdateGarment(UpdateGarmentDTO dTO)
        {
            Enum.TryParse(dTO.CategoryType, out Category category);
            Enum.TryParse(dTO.GarmentStatus, out Status.GarmentStatus garmentStatus);

            var garmentToBeUpdated = _garmentRepository.GetById(dTO.Id);
            if (garmentToBeUpdated != null)
            {
                garmentToBeUpdated.Name = dTO.Name;
                garmentToBeUpdated.Design = dTO.Design;
                garmentToBeUpdated.CategoryType = category;
                garmentToBeUpdated.Sizes = dTO.Sizes;
                garmentToBeUpdated.BasePrice = dTO.BasePrice;
                garmentToBeUpdated.GarmentStatus = garmentStatus;
                garmentToBeUpdated.LaborHoursPerUnit = dTO.LaborHoursPerUnit;
                garmentToBeUpdated.HourlyLaborRate = dTO.HourlyLaborRate;

                _garmentRepository.Update(garmentToBeUpdated);
            }


        }

        public void DeleteGarment(int id)
        {
            var garment = _garmentRepository.GetById(id);
            if (garment != null)
            {
                _garmentRepository.Delete(id);
            }

        }
    }
}

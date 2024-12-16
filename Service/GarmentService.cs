using Data.Entities;
using Data.Entities.Enums;
using Data.Repositories;
using Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public interface IGarmentService
    {
        long AddGarment(CreateGarmentDTO dTO);
        object GetGarment(int id);
        object GetAllGarments();
        void UpdateGarment(UpdateGarmentDTO dTO);
        void UpdateGarmentStatus(UpdateGarmentStatusDTO dTO);
        void UpdateCategory(UpdateCategoryDTO dTO);
        void DeleteGarment(int id);
    }

    public class GarmentService : IGarmentService
    {
        private readonly IGarmentRepository _garmentRepository;

        public GarmentService(IGarmentRepository garmentRepository)
        {
            _garmentRepository = garmentRepository;
        }

        // Add a new garment
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

        // Retrieve a single garment by ID
        public object GetGarment(int id)
        {
            var garment = _garmentRepository.GetById(id);
            if (garment == null)
            {
                throw new InvalidOperationException("Garment not found.");
            }

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

        // Retrieve all garments
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

        // Update an entire garment
        public void UpdateGarment(UpdateGarmentDTO dTO)
        {
            Enum.TryParse(dTO.CategoryType, out Category category);
            Enum.TryParse(dTO.GarmentStatus, out Status.GarmentStatus garmentStatus);

            var garmentToBeUpdated = _garmentRepository.GetById(dTO.Id);
            if (garmentToBeUpdated == null)
            {
                throw new InvalidOperationException("Garment not found.");
            }

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

        // Update the status of a garment
        public void UpdateGarmentStatus(UpdateGarmentStatusDTO dTO)
        {
            Enum.TryParse(dTO.GarmentStatus, out Status.GarmentStatus garmentStatus);

            var garmentToBeUpdated = _garmentRepository.GetById(dTO.Id);
            if (garmentToBeUpdated == null)
            {
                throw new InvalidOperationException("Garment not found.");
            }

            garmentToBeUpdated.GarmentStatus = garmentStatus;
            _garmentRepository.Update(garmentToBeUpdated);
        }

        // Update the category of a garment
        public void UpdateCategory(UpdateCategoryDTO dTO)
        {
            Enum.TryParse(dTO.CategoryType, out Category category);

            var garmentToBeUpdated = _garmentRepository.GetById(dTO.Id);
            if (garmentToBeUpdated == null)
            {
                throw new InvalidOperationException("Garment not found.");
            }

            garmentToBeUpdated.CategoryType = category;
            _garmentRepository.Update(garmentToBeUpdated);
        }

        // Delete a garment
        public void DeleteGarment(int id)
        {
            var garment = _garmentRepository.GetById(id);
            if (garment == null)
            {
                throw new InvalidOperationException("Garment not found.");
            }

            _garmentRepository.Delete(id);
        }
    }
}

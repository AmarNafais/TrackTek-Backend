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
    public interface IGarmentMachineService
    {
        long AddGarmentMachine(CreateGarmentMachineDTO dTO);
        object GetGarmentMachine(int id);
        object GetAllGarmentMachines();
        void UpdateGarmentMachine(UpdateGarmentMachineDTO dTO);
        void DeleteGarmentMachine(int id);
    }

    public class GarmentMachineService : IGarmentMachineService
    {
        private readonly IGarmentMachineRepository _garmentMachineRepository;

        public GarmentMachineService(IGarmentMachineRepository garmentMachineRepository)
        {
            _garmentMachineRepository = garmentMachineRepository;
        }

        public long AddGarmentMachine(CreateGarmentMachineDTO dTO)
        {
            var newGarmentMachine = new GarmentMachine
            {
                GarmentId = dTO.GarmentId,
                MachineId = dTO.MachineId,
                HoursRequired = dTO.HoursRequired
            };

            return _garmentMachineRepository.Add(newGarmentMachine);
        }

        public object GetGarmentMachine(int id)
        {
            var garmentMachine = _garmentMachineRepository.GetById(id);
            return new
            {
                garmentMachine.Id,
                garmentMachine.GarmentId,
                garmentMachine.MachineId,
                garmentMachine.HoursRequired
            };
        }

        public object GetAllGarmentMachines()
        {
            var garmentMachines = _garmentMachineRepository.GetAll();
            return garmentMachines.Select(garmentMachine => new
            {
                garmentMachine.Id,
                garmentMachine.GarmentId,
                garmentMachine.MachineId,
                garmentMachine.HoursRequired
            });
        }

        public void UpdateGarmentMachine(UpdateGarmentMachineDTO dTO)
        {
            var garmentMachineToBeUpdated = _garmentMachineRepository.GetById(dTO.Id);
            if (garmentMachineToBeUpdated != null)
            {
                garmentMachineToBeUpdated.GarmentId = dTO.GarmentId;
                garmentMachineToBeUpdated.MachineId = dTO.MachineId;
                garmentMachineToBeUpdated.HoursRequired = dTO.HoursRequired;

                _garmentMachineRepository.Update(garmentMachineToBeUpdated);
            }
        }

        public void DeleteGarmentMachine(int id)
        {
            var garmentMachine = _garmentMachineRepository.GetById(id);
            if (garmentMachine != null)
            {
                _garmentMachineRepository.Delete(id);
            }

        }
    }
}

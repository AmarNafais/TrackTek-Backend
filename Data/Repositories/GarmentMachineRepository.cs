using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.Repositories
{
    public interface IGarmentMachineRepository
    {
        long Add(GarmentMachine garmentMachine);
        GarmentMachine GetById(int id);
        IEnumerable<GarmentMachine> GetAll();
        IEnumerable<GarmentMachine> GetByGarmentId(int garmentId); // New method
        void Update(GarmentMachine garmentMachine);
        void Delete(int id);
    }

    public class GarmentMachineRepository : IGarmentMachineRepository
    {
        private readonly Repository _repository;

        public GarmentMachineRepository(Repository repository)
        {
            _repository = repository;
        }

        public long Add(GarmentMachine garmentMachine)
        {
            _repository.GarmentMachines.Add(garmentMachine);
            _repository.SaveChanges();
            return garmentMachine.Id;
        }

        public GarmentMachine GetById(int id)
        {
            return _repository.GarmentMachines.FirstOrDefault(g => g.Id == id)
                ?? throw new InvalidOperationException($"GarmentMachine with ID {id} not found.");
        }

        public IEnumerable<GarmentMachine> GetAll()
        {
            return _repository.GarmentMachines;
        }

        public IEnumerable<GarmentMachine> GetByGarmentId(int garmentId) // New method implementation
        {
            return _repository.GarmentMachines.Where(g => g.GarmentId == garmentId).ToList();
        }

        public void Update(GarmentMachine garmentMachine)
        {
            var garmentMachineToBeUpdated = _repository.GarmentMachines.FirstOrDefault(g => g.Id == garmentMachine.Id)
                ?? throw new InvalidOperationException($"GarmentMachine with ID {garmentMachine.Id} not found");

            garmentMachineToBeUpdated.GarmentId = garmentMachine.GarmentId;
            garmentMachineToBeUpdated.MachineId = garmentMachine.MachineId;
            garmentMachineToBeUpdated.HoursRequired = garmentMachine.HoursRequired;

            _repository.SaveChanges();
        }

        public void Delete(int id)
        {
            var garmentMachineToDelete = _repository.GarmentMachines.FirstOrDefault(g => g.Id == id)
                ?? throw new InvalidOperationException($"GarmentMachine with ID {id} not found."); // Fixed message
            _repository.GarmentMachines.Remove(garmentMachineToDelete);
            _repository.SaveChanges();
        }
    }
}

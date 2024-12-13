using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IGarmentMachineRepository
    {
        long Add(GarmentMachine garmentMachine);
        GarmentMachine GetById(int id);
        IEnumerable<GarmentMachine> GetAll();
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
                ?? throw new InvalidOperationException($"GarmentMaterial with ID {id} not found.");
            _repository.GarmentMachines.Remove(garmentMachineToDelete);
            _repository.SaveChanges();
        }

    }
}

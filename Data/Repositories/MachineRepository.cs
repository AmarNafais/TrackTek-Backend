using Data.Entities;

namespace Data.Repositories
{
    public interface IMachineRepository
    {
        long Add(Machine machine);
        Machine GetById(int id);
        IEnumerable<Machine> GetAll();
        void Update(Machine machine);
        void Delete(int id);
    }

    public class MachineRepository : IMachineRepository
    {
        private readonly Repository _repository;

        public MachineRepository(Repository repository)
        {
            _repository = repository;
        }

        public long Add(Machine machine)
        {
            var existingMachine = _repository.Machines.Any(m => m.Name == machine.Name);
            if (existingMachine)
            {
                throw new InvalidOperationException("Machine already Exists");
            }

            _repository.Machines.Add(machine);
            _repository.SaveChanges();
            return machine.Id;
        }

        public Machine GetById(int id)
        {
            return _repository.Machines.FirstOrDefault(m => m.Id == id)
                ?? throw new InvalidOperationException($"Machine with ID {id} not found.");
        }

        public IEnumerable<Machine> GetAll()
        {
            return _repository.Machines;
        }

        public void Update(Machine machine)
        {
            var machineToUpdate = _repository.Machines.FirstOrDefault(m => m.Id == machine.Id)
                ?? throw new InvalidOperationException($"Machine with ID {machine.Id} not found.");

            machineToUpdate.Name = machine.Name;
            machineToUpdate.MachineType = machine.MachineType;
            machineToUpdate.MachineStatus = machine.MachineStatus;
            machineToUpdate.HourlyRate = machine.HourlyRate;
            
            _repository.SaveChanges();
        }

        public void Delete(int id)
        {
            var machineToUpdate = _repository.Machines.FirstOrDefault(m => m.Id == id)
                ?? throw new InvalidOperationException($"Machine with ID {id} not found.");
            _repository.Machines.Remove(machineToUpdate);
            _repository.SaveChanges();
        }
    }
}

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
    public interface IMachineService
    {
        long AddMachine(CreateMachineDTO createMachineDTO);
        object GetMachine(int id);
        object GetAllMachines();
        void UpdateMachine(UpdateMachineDTO updateMachineDTO);
        void DeleteMachine(int id);
    }

    public class MachineService : IMachineService
    {
        private readonly IMachineRepository _machineRepository;

        public MachineService(IMachineRepository machineRepository)
        {
            _machineRepository = machineRepository;
        }

        public long AddMachine(CreateMachineDTO createMachineDTO)
        {
            Enum.TryParse(createMachineDTO.MachineStatus, out Status.MachineStatus machineStatus);
            var newMachine = new Machine
            {
                Name = createMachineDTO.Name,
                MachineType = createMachineDTO.MachineType,
                MachineStatus = machineStatus,
                HourlyRate = createMachineDTO.HourlyRate,
            };

            return _machineRepository.Add(newMachine);
        }

        public object GetMachine(int id)
        {
            var machine = _machineRepository.GetById(id);
            return new
            {
                machine.Name,
                machine.MachineType,
                MachineStatus = machine.MachineStatus.ToString(),
                machine.HourlyRate,
            };
        }

        public object GetAllMachines()
        {
            var machines = _machineRepository.GetAll();
            return machines.Select(machine => new
            {
                machine.Id,
                machine.Name,
                machine.MachineType,
                MachineStatus = machine.MachineStatus.ToString(),
                machine.HourlyRate
            });
        }

        public void UpdateMachine(UpdateMachineDTO updateMachineDTO)
        {
            Enum.TryParse(updateMachineDTO.MachineStatus, out Status.MachineStatus machineStatus);

            var machineToBeUpdated = _machineRepository.GetById(updateMachineDTO.Id);
            if (machineToBeUpdated != null)
            {
                machineToBeUpdated.Name = updateMachineDTO.Name;
                machineToBeUpdated.MachineType = updateMachineDTO.MachineType;
                machineToBeUpdated.MachineStatus = machineStatus;
                machineToBeUpdated.HourlyRate = updateMachineDTO.HourlyRate;

                _machineRepository.Update(machineToBeUpdated);
            }


        }

        public void DeleteMachine(int id)
        {
            var machine = _machineRepository.GetById(id);
            if (machine != null)
            {
                _machineRepository.Delete(id);
            }

        }
    }
}

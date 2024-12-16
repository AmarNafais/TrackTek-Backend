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
    public interface ICostService
    {
        object CalculateCost(int orderId);
        decimal CalculateTotalCost(int garmentId, int quantity);
        void AddCost(Cost cost);
        void UpdateCost(Cost cost);
        void DeleteCost(int id);
        Cost GetCostById(int id); 
        Cost GetCostByOrderId(int orderId); 
        IEnumerable<Cost> GetAllCosts();

    }

    public class CostService : ICostService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly IGarmentMaterialRepository _garmentMaterialRepository;
        private readonly IMachineRepository _machineRepository;
        private readonly IGarmentMachineRepository _garmentMachineRepository;
        private readonly ICostRepository _costRepository;
        private readonly IGarmentRepository _garmentRepository;

        public CostService(
            IOrderRepository orderRepository,
            IMaterialRepository materialRepository,
            IGarmentMaterialRepository garmentMaterialRepository,
            IMachineRepository machineRepository,
            IGarmentMachineRepository garmentMachineRepository,
            ICostRepository costRepository,
            IGarmentRepository garmentRepository)
        {
            _orderRepository = orderRepository;
            _materialRepository = materialRepository;
            _garmentMaterialRepository = garmentMaterialRepository;
            _machineRepository = machineRepository;
            _garmentMachineRepository = garmentMachineRepository;
            _costRepository = costRepository;
            _garmentRepository = garmentRepository;
        }

        public object CalculateCost(int orderId)
        {
            // Fetch GarmentId using OrderId
            var order = _orderRepository.GetById(orderId);
            if (order == null)
            {
                throw new InvalidOperationException("Order not found.");
            }

            var garmentId = order.GarmentId;
            if (garmentId <= 0)
            {
                throw new InvalidOperationException("No Garment ID associated with the given Order ID.");
            }

            // Fetch Garment Materials and Machines
            var garmentMaterials = _garmentMaterialRepository.GetByGarmentId(garmentId).ToList();
            var garmentMachines = _garmentMachineRepository.GetByGarmentId(garmentId).ToList();

            if (!garmentMaterials.Any() && !garmentMachines.Any())
            {
                throw new InvalidOperationException("No materials or machines associated with the provided Garment ID.");
            }

            // Calculate Material Costs
            var materialCost = garmentMaterials.Sum(garmentMaterial =>
            {
                var material = _materialRepository.GetById(garmentMaterial.MaterialId);
                return material.UnitCost * garmentMaterial.RequiredQuantity * order.Quantity;
            });

            // Calculate Machine Costs
            var machineCost = garmentMachines.Sum(garmentMachine =>
            {
                var machine = _machineRepository.GetById(garmentMachine.MachineId);
                return machine.HourlyRate * garmentMachine.HoursRequired;
            });

            // Fetch Garment for Labor Cost Calculation
            var garment = _garmentRepository.GetById(garmentId);
            if (garment == null)
            {
                throw new InvalidOperationException("Garment not found.");
            }
            Console.WriteLine($"Garment Found: {garment.Name}, Labor Hours per Unit: {garment.LaborHoursPerUnit}, Hourly Rate: {garment.HourlyLaborRate}, Quantity: {order.Quantity}");
            // Calculate Labor Costs
            var laborCost = garment.LaborHoursPerUnit * garment.HourlyLaborRate * order.Quantity;

            // Total Cost
            var totalCost = materialCost + machineCost + laborCost;

            var cost = new Cost
            {
                OrderId = orderId,  // Linking to the Order
                MaterialCost = materialCost,
                LaborCost = laborCost,
                MachineCost = machineCost,
                TotalCost = totalCost
            };

            AddCost(cost);

            return new
            {
                MaterialCost = materialCost,
                LaborCost = laborCost,
                MachineCost = machineCost,
                TotalCost = totalCost
            };
        }

        // for totalcost variable in order entity
        public decimal CalculateTotalCost(int garmentId, int quantity)
        {
            // Fetch Garment Materials
            var garmentMaterials = _garmentMaterialRepository.GetByGarmentId(garmentId).ToList();

            // Calculate Material Costs
            var materialCost = garmentMaterials.Sum(garmentMaterial =>
            {
                var material = _materialRepository.GetById(garmentMaterial.MaterialId);
                return material.UnitCost * garmentMaterial.RequiredQuantity * quantity;
            });

            // Fetch Garment Machines
            var garmentMachines = _garmentMachineRepository.GetByGarmentId(garmentId).ToList();

            // Calculate Machine Costs
            var machineCost = garmentMachines.Sum(garmentMachine =>
            {
                var machine = _machineRepository.GetById(garmentMachine.MachineId);
                return machine.HourlyRate * garmentMachine.HoursRequired;
            });

            // Fetch Garment for Labor Costs
            var garment = _garmentRepository.GetById(garmentId);
            if (garment == null)
            {
                throw new InvalidOperationException("Garment not found.");
            }

            // Calculate Labor Costs
            var laborCost = garment.LaborHoursPerUnit * garment.HourlyLaborRate * quantity;

            // Total Cost
            return materialCost + machineCost + laborCost;
        }


        public void AddCost(Cost cost)
        {
            _costRepository.Add(cost);
        }

        public void UpdateCost(Cost cost)
        {
            _costRepository.Update(cost);
        }

        public void DeleteCost(int id)
        {
            _costRepository.Delete(id);
        }
        public Cost GetCostById(int id)
        {
            return _costRepository.GetById(id);
        }

        public Cost GetCostByOrderId(int orderId)
        {
            return _costRepository.GetByOrderId(orderId);
        }

        public IEnumerable<Cost> GetAllCosts()
        {
            return _costRepository.GetAll();
        }
    }
}

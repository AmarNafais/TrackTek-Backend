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
        object CalculateCost(CreateCostDTO dTO);
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

        public CostService(
            IOrderRepository orderRepository,
            IMaterialRepository materialRepository,
            IGarmentMaterialRepository garmentMaterialRepository,
            IMachineRepository machineRepository,
            IGarmentMachineRepository garmentMachineRepository,
            ICostRepository costRepository)
        {
            _orderRepository = orderRepository;
            _materialRepository = materialRepository;
            _garmentMaterialRepository = garmentMaterialRepository;
            _machineRepository = machineRepository;
            _garmentMachineRepository = garmentMachineRepository;
            _costRepository = costRepository;
        }

        public object CalculateCost(CreateCostDTO dTO)
        {
            // Fetch GarmentId using OrderId
            var order = _orderRepository.GetById(dTO.OrderId);
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

            // Total Cost
            var totalCost = materialCost + machineCost;

            var cost = new Cost
            {
                OrderId = dTO.OrderId,  // Linking to the Order
                MaterialCost = materialCost,
                MachineCost = machineCost,
                TotalCost = totalCost
            };

            AddCost(cost);

            return new
            {
                MaterialCost = materialCost,
                MachineCost = machineCost,
                TotalCost = totalCost
            };
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

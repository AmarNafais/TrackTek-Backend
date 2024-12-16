using Data.Entities;
using Data.Entities.Enums;
using Data.Repositories;
using Microsoft.Extensions.Logging;
using Service.DTOs;
using System;
using System.Linq;

namespace Service
{
    public interface IOrderService
    {
        object AddOrder(CreateOrderDTO dTO);
        object GetOrder(int id);
        object GetAllOrders();
        void UpdateOrder(UpdateOrderDTO dTO);
        void DeleteOrder(int id);
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMachineRepository _machineRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly IGarmentRepository _garmentRepository;
        private readonly IGarmentMaterialRepository _garmentMaterialRepository;
        private readonly IGarmentMachineRepository _garmentMachineRepository;
        private readonly ICostService _costService;
        private readonly IReportService _reportService;
        private readonly ICustomerRepository _customerRepository;

        public OrderService(
            IOrderRepository orderRepository,
            IMachineRepository machineRepository,
            IMaterialRepository materialRepository,
            IGarmentRepository garmentRepository,
            IGarmentMaterialRepository garmentMaterialRepository,
            IGarmentMachineRepository garmentMachineRepository,
            ICostService costService,
            IReportService reportService,
            ICustomerRepository customerRepository)
        {
            _orderRepository = orderRepository;
            _machineRepository = machineRepository;
            _materialRepository = materialRepository;
            _garmentRepository = garmentRepository;
            _garmentMaterialRepository = garmentMaterialRepository;
            _garmentMachineRepository = garmentMachineRepository;
            _costService = costService;
            _reportService = reportService;
            _customerRepository = customerRepository;
        }

        public object AddOrder(CreateOrderDTO dTO)
        {
            // Validate Garment
            var garment = _garmentRepository.GetById(dTO.GarmentId);
            if (garment == null)
                throw new InvalidOperationException("Garment not found.");

            // Check Material Availability
            var warnings = new List<string>();
            var materialsAvailable = true;
            var garmentMaterials = _garmentMaterialRepository.GetAll()
                .Where(gm => gm.GarmentId == dTO.GarmentId)
                .ToList();

            foreach (var garmentMaterial in garmentMaterials)
            {
                var material = _materialRepository.GetById(garmentMaterial.MaterialId);
                if (material.QuantityInStock < garmentMaterial.RequiredQuantity * dTO.Quantity)
                {
                    materialsAvailable = false;
                    warnings.Add($"Insufficient material: {material.Name}");
                }
            }

            // Check Machine Availability
            var machinesAvailable = true;
            var garmentMachines = _garmentMachineRepository.GetAll()
                .Where(gm => gm.GarmentId == dTO.GarmentId)
                .ToList();

            foreach (var garmentMachine in garmentMachines)
            {
                var machine = _machineRepository.GetById(garmentMachine.MachineId);
                if (machine.MachineStatus != Status.MachineStatus.InActive)
                {
                    machinesAvailable = false;
                    warnings.Add($"Machine {machine.Name} is unavailable.");
                }
            }

            // Set the order status
            var orderStatus = (materialsAvailable && machinesAvailable)
                ? Status.OrderStatus.InProgress
                : Status.OrderStatus.Pending;

            // calculate the total cost
            var totalCost = _costService.CalculateTotalCost(dTO.GarmentId, dTO.Quantity);

            // Create and add the order
            var newOrder = new Order
            {
                CustomerId = dTO.CustomerId,
                OrderDate = dTO.OrderDate,
                DueDate = dTO.DueDate,
                TotalCost = totalCost,
                OrderStatus = orderStatus,
                UserId = 1,
                GarmentId = dTO.GarmentId,
                Quantity = dTO.Quantity,
                Size = dTO.Size
            };

            var orderId = _orderRepository.Add(newOrder);
            _costService.CalculateCost(orderId);
            _reportService.GenerateAndSaveReport(orderId);
            // Update Stock and Machine Status if the order is InProgress
            if (orderStatus == Status.OrderStatus.InProgress)
            {
                // Update Material Stock
                foreach (var garmentMaterial in garmentMaterials)
                {
                    var material = _materialRepository.GetById(garmentMaterial.MaterialId);
                    material.QuantityInStock -= (int)(garmentMaterial.RequiredQuantity * dTO.Quantity);
                    _materialRepository.Update(material);
                }

                // Update Machine Status
                foreach (var garmentMachine in garmentMachines)
                {
                    var machine = _machineRepository.GetById(garmentMachine.MachineId);
                    if (machine.MachineStatus == Status.MachineStatus.InActive)
                    {
                        machine.MachineStatus = Status.MachineStatus.Active;
                        _machineRepository.Update(machine);
                    }
                }
            }

            return new
            {
                Message = "Order created successfully.",
                OrderId = orderId,
                OrderStatus = orderStatus,
                Warnings = warnings
            };
        }


        public object GetOrder(int id)
        {
            var order = _orderRepository.GetById(id);
            var customer = _customerRepository.GetById(order.CustomerId);
            var garment = _garmentRepository.GetById(order.GarmentId);
            return new
            {
                order.Id,
                order.CustomerId,
                customer.CustomerName,
                order.OrderDate,
                order.DueDate,
                order.TotalCost,
                order.OrderStatus,
                order.GarmentId,
                garment.Name,
                order.Quantity,
                order.Size
            };
        }

        public object GetAllOrders()
        {
            var orders = _orderRepository.GetAll();
            return orders.Select(order => new
            {
                order.Id,
                order.CustomerId,
                _customerRepository.GetById(order.CustomerId).CustomerName,
                order.OrderDate,
                order.DueDate,
                order.TotalCost,
                order.OrderStatus,
                order.GarmentId,
                _garmentRepository.GetById(order.GarmentId).Name,
                order.Quantity,
                order.Size
            });
        }

        public void UpdateOrder(UpdateOrderDTO dTO)
        {
            // Retrieve the order to update
            var orderToUpdate = _orderRepository.GetById(dTO.Id);

            if (orderToUpdate == null)
            {
                throw new InvalidOperationException("Order not found.");
            }

            // Check if the current order status is InProgress
            if (orderToUpdate.OrderStatus == Status.OrderStatus.InProgress)
            {
                // Restore materials
                var garmentMaterials = _garmentMaterialRepository.GetAll()
                    .Where(gm => gm.GarmentId == orderToUpdate.GarmentId)
                    .ToList();

                foreach (var garmentMaterial in garmentMaterials)
                {
                    var material = _materialRepository.GetById(garmentMaterial.MaterialId);
                    material.QuantityInStock += (int)(garmentMaterial.RequiredQuantity * orderToUpdate.Quantity);
                    _materialRepository.Update(material);
                }

                // Reset machine status
                var garmentMachines = _garmentMachineRepository.GetAll()
                    .Where(gm => gm.GarmentId == orderToUpdate.GarmentId)
                    .ToList();

                foreach (var garmentMachine in garmentMachines)
                {
                    var machine = _machineRepository.GetById(garmentMachine.MachineId);
                    if (machine.MachineStatus == Status.MachineStatus.Active)
                    {
                        machine.MachineStatus = Status.MachineStatus.InActive;
                        _machineRepository.Update(machine);
                    }
                }
            }
            Enum.TryParse(dTO.OrderStatus, out Status.OrderStatus orderStatus);
            
            // calculate the total cost
            var totalCost = _costService.CalculateTotalCost(dTO.GarmentId, dTO.Quantity);

            if (orderToUpdate != null)
            {
                orderToUpdate.CustomerId = dTO.CustomerId;
                orderToUpdate.OrderDate = dTO.OrderDate;
                orderToUpdate.DueDate = dTO.DueDate;
                orderToUpdate.TotalCost = totalCost;
                orderToUpdate.OrderStatus = orderStatus;
                orderToUpdate.GarmentId = dTO.GarmentId;
                orderToUpdate.Quantity = dTO.Quantity;
                orderToUpdate.Size = dTO.Size;

                _orderRepository.Update(orderToUpdate);
            }
        }

        public void DeleteOrder(int id)
        {
            var order = _orderRepository.GetById(id);

            if (order == null)
            {
                throw new InvalidOperationException("Order not found.");
            }

            // Check if the current order status is InProgress
            if (order.OrderStatus == Status.OrderStatus.InProgress)
            {
                // Restore materials
                var garmentMaterials = _garmentMaterialRepository.GetAll()
                    .Where(gm => gm.GarmentId == order.GarmentId)
                    .ToList();

                foreach (var garmentMaterial in garmentMaterials)
                {
                    var material = _materialRepository.GetById(garmentMaterial.MaterialId);
                    material.QuantityInStock += (int)(garmentMaterial.RequiredQuantity * order.Quantity);
                    _materialRepository.Update(material);
                }

                // Reset machine status
                var garmentMachines = _garmentMachineRepository.GetAll()
                    .Where(gm => gm.GarmentId == order.GarmentId)
                    .ToList();

                foreach (var garmentMachine in garmentMachines)
                {
                    var machine = _machineRepository.GetById(garmentMachine.MachineId);
                    if (machine.MachineStatus == Status.MachineStatus.Active)
                    {
                        machine.MachineStatus = Status.MachineStatus.InActive;
                        _machineRepository.Update(machine);
                    }
                }
            }

            _orderRepository.Delete(id);
        }
    }
}

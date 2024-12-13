using Data.Entities;
using Data.Entities.Enums;
using Data.Repositories;
using Service.DTOs;
using System;
using System.Linq;

namespace Service
{
    public interface IOrderService
    {
        long AddOrder(CreateOrderDTO dTO);
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

        public OrderService(
            IOrderRepository orderRepository,
            IMachineRepository machineRepository,
            IMaterialRepository materialRepository,
            IGarmentRepository garmentRepository,
            IGarmentMaterialRepository garmentMaterialRepository,
            IGarmentMachineRepository garmentMachineRepository)
        {
            _orderRepository = orderRepository;
            _machineRepository = machineRepository;
            _materialRepository = materialRepository;
            _garmentRepository = garmentRepository;
            _garmentMaterialRepository = garmentMaterialRepository;
            _garmentMachineRepository = garmentMachineRepository;
        }

        public long AddOrder(CreateOrderDTO dTO)
        {
            // Validate Garment
            var garment = _garmentRepository.GetById(dTO.GarmentId);
            if (garment == null)
                throw new InvalidOperationException("Garment not found.");

            // Check Material Availability
            bool materialsAvailable = true;
            var garmentMaterials = _garmentMaterialRepository.GetAll().Where(gm => gm.GarmentId == dTO.GarmentId);
            foreach (var garmentMaterial in garmentMaterials)
            {
                var material = _materialRepository.GetById(garmentMaterial.MaterialId);
                if (material.QuantityInStock < garmentMaterial.RequiredQuantity * dTO.Quantity)
                {
                    materialsAvailable = false;
                    break; // No need to check further materials if one is already unavailable
                }
            }

            // Check Machine Availability
            bool machinesAvailable = true;
            var garmentMachines = _garmentMachineRepository.GetAll().Where(gm => gm.GarmentId == dTO.GarmentId);
            foreach (var garmentMachine in garmentMachines)
            {
                var machine = _machineRepository.GetById(garmentMachine.MachineId);
                if (machine.MachineStatus == Status.MachineStatus.Active)
                {
                    machinesAvailable = false;
                    break;
                }
            }

            var orderStatus = (materialsAvailable && machinesAvailable) ? Status.OrderStatus.InProgress : Status.OrderStatus.Pending;

            var newOrder = new Order
            {
                CustomerId = dTO.CustomerId,
                OrderDate = dTO.OrderDate,
                DueDate = dTO.DueDate,
                TotalCost = dTO.TotalCost,
                OrderStatus = orderStatus,
                UserId = dTO.UserId,
                GarmentId = dTO.GarmentId,
                Quantity = dTO.Quantity,
                Size = dTO.Size,
            };

            return _orderRepository.Add(newOrder);
        }

        public object GetOrder(int id)
        {
            var order = _orderRepository.GetById(id);
            return new
            {
                order.Id,
                order.CustomerId,
                order.OrderDate,
                order.DueDate,
                order.TotalCost,
                order.OrderStatus,
                order.GarmentId,
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
                order.OrderDate,
                order.DueDate,
                order.TotalCost,
                order.OrderStatus,
                order.GarmentId,
                order.Quantity,
                order.Size
            });
        }

        public void UpdateOrder(UpdateOrderDTO dTO)
        {
            Enum.TryParse(dTO.OrderStatus, out Status.OrderStatus orderStatus);

            var orderToUpdate = _orderRepository.GetById(dTO.Id);
            if (orderToUpdate != null)
            {
                orderToUpdate.CustomerId = dTO.CustomerId;
                orderToUpdate.OrderDate = dTO.OrderDate;
                orderToUpdate.DueDate = dTO.DueDate;
                orderToUpdate.TotalCost = dTO.TotalCost;
                orderToUpdate.OrderStatus = orderStatus;
                orderToUpdate.UserId = dTO.UserId;
                orderToUpdate.GarmentId = dTO.GarmentId;
                orderToUpdate.Quantity = dTO.Quantity;
                orderToUpdate.Size = dTO.Size;

                _orderRepository.Update(orderToUpdate);
            }
        }

        public void DeleteOrder(int id)
        {
            var order = _orderRepository.GetById(id);
            if (order != null)
            {
                _orderRepository.Delete(id);
            }
        }
    }
}

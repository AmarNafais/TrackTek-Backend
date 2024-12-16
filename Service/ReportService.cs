using Data.Entities;
using Data.Repositories;
using Service.DTOs;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IReportService
    {
        object GenerateAndSaveReport(int orderId);
        object GetReportByOrderId(int orderId);
        object GetReportById(int id);
        IEnumerable<object> GetAllReports();
        void DeleteReport(int id);
    }
    public class ReportService : IReportService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICostService _costService;
        private readonly IReportRepository _reportRepository;
        private readonly IGarmentMachineRepository _garmentMachineRepository;
        private readonly IGarmentMaterialRepository _garmentMaterialRepository;
        private readonly IGarmentRepository _garmentRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly IMachineRepository _machineRepository;
       
        public ReportService(IOrderRepository orderRepository, 
            ICustomerRepository customerRepository, 
            ICostService costService, 
            IReportRepository reportRepository,
            IGarmentRepository garmentRepository,
            IGarmentMaterialRepository garmentMaterialRepository,
            IGarmentMachineRepository garmentMachineRepository,
            IMaterialRepository materialRepository,
            IMachineRepository machineRepository)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _costService = costService;
            _reportRepository = reportRepository;
            _garmentRepository = garmentRepository;
            _garmentMachineRepository = garmentMachineRepository;
            _garmentMaterialRepository = garmentMaterialRepository;
            _materialRepository = materialRepository;
            _machineRepository = machineRepository;
        }
        public object GenerateAndSaveReport(int orderId)
        {
            // Fetch order data
            var order = _orderRepository.GetById(orderId);
            if (order == null)
            {
                throw new InvalidOperationException("Order not found.");
            }

            // Fetch customer data
            var customer = _customerRepository.GetById(order.CustomerId);
            if (customer == null)
            {
                throw new InvalidOperationException("Customer not found.");
            }

            // Generate description
            var description = GenerateDescription(order, customer);

            // Generate content (JSON structure)
            var content = GenerateContent(orderId);

            // Create report object
            var report = new Report
            {
                OrderId = orderId,
                Description = description,
                Content = JsonSerializer.Serialize(content)
            };

            // Save report to database
            _reportRepository.AddReport(report);
            return report;
        }

        private string GenerateDescription(Order order, Customer customer)
        {
            return $"This order was placed on {order.OrderDate?.ToString("MM/dd/yyyy")} and is due on {order.DueDate.ToString("MM/dd/yyyy")}. " +
                   $"The order is placed by {customer.CustomerName} (Email: {customer.CustomerEmail}, Contact: {customer.ContactNumber}) ";
        }

        private object GenerateContent(int orderId)
        {
            // Fetch Garment ID for the Order
            var garmentId = _orderRepository.GetById(orderId)?.GarmentId;
            if (!garmentId.HasValue)
            {
                return new { Message = "Garment not found for this order." };
            }

            // Retrieve Garment Details
            var garmentDetails = _garmentRepository.GetById(garmentId.Value);
            if (garmentDetails == null)
            {
                return new { Message = "Garment details not found." };
            }

            // Retrieve Garment Materials
            var garmentMaterials = _garmentMaterialRepository.GetByGarmentId(garmentId.Value).ToList();
            var materials = garmentMaterials?.Select(garmentMaterial =>
            {
                var materialEntity = _materialRepository.GetById(garmentMaterial.MaterialId);
                return materialEntity != null
                    ? new
                    {
                        materialName = materialEntity.Name,
                        requiredQuantity = garmentMaterial.RequiredQuantity,
                        unit = materialEntity.Unit
                    }
                    : null;
            }).Where(m => m != null).ToList();

            // Retrieve Garment Machines
            var garmentMachines = _garmentMachineRepository.GetByGarmentId(garmentId.Value).ToList();
            var machines = garmentMachines?.Select(garmentMachine =>
            {
                var machineEntity = _machineRepository.GetById(garmentMachine.MachineId);  
                return machineEntity != null
                    ? new
                    {
                        machineName = machineEntity.Name,
                        machineType = machineEntity.MachineType,
                        machineStatus = machineEntity.MachineStatus,
                        hoursRequired = garmentMachine.HoursRequired
                    }
                    : null;
            }).Where(m => m != null).ToList();

            // Retrieve Cost Details
            var costDetails = _costService.GetCostByOrderId(orderId);
            if (costDetails == null)
            {
                return new { Message = "Cost details not found." };
            }

            // Return an object with variables
            return new
            {
                OrderId = orderId,
                Garment = new
                {
                    garmentDetails.Name,
                    garmentDetails.Design,
                    garmentDetails.CategoryType,
                    garmentDetails.BasePrice
                },
                Materials = materials,
                Machines = machines,
                Costs = new
                {
                    costDetails.MaterialCost,
                    costDetails.LaborCost,
                    costDetails.MachineCost,
                    costDetails.TotalCost
                }
            };
        }

        public object GetReportById(int id)
        {
            var report = _reportRepository.GetById(id);
            if (report == null)
            {
                return null; // Or throw a custom exception
            }

            return new
            {
                report.Id,
                report.OrderId,
                report.Description,
                report.Content,
            };
        }

        public IEnumerable<object> GetAllReports()
        {
            var reports = _reportRepository.GetAll();

            return reports.Select(report => new
            {
                report.Id,
                report.OrderId,
                report.Description,
                report.Content,
            });
        }
        public object GetReportByOrderId(int orderId)
        {
            // Fetch order data
            var order = _orderRepository.GetById(orderId);
            if (order == null)
            {
                throw new InvalidOperationException("Order not found.");
            }

            // Fetch customer data
            var customer = _customerRepository.GetById(order.CustomerId);
            if (customer == null)
            {
                throw new InvalidOperationException("Customer not found.");
            }

            // Generate description
            var description = GenerateDescription(order, customer);

            // Generate content (JSON structure)
            var content = GenerateContent(orderId);

            return new
            {
                order,
                description,
                content
            };
        }
        public void DeleteReport(int id)
        {
            _reportRepository.Delete(id);
        }
    }
}

using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IReportRepository
    {
        Report GetOrderById(int orderId);
        void AddReport(Report report);
        Report GetById(int id);
        IEnumerable<Report> GetAll();
        Report GetByOrderId(int orderId);
        void Delete(int id);
    }
    public class ReportRepository : IReportRepository
    {
        private readonly Repository _repository;

        public ReportRepository(Repository repository)
        {
            _repository = repository;
        }
        public Report GetOrderById(int orderId)
        {
            return _repository.Reports.FirstOrDefault(r => r.OrderId == orderId);
        }
        public void AddReport(Report report)
        {
            _repository.Reports.Add(report);
            _repository.SaveChanges();
        }
        public Report GetById(int id)
        {
            return _repository.Reports.FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<Report> GetAll()
        {
            return _repository.Reports.ToList();
        }
        public Report GetByOrderId(int orderId)
        {
            return _repository.Reports.FirstOrDefault(r => r.OrderId == orderId);
        }

        public void Delete(int id)
        {
            var report = _repository.Reports.FirstOrDefault(r => r.Id == id);
            if (report != null)
            {
                _repository.Reports.Remove(report);
                _repository.SaveChanges();
            }
        }
    }
}

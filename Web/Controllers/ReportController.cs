using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.DTOs;

namespace Web.Controllers
{
    [ApiController]
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [Route("v1/Report/Create")]
        [HttpPost]
        public IActionResult CreateReport(int orderId)
        {
            try
            {
                var a = _reportService.GenerateAndSaveReport(orderId);
                return Ok(a);

            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("v1/Report/GetById")]
        [HttpGet]
        public IActionResult GetReportById(int id)
        {
            try
            {
                var report = _reportService.GetReportById(id);
                if (report == null)
                {
                    return NotFound(new { Message = $"Report with ID {id} not found." });
                }
                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("v1/Report/GetAll")]
        [HttpGet]
        public IActionResult GetAllReports()
        {
            try
            {
                var reports = _reportService.GetAllReports();
                return Ok(reports);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("v1/Report/GetByOrderId")]
        [HttpGet]
        public IActionResult GetReportByOrderId(int orderId)
        {
            try
            {
                var report = _reportService.GetReportByOrderId(orderId);
                if (report == null)
                {
                    return NotFound(new { Message = $"Report for Order ID {orderId} not found." });
                }
                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("v1/Report/Delete")]
        [HttpDelete]
        public IActionResult DeleteOrder(int id)
        {
            try
            {
                _reportService.DeleteReport(id);
                return Ok("Report deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.DTOs;

namespace Web.Controllers
{
    [ApiController]
    [Authorize]
    public class CostController : Controller
    {
        private readonly ICostService _costService;

        public CostController(ICostService costService)
        {
            _costService = costService;
        }

        //[Route("v1/Cost/Create")]
        //[HttpPost]
        //public IActionResult CreateCost(CreateCostDTO dTO)
        //{
        //    try
        //    {
        //        var costDetails = _costService.CalculateCost(dTO);
        //        return Ok(costDetails);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [Route("v1/Cost/GetById")]
        [HttpGet]
        public IActionResult GetCostById(int id)
        {
            try
            {
                var cost = _costService.GetCostById(id);
                return Ok(cost);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [Route("v1/Cost/GetByOrderId")]
        [HttpGet]
        public IActionResult GetCostByOrderId(int orderId)
        {
            try
            {
                var cost = _costService.GetCostByOrderId(orderId);
                return Ok(cost);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [Route("v1/Cost/GetAll")]
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                IEnumerable<Cost> costs = _costService.GetAllCosts();
                return Ok(costs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("v1/Cost/Delete")]
        [HttpDelete]
        public IActionResult DeleteCustomer(int id)
        {
            try
            {
                _costService.DeleteCost(id);
                return Ok("Deleted Cost Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

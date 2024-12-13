using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs;
using Service;

namespace Web.Controllers
{
    [ApiController]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Route("v1/Order/Create")]
        [HttpPost]
        public IActionResult CreateOrder(CreateOrderDTO dTO)
        {
            try
            {
                var orderId = _orderService.AddOrder(dTO);
                return Ok($"Order created successfully with ID {orderId}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("v1/Order/GetById")]
        [HttpGet]
        public IActionResult GetOrderById(int id)
        {
            try
            {
                var order = _orderService.GetOrder(id);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("v1/Order/GetAll")]
        [HttpGet]
        public IActionResult GetAllOrders()
        {
            try
            {
                var orders = _orderService.GetAllOrders();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("v1/Order/Update")]
        [HttpPut]
        public IActionResult UpdateOrder(UpdateOrderDTO dTO)
        {
            try
            {
                _orderService.UpdateOrder(dTO);
                return Ok("Order updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("v1/Order/Delete")]
        [HttpDelete]
        public IActionResult DeleteOrder(int id)
        {
            try
            {
                _orderService.DeleteOrder(id);
                return Ok("Order deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

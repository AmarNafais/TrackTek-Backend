using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.DTOs;

namespace Web.Controllers;

[ApiController]
[Authorize]
public class OrderItemController : Controller
{
    private readonly IOrderItemService _orderItemService;

    public OrderItemController(IOrderItemService orderItemService)
    {
        _orderItemService = orderItemService;
    }

    [Route("v1/OrderItem/Create")]
    [HttpPost]
    public IActionResult CreateOrderItem(CreateOrderItemDTO dTO)
    {
        try
        {
            _orderItemService.AddOrderItem(dTO);
            return Ok("OrderItem created Successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("v1/OrderItem/GetById")]
    [HttpGet]
    public IActionResult GetOrderItemById(int id)
    {
        try
        {
            var orderItem = _orderItemService.GetOrderItem(id);
            return Ok(orderItem);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("v1/OrderItem/GetAll")]
    [HttpGet]
    public IActionResult GetAllOrderItems()
    {
        try
        {
            var orderItems = _orderItemService.GetAllOrderItems;
            return Ok(orderItems);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("v1/OrderItem/Update")]
    [HttpPut]
    public IActionResult UpdateOrderItem(UpdateOrderItemDTO dTO)
    {
        try
        {
            _orderItemService.UpdateOrderItem(dTO);
            return Ok("Updated OrderItem Successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("v1/OrderItem/Delete")]
    [HttpDelete]
    public IActionResult DeleteOrderItem(int id)
    {
        try
        {
            _orderItemService.DeleteOrderItem(id);
            return Ok("Deleted OrderItem Successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}


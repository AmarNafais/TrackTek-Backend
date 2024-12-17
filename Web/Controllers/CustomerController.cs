using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.DTOs;

namespace Web.Controllers;

[ApiController]
[Authorize]
public class CustomerController : Controller
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [Route("v1/Customer/Create")]
    [HttpPost]
    public IActionResult CreateUser(CreateCustomerDTO createCustomerDTO)
    {
        try
        {
            _customerService.AddCustomer(createCustomerDTO);
            return Ok("Customer created Successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("v1/Customer/GetById")]
    [HttpGet]
    public IActionResult GetCustomerById(int id)
    {
        try
        {
            var customer = _customerService.GetCustomer(id);
            return Ok(customer);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("v1/Customer/GetAll")]
    [HttpGet]
    public IActionResult GetAllCustomers()
    {
        try
        {
            var customers = _customerService.GetAllCustomers();
            return Ok(customers);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("v1/Customer/Update")]
    [HttpPut]
    public IActionResult UpdateCustomer(UpdateCustomerDTO updateCustomerDTO)
    {
        try
        {
            _customerService.UpdateCustomer(updateCustomerDTO);
            return Ok("Updated Customer Successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("v1/Customer/Delete")]
    [HttpDelete]
    public IActionResult DeleteCustomer(int id)
    {
        try
        {
            _customerService.DeleteCustomer(id);
            return Ok("Deleted Customer Successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}


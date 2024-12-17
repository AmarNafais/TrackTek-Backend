using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.DTOs;

namespace Web.Controllers;

[ApiController]
[Authorize]
public class SupplierController : Controller
{
    private readonly ISupplierService _supplierService;

    public SupplierController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    [Route("v1/Supplier/Create")]
    [HttpPost]
    public IActionResult CreateSupplier(CreateSupplierDTO createSupplierDTO)
    {
        try
        {
            _supplierService.AddSupplier(createSupplierDTO);
            return Ok("Supplier Created Successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("v1/Supplier/GetById")]
    [HttpGet]
    public IActionResult GetSupplier(int id)
    {
        try
        {
            var supplier = _supplierService.GetSupplier(id);
            return Ok(supplier);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("v1/Supplier/GetAll")]
    [HttpGet]
    public IActionResult GetAllSuppliers()
    {
        try
        {
            var suppliers = _supplierService.GetAllSuppliers();
            return Ok(suppliers);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("v1/Supplier/Update")]
    [HttpPut]
    public IActionResult UpdateSupplier(UpdateSupplierDTO updateSupplierDTO)
    {
        try
        {
            _supplierService.UpdateSupplier(updateSupplierDTO);
            return Ok("Updated Supplier Successfully");
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("v1/Supplier/Delete")]
    [HttpDelete]
    public IActionResult DeleteSupplier(int id)
    {
        try
        {
            _supplierService.DeleteSupplier(id);
            return Ok("Deleted Supplier Successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}


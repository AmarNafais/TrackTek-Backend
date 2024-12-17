using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.DTOs;

namespace Web.Controllers;

[ApiController]
[Authorize]
public class GarmentMachineController : Controller
{
    private readonly IGarmentMachineService _garmentMachineService;

    public GarmentMachineController(IGarmentMachineService garmentMachineService)
    {
        _garmentMachineService = garmentMachineService;
    }

    [Route("v1/GarmentMachine/Create")]
    [HttpPost]
    public IActionResult CreateGarmentMachine(CreateGarmentMachineDTO dTO)
    {
        try
        {
            _garmentMachineService.AddGarmentMachine(dTO);
            return Ok("GarmentMachine created Successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("v1/GarmentMachine/GetById")]
    [HttpGet]
    public IActionResult GetGarmentMachineById(int id)
    {
        try
        {
            var garmentMachine = _garmentMachineService.GetGarmentMachine(id);
            return Ok(garmentMachine);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("v1/GarmentMachine/GetAll")]
    [HttpGet]
    public IActionResult GetAllGarmentMachines()
    {
        try
        {
            var garmentMachines = _garmentMachineService.GetAllGarmentMachines();
            return Ok(garmentMachines);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("v1/GarmentMachine/Update")]
    [HttpPut]
    public IActionResult UpdateGarmentMachine(UpdateGarmentMachineDTO dTO)
    {
        try
        {
            _garmentMachineService.UpdateGarmentMachine(dTO);
            return Ok("Updated GarmentMachine Successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("v1/GarmentMachine/Delete")]
    [HttpDelete]
    public IActionResult DeleteGarmentMachine(int id)
    {
        try
        {
            _garmentMachineService.DeleteGarmentMachine(id);
            return Ok("Deleted GarmentMachine Successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}


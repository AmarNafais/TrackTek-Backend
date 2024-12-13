using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.DTOs;

namespace Web.Controllers;

[ApiController]
[Authorize]
public class MachineController : Controller
{
    private readonly IMachineService _machineService;

    public MachineController(IMachineService machineService)
    {
        _machineService = machineService;
    }

    [Route("v1/Machine/Create")]
    [HttpPost]
    public IActionResult CreateMachine(CreateMachineDTO createMachineDTO)
    {
        try
        {
            _machineService.AddMachine(createMachineDTO);
            return Ok("Machine Created Successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("v1/Machine/GetById")]
    [HttpGet]
    public IActionResult GetMachine(int id)
    {
        try
        {
            var machine = _machineService.GetMachine(id);
            return Ok(machine);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("v1/Machine/GetAll")]
    [HttpGet]
    public IActionResult GetAllMachines()
    {
        try
        {
            var machines = _machineService.GetAllMachines();
            return Ok(machines);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("v1/Machine/Update")]
    [HttpPut]
    public IActionResult UpdateMachine(UpdateMachineDTO updateMachineDTO)
    {
        try
        {
            _machineService.UpdateMachine(updateMachineDTO);
            return Ok("Updated Machine Successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("v1/Machine/Delete")]
    [HttpDelete]
    public IActionResult DeleteMachine(int id)
    {
        try
        {
            _machineService.DeleteMachine(id);
            return Ok("Deleted Machine Successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}


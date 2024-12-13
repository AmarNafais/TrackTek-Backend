using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs;
using Service;

namespace Web.Controllers;

[ApiController]
[Authorize]
public class GarmentController : Controller
{
    private readonly IGarmentService _garmentService;

    public GarmentController(IGarmentService garmentService)
    {
        _garmentService = garmentService;
    }

    [Route("v1/Garment/Create")]
    [HttpPost]
    public IActionResult CreateGarment(CreateGarmentDTO dTO)
    {
        try
        {
            _garmentService.AddGarment(dTO);
            return Ok("Garment Created Successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("v1/Garment/GetById")]
    [HttpGet]
    public IActionResult GetGarment(int id)
    {
        try
        {
            var garment = _garmentService.GetGarment(id);
            return Ok(garment);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("v1/Garment/GetAll")]
    [HttpGet]
    public IActionResult GetAllGarments()
    {
        try
        {
            var garments = _garmentService.GetAllGarments();
            return Ok(garments);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("v1/Garment/Update")]
    [HttpPut]
    public IActionResult UpdateGarment(UpdateGarmentDTO dTO)
    {
        try
        {
            _garmentService.UpdateGarment(dTO);
            return Ok("Updated Garment Successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("v1/Garment/Delete")]
    [HttpDelete]
    public IActionResult DeleteGarment(int id)
    {
        try
        {
            _garmentService.DeleteGarment(id);
            return Ok("Deleted Garment Successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.DTOs;

namespace Web.Controllers;

[ApiController]
[Authorize]
public class GarmentMaterialController : Controller
{
    private readonly IGarmentMaterialService _garmentMaterialService;

    public GarmentMaterialController(IGarmentMaterialService garmentMaterialService)
    {
        _garmentMaterialService = garmentMaterialService;
    }

    [Route("v1/GarmentMaterial/Create")]
    [HttpPost]
    public IActionResult CreateGarmentMaterial(CreateGarmentMaterialDTO dTO)
    {
        try
        {
            _garmentMaterialService.AddGarmentMaterial(dTO);
            return Ok("GarmentMaterial created Successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("v1/GarmentMaterial/GetById")]
    [HttpGet]
    public IActionResult GetGarmentMaterialById(int id)
    {
        try
        {
            var garmentMaterial = _garmentMaterialService.GetGarmentMaterial(id);
            return Ok(garmentMaterial);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("v1/GarmentMaterial/GetAll")]
    [HttpGet]
    public IActionResult GetAllGarmentMaterials()
    {
        try
        {
            var garmentMaterials = _garmentMaterialService.GetAllGarmentMaterials();
            return Ok(garmentMaterials);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("v1/GarmentMaterial/Update")]
    [HttpPut]
    public IActionResult UpdateGarmentMaterial(UpdateGarmentMaterialDTO dTO)
    {
        try
        {
            _garmentMaterialService.UpdateGarmentMaterial(dTO);
            return Ok("Updated GarmentMaterial Successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("v1/GarmentMaterial/Delete")]
    [HttpDelete]
    public IActionResult DeleteGarmentMaterial(int id)
    {
        try
        {
            _garmentMaterialService.DeleteGarmentMaterial(id);
            return Ok("Deleted GarmentMaterial Successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}


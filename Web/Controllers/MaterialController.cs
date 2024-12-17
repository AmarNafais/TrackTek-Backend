using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs;
using Service;
using Data.Entities;

namespace Web.Controllers;

[ApiController]
[Authorize]
public class MaterialController : Controller
{
    private readonly IMaterialService _materialService;

    public MaterialController(IMaterialService materialService)
    {
        _materialService = materialService;
    }

    [Route("v1/Material/Create")]
    [HttpPost]
    public IActionResult CreateUser(CreateMaterialDTO dTO)
    {
        try
        {
            _materialService.AddMaterial(dTO);
            return Ok("Material created Successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("v1/Material/GetById")]
    [HttpGet]
    public IActionResult GetMaterialById(int id)
    {
        try
        {
            var material = _materialService.GetMaterial(id);
            return Ok(material);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("v1/Material/GetAll")]
    [HttpGet]
    public IActionResult GetAllMaterials()
    {
        try
        {
            var materials = _materialService.GetAllMaterials();
            return Ok(materials);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("v1/Material/Update")]
    [HttpPut]
    public IActionResult UpdateMaterial(UpdateMaterialDTO dTO)
    {
        try
        {
            _materialService.UpdateMaterial(dTO);
            return Ok("Updated Material Successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("v1/Material/Delete")]
    [HttpDelete]
    public IActionResult DeleteMaterial(int id)
    {
        try
        {
            _materialService.DeleteMaterial(id);
            return Ok("Deleted Material Successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

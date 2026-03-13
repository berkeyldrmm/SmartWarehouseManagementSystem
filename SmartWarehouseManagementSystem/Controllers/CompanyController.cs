using EntityLayer.Dtos.Companies.Requests;
using ManagerLayer.Authorization;
using ManagerLayer.Services.Abstract.Companies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartWarehouseManagementSystem.Wrappers;

namespace SmartWarehouseManagementSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpGet]
    [RoleValidation("Admin")]
    public async Task<IActionResult> GetAllAsync()
    {
        var data = await _companyService.GetAllAsync();
        return Ok(new DataResponse<object>(data));
    }

    [HttpGet("{id:guid}")]
    [RoleValidation("Admin")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        try
        {
            var data = await _companyService.GetByIdAsync(id);
            return Ok(new DataResponse<object>(data));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ErrorResponse(404, [ex.Message]));
        }
    }

    [HttpPost]
    [RoleValidation("Admin")]
    public async Task<IActionResult> AddAsync([FromBody] CreateCompanyDto request)
    {
        var result = await _companyService.AddAsync(request);
        if (!result)
        {
            return BadRequest(new ErrorResponse(400, ["Þirket eklenemedi."]));
        }

        return Ok(new DataResponse<bool>(true));
    }

    [HttpPost("update/{id:guid}")]
    [RoleValidation("Admin")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateCompanyDto request)
    {
        try
        {
            var result = await _companyService.UpdateAsync(id, request);
            if (!result)
            {
                return BadRequest(new ErrorResponse(400, ["Þirket güncellenemedi."]));
            }

            return Ok(new DataResponse<bool>(true));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ErrorResponse(404, [ex.Message]));
        }
    }

    [HttpPost("delete/{id:guid}")]
    [RoleValidation("Admin")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        try
        {
            var result = await _companyService.DeleteAsync(id);
            if (!result)
            {
                return BadRequest(new ErrorResponse(400, ["Þirket silinemedi."]));
            }

            return Ok(new DataResponse<bool>(true));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ErrorResponse(404, [ex.Message]));
        }
    }
}

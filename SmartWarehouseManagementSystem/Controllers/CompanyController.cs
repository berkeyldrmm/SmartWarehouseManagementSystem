using EntityLayer.Dtos.Companies.Requests;
using ManagerLayer.Authorization;
using ManagerLayer.Services.Abstract.Companies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.UnitOfWorks.Abstraction;
using SmartWarehouseManagementSystem.Wrappers;

namespace SmartWarehouseManagementSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;
    private readonly IUnitOfWork _unitOfWork;

    public CompanyController(ICompanyService companyService, IUnitOfWork unitOfWork)
    {
        _companyService = companyService;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    [RoleValidation("Admin")]
    public async Task<IActionResult> GetAllAsync()
    {
        var data = await _companyService.GetAllAsync();
        await _unitOfWork.SaveChangesAsync();
        return Ok(new DataResponse<object>(data));
    }

    [HttpGet("{id:guid}")]
    [RoleValidation("Admin")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        try
        {
            var data = await _companyService.GetByIdAsync(id);
            await _unitOfWork.SaveChangesAsync();
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
        await _unitOfWork.SaveChangesAsync();
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
            await _unitOfWork.SaveChangesAsync();
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
            await _unitOfWork.SaveChangesAsync();
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

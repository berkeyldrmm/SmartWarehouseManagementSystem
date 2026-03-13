using EntityLayer.Dtos.Warehouses.Requests;
using ManagerLayer.Authorization;
using ManagerLayer.Services.Abstract.Warehouses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartWarehouseManagementSystem.Wrappers;

namespace SmartWarehouseManagementSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class WarehouseController : BaseController
{
    private readonly IWarehouseService _warehouseService;

    public WarehouseController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    [HttpGet]
    [RoleValidation("User")]
    public async Task<IActionResult> GetAllForUserAsync()
    {
        var data = await _warehouseService.GetAllForUserAsync(UserId.ToString());
        return Ok(new DataResponse<object>(data));
    }

    [HttpGet("{id:guid}")]
    [RoleValidation("User")]
    public async Task<IActionResult> GetByIdForUserAsync(Guid id)
    {
        try
        {
            var data = await _warehouseService.GetByIdForUserAsync(id, UserId.ToString());
            return Ok(new DataResponse<object>(data));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ErrorResponse(404, [ex.Message]));
        }
    }

    [HttpPost]
    [RoleValidation("User")]
    public async Task<IActionResult> AddForUserAsync([FromBody] CreateWarehouseDto request)
    {
        var result = await _warehouseService.AddForUserAsync(request, UserId.ToString());
        if (!result)
        {
            return BadRequest(new ErrorResponse(400, ["Depo eklenemedi."]));
        }

        return Ok(new DataResponse<bool>(true));
    }

    [HttpPost("update/{id:guid}")]
    [RoleValidation("User")]
    public async Task<IActionResult> UpdateForUserAsync(Guid id, [FromBody] UpdateWarehouseDto request)
    {
        try
        {
            var result = await _warehouseService.UpdateForUserAsync(id, request, UserId.ToString());
            if (!result)
            {
                return BadRequest(new ErrorResponse(400, ["Depo güncellenemedi."]));
            }

            return Ok(new DataResponse<bool>(true));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ErrorResponse(404, [ex.Message]));
        }
    }

    [HttpPost("delete/{id:guid}")]
    [RoleValidation("User")]
    public async Task<IActionResult> DeleteForUserAsync(Guid id)
    {
        try
        {
            var result = await _warehouseService.DeleteForUserAsync(id, UserId.ToString());
            if (!result)
            {
                return BadRequest(new ErrorResponse(400, ["Depo silinemedi."]));
            }

            return Ok(new DataResponse<bool>(true));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ErrorResponse(404, [ex.Message]));
        }
    }
}

using EntityLayer.Dtos.Products;
using ManagerLayer.Authorization;
using ManagerLayer.Services.Abstract.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Pagination;
using RepositoryLayer.UnitOfWorks.Abstraction;
using SmartWarehouseManagementSystem.Wrappers;

namespace SmartWarehouseManagementSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductController : BaseController
{
    private readonly IProductService _productService;
    private readonly IUnitOfWork _unitOfWork;

    public ProductController(IProductService productService, IUnitOfWork unitOfWork)
    {
        _productService = productService;
        _unitOfWork = unitOfWork;
    }

    [HttpGet("admin/all")]
    [RoleValidation("Admin")]
    public async Task<IActionResult> GetAllForAdminAsync()
    {
        var products = await _productService.GetAllAsync(ProductSelectors.Admin, null, true);
        return Ok(new DataResponse<IEnumerable<ProductAdminDto>>(products));
    }

    [HttpGet("admin/{id:guid}")]
    [RoleValidation("Admin")]
    public async Task<IActionResult> GetByIdForAdminAsync(Guid id)
    {
        try
        {
            var product = await _productService.GetByIdAsync(id, ProductSelectors.Admin, null, true);
            return Ok(new DataResponse<ProductAdminDto>(product));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ErrorResponse(404, [ex.Message]));
        }
    }

    [HttpGet("admin/filter")]
    [RoleValidation("Admin")]
    public async Task<IActionResult> GetByFiltersForAdminAsync([FromQuery] ProductFilterDto filter)
    {
        var products = await _productService.GetByFiltersAsync(filter, ProductSelectors.Admin, null, true);
        return Ok(new DataResponse<IEnumerable<ProductAdminDto>>(products));
    }

    [HttpGet("admin/paged")]
    [RoleValidation("Admin")]
    public async Task<IActionResult> GetPagedDataListForAdminAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] ProductFilterDto? filter = null)
    {
        var pagedData = await _productService.GetPagedDataListAsync(pageNumber, pageSize, filter ?? new ProductFilterDto(), ProductSelectors.Admin, null, true);
        return Ok(new DataResponse<PagedDataListModel<ProductAdminDto>>(pagedData));
    }

    [HttpGet("admin/count")]
    [RoleValidation("Admin")]
    public async Task<IActionResult> CountAsync([FromQuery] ProductFilterDto filter)
    {
        var count = await _productService.CountAsync(filter, null, true);
        return Ok(new DataResponse<int>(count));
    }

    [HttpPost("admin")]
    [RoleValidation("Admin")]
    public async Task<IActionResult> AddAsync([FromBody] CreateProductDto product)
    {
        var result = await _productService.AddAsync(product, null, true);
        await _unitOfWork.SaveChangesAsync();
        if (!result)
        {
            return BadRequest(new ErrorResponse(400, ["Ürün eklenemedi."]));
        }

        return Ok(new DataResponse<bool>(true));
    }

    [HttpPost("admin/update/{id:guid}")]
    [RoleValidation("Admin")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateProductDto product)
    {
        try
        {
            var result = await _productService.UpdateAsync(id, product, null, true);
            await _unitOfWork.SaveChangesAsync();
            if (!result)
            {
                return BadRequest(new ErrorResponse(400, ["Ürün güncellenemedi."]));
            }

            return Ok(new DataResponse<bool>(true));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ErrorResponse(404, [ex.Message]));
        }
    }

    [HttpPost("admin/delete/{id:guid}")]
    [RoleValidation("Admin")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        try
        {
            var result = await _productService.DeleteAsync(id, null, true);
            await _unitOfWork.SaveChangesAsync();
            if (!result)
            {
                return BadRequest(new ErrorResponse(400, ["Ürün silinemedi."]));
            }

            return Ok(new DataResponse<bool>(true));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ErrorResponse(404, [ex.Message]));
        }
    }

    [HttpPost("admin/delete-range")]
    [RoleValidation("Admin")]
    public async Task<IActionResult> DeleteRangeAsync([FromBody] IEnumerable<Guid> ids)
    {
        await _productService.DeleteRangeAsync(ids, null, true);
        await _unitOfWork.SaveChangesAsync();
        
        return Ok(new DataResponse<bool>(true));
    }

    [HttpGet("user/all")]
    [RoleValidation("User")]
    public async Task<IActionResult> GetAllForUserAsync()
    {
        var products = await _productService.GetAllAsync(ProductSelectors.User, UserId.ToString(), false);
        return Ok(new DataResponse<IEnumerable<ProductUserDto>>(products));
    }

    [HttpGet("user/{id:guid}")]
    [RoleValidation("User")]
    public async Task<IActionResult> GetByIdForUserAsync(Guid id)
    {
        try
        {
            var product = await _productService.GetByIdAsync(id, ProductSelectors.User, UserId.ToString(), false);
            return Ok(new DataResponse<ProductUserDto>(product));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ErrorResponse(404, [ex.Message]));
        }
    }

    [HttpGet("user/filter")]
    [RoleValidation("User")]
    public async Task<IActionResult> GetByFiltersForUserAsync([FromQuery] ProductFilterDto filter)
    {
        var products = await _productService.GetByFiltersAsync(filter, ProductSelectors.User, UserId.ToString(), false);
        return Ok(new DataResponse<IEnumerable<ProductUserDto>>(products));
    }

    [HttpGet("user/paged")]
    [RoleValidation("User")]
    public async Task<IActionResult> GetPagedDataListForUserAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] ProductFilterDto? filter = null)
    {
        var pagedData = await _productService.GetPagedDataListAsync(pageNumber, pageSize, filter ?? new ProductFilterDto(), ProductSelectors.User, UserId.ToString(), false);
        return Ok(new DataResponse<PagedDataListModel<ProductUserDto>>(pagedData));
    }

    [HttpPost("user")]
    [RoleValidation("User")]
    public async Task<IActionResult> AddForUserAsync([FromBody] CreateProductForUserDto product)
    {
        var result = await _productService.AddForUserAsync(product, UserId.ToString());
        if (!result)
        {
            return BadRequest(new ErrorResponse(400, ["Ürün eklenemedi."]));
        }

        return Ok(new DataResponse<bool>(true));
    }

    [HttpPost("user/update/{id:guid}")]
    [RoleValidation("User")]
    public async Task<IActionResult> UpdateForUserAsync(Guid id, [FromBody] UpdateProductDto product)
    {
        try
        {
            var result = await _productService.UpdateAsync(id, product, UserId.ToString(), false);
            if (!result)
            {
                return BadRequest(new ErrorResponse(400, ["Ürün güncellenemedi."]));
            }

            return Ok(new DataResponse<bool>(true));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ErrorResponse(404, [ex.Message]));
        }
    }

    [HttpPost("user/delete/{id:guid}")]
    [RoleValidation("User")]
    public async Task<IActionResult> DeleteForUserAsync(Guid id)
    {
        try
        {
            var result = await _productService.DeleteAsync(id, UserId.ToString(), false);
            if (!result)
            {
                return BadRequest(new ErrorResponse(400, ["Ürün silinemedi."]));
            }

            return Ok(new DataResponse<bool>(true));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ErrorResponse(404, [ex.Message]));
        }
    }

    [HttpPost("user/delete-range")]
    [RoleValidation("User")]
    public async Task<IActionResult> DeleteRangeForUserAsync([FromBody] IEnumerable<Guid> ids)
    {
        await _productService.DeleteRangeAsync(ids, UserId.ToString(), false);
        await _unitOfWork.SaveChangesAsync();

        return Ok(new DataResponse<bool>(true));
    }

    [HttpPost("user/warehouse/add/{productId:guid}")]
    [RoleValidation("User")]
    public async Task<IActionResult> AddProductToWarehouseForUserAsync(Guid productId, [FromBody] AddProductToWarehouseDto request)
    {
        try
        {
            var result = await _productService.AddProductToWarehouseAsync(productId, request, UserId.ToString());
            if (!result)
            {
                return BadRequest(new ErrorResponse(400, ["Ürün depoya eklenemedi."]));
            }

            await _unitOfWork.SaveChangesAsync();
            return Ok(new DataResponse<bool>(true));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ErrorResponse(404, [ex.Message]));
        }
    }

    [HttpPost("user/warehouse/remove/{productId:guid}/{warehouseId:guid}")]
    [RoleValidation("User")]
    public async Task<IActionResult> RemoveProductFromWarehouseForUserAsync(Guid productId, Guid warehouseId)
    {
        try
        {
            var result = await _productService.RemoveProductFromWarehouseAsync(productId, warehouseId, UserId.ToString());
            if (!result)
            {
                return BadRequest(new ErrorResponse(400, ["Ürün depodan kaldırılamadı."]));
            }

            await _unitOfWork.SaveChangesAsync();
            return Ok(new DataResponse<bool>(true));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ErrorResponse(404, [ex.Message]));
        }
    }

    [HttpPost("user/warehouse/decrease-stock/{productId:guid}")]
    [RoleValidation("User")]
    public async Task<IActionResult> DecreaseWarehouseStockForUserAsync(Guid productId, [FromBody] DecreaseWarehouseStockDto request)
    {
        try
        {
            var result = await _productService.DecreaseWarehouseStockAsync(productId, request, UserId.ToString());
            if (!result)
            {
                return BadRequest(new ErrorResponse(400, ["Ürün stoğu düşürülemedi."]));
            }

            await _unitOfWork.SaveChangesAsync();
            return Ok(new DataResponse<bool>(true));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ErrorResponse(404, [ex.Message]));
        }
    }
}

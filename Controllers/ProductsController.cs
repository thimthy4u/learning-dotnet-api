// Controllers/ProductsController.cs
using Microsoft.AspNetCore.Mvc;
using dotnet_api.Dtos;
using dotnet_api.Services;
using dotnet_api.Exceptions;
namespace dotnet_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetAll() => Ok(await service.GetAllAsync());

    [HttpGet("count")]
    public async Task<ActionResult<int>> Count() => Ok(await service.CountAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDto>> GetById(int id) => Ok(await service.GetByIdAsync(id));

    [HttpGet("search")]
    public async Task<ActionResult<List<ProductDto>>> GetByName([FromQuery] string? name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return BadRequest(new {message = "Product not found"});
        }
        return Ok(await service.GetByNameAsync(name));
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create(ProductCreateDto dto)
    {
        var created = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductDto>> Update(int id, ProductUpdateDto dto) =>
        Ok(await service.UpdateAsync(id, dto));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }


}

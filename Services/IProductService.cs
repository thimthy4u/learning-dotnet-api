// Services/IProductService.cs
using dotnet_api.Dtos;
namespace dotnet_api.Services;

public interface IProductService
{
    Task<List<ProductDto>> GetAllAsync();
    Task<List<ProductDto>> GetByNameAsync(string name);
    Task<ProductDto> GetByIdAsync(int id);
    Task<ProductDto> CreateAsync(ProductCreateDto dto);
    Task<ProductDto> UpdateAsync(int id, ProductUpdateDto dto);
    Task DeleteAsync(int id);
    Task<int> CountAsync();
}

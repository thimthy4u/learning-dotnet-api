// Mappers/ProductMapper.cs
using Riok.Mapperly.Abstractions;
using dotnet_api.Dtos;
using dotnet_api.Entities;
namespace dotnet_api.Mappers;

[Mapper]
public partial class ProductMapper
{
    public partial ProductDto ToDto(Product product);
    public partial List<ProductDto> ToDtos(List<Product> products);

    [MapperIgnoreTarget(nameof(Product.Id))]
    [MapperIgnoreTarget(nameof(Product.CreatedAt))]
    public partial Product ToEntity(ProductCreateDto dto);

    [MapperIgnoreTarget(nameof(Product.Id))]
    [MapperIgnoreTarget(nameof(Product.CreatedAt))]
    public partial void UpdateEntity(ProductUpdateDto dto, Product product);
}

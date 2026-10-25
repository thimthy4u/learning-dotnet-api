// Dtos/ProductDtos.cs
using System.ComponentModel.DataAnnotations;
namespace dotnet_api.Dtos;

public record ProductCreateDto(
    [Required, MaxLength(100)] string Name,
    [Range(0.01, 1_000_000)] decimal Price,
    [Range(0, int.MaxValue)] int Stock);

public record ProductUpdateDto(
    [Required, MaxLength(100)] string Name,
    [Range(0.01, 1_000_000)] decimal Price,
    [Range(0, int.MaxValue)] int Stock);

public record ProductDto(int Id, string Name, decimal Price, int Stock, DateTime CreatedAt);

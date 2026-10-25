// Services/ProductService.cs
using Microsoft.EntityFrameworkCore;
using dotnet_api.Data;
using dotnet_api.Dtos;
using dotnet_api.Exceptions;
using dotnet_api.Mappers;
namespace dotnet_api.Services;

public class ProductService(AppDbContext db, ProductMapper mapper) : IProductService
{
    public async Task<List<ProductDto>> GetAllAsync() =>
        mapper.ToDtos(await db.Products.AsNoTracking().OrderBy(p => p.Id).ToListAsync());

    public async Task<ProductDto> GetByIdAsync(int id)
    {
        var product = await db.Products.FindAsync(id)
            ?? throw new NotFoundException($"Product {id} not found");
        return mapper.ToDto(product);
    }

    public async Task<List<ProductDto>> GetByNameAsync(string? searchName)
    {
        // 1. Initialize the queryable object from the DbSet
        var query = db.Products.AsNoTracking();

        // 2. Apply filtering logic if a name search term is provided
        if (!string.IsNullOrWhiteSpace(searchName))
        {
            string name = searchName.ToLower();

            // Update line 39 in ProductService.cs
            query = query.Where(p => EF.Functions.ILike(p.Name, $"%{searchName}%"));

            // query = query.Where(p => p.Name.ToLower().StartsWith(name) ||
            //                          p.Name.ToLower().EndsWith(name));
        }

        // 3. Execute the query asynchronously against the database
        var products = await query.ToListAsync();

        // 4. Handle a "Not Found" business rule scenario if no matches exist
        if (products == null || !products.Any())
        {
            throw new NotFoundException($"No products found matching the name: '{searchName}'");
        }

        // 5. Map the database entities back to data transfer objects (DTOs)
        return mapper.ToDtos(products);
    }


    public async Task<ProductDto> CreateAsync(ProductCreateDto dto)
    {
        // Business rule: names must be unique
        if (await db.Products.AnyAsync(p => p.Name == dto.Name))
            throw new ConflictException($"Product '{dto.Name}' already exists");

        var product = mapper.ToEntity(dto);
        db.Products.Add(product);
        await db.SaveChangesAsync();
        return mapper.ToDto(product);
    }

    public async Task<ProductDto> UpdateAsync(int id, ProductUpdateDto dto)
    {
        var product = await db.Products.FindAsync(id)
            ?? throw new NotFoundException($"Product {id} not found");

        if (await db.Products.AnyAsync(p => p.Name == dto.Name && p.Id != id))
            throw new ConflictException($"Product '{dto.Name}' already exists");

        // Business rule: price can't change by more than 50% at once
        if (product.Price > 0 && Math.Abs(dto.Price - product.Price) / product.Price > 0.5m)
            throw new BusinessRuleException("Price cannot change by more than 50% in one update");

        mapper.UpdateEntity(dto, product);
        await db.SaveChangesAsync();
        return mapper.ToDto(product);
    }

    public async Task DeleteAsync(int id)
    {
        var product = await db.Products.FindAsync(id)
            ?? throw new NotFoundException($"Product {id} not found");

        // Business rule: can't delete products that still have stock
        if (product.Stock > 0)
            throw new BusinessRuleException("Cannot delete a product that still has stock");

        db.Products.Remove(product);
        await db.SaveChangesAsync();
    }

    public async Task<int> CountAsync() => await db.Products.CountAsync();


}

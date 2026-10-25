// Exceptions/GlobalExceptionHandler.cs
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
namespace dotnet_api.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext ctx, Exception ex, CancellationToken ct)
    {
        var (status, title) = ex switch
        {
            NotFoundException => (404, "Not Found"),
            ConflictException => (409, "Conflict"),
            BusinessRuleException => (422, "Business rule violated"),
            _ => (500, "Server error")
        };

        ctx.Response.StatusCode = status;
        await ctx.Response.WriteAsJsonAsync(
            new ProblemDetails { Status = status, Title = title, Detail = status == 500 ? null : ex.Message }, ct);
        return true;
    }
}

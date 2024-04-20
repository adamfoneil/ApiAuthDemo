using ApiAuthDemo.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiAuthDemo;

internal static partial class Program
{
    public static void MapQueries(this IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("/api/Queries").RequireAuthorization();

        group.MapGet("MyWidgets", async (ApplicationDbContext db, HttpRequest request) =>
        {
            var user = request.HttpContext.User;
            var userName = user.Identity?.Name ?? "<anon>";
            var widgets = await db.Widgets.Where(row => row.CreatedBy == userName).ToListAsync();
        });
    }
}

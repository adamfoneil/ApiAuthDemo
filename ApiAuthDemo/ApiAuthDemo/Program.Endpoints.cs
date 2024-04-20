using ApiAuthDemo.Data;
using ApiAuthDemo.Data.Entities;
using ApiAuthDemo.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ApiAuthDemo;

internal static partial class Program
{
    public static void MapApiEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("/api").RequireAuthorization();

        group.MapGet("/Queries/MyWidgets", async (ApplicationDbContext db, HttpRequest request) =>
        {
            var userName = GetUserName(request);
            return await db.Widgets.Where(row => row.CreatedBy == userName).ToListAsync();            
        });

        group.MapPost("/Widgets", async (ApplicationDbContext db, HttpRequest request, Widget data) =>
        {
            db.UserName = GetUserName(request);
			db.Widgets.Save(data);   
			await db.SaveChangesAsync();
			return Results.Ok(data);
		});

        group.MapDelete("/Widgets/{id}", async (ApplicationDbContext db, HttpRequest request, int id) =>
        {
            var userName = GetUserName(request);
            await db.Widgets.Where(row => row.Id == id && row.CreatedBy == userName).ExecuteDeleteAsync();
            return Results.Ok();
        });
    }

    private static string GetUserName(HttpRequest request)
    {
		var user = request.HttpContext.User;
		return user.Identity?.Name ?? "<anon>";
	}
}

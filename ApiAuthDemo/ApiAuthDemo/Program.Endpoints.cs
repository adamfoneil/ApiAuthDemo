using ApiAuthDemo.Data;
using ApiAuthDemo.Data.Entities;
using ApiAuthDemo.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ApiAuthDemo;

internal static partial class Program
{
	public static void MapApiEndpoints(this IEndpointRouteBuilder routeBuilder)
	{
		var group = routeBuilder.MapGroup("/api").RequireAuthorization();

		group.MapGet("/Queries/MyWidgets", async (ApplicationDbContext db, HttpRequest request) =>
		{
			var (userName, _) = GetUserInfo(request);
			db.UserName = userName;
			return await db.Widgets.Where(row => row.CreatedBy == userName).OrderBy(row => row.Name).ToListAsync();
		});

		group.MapPost("/Widgets", async (ApplicationDbContext db, HttpRequest request, Widget data) =>
		{
			var (userName, timeZoneId) = GetUserInfo(request);
			db.UserName = userName;
			db.TimeZoneId = timeZoneId;
			db.Widgets.Save(data);
			await db.SaveChangesAsync();
			return Results.Ok(data);
		});

		group.MapDelete("/Widgets/{id}", async (ApplicationDbContext db, HttpRequest request, int id) =>
		{
			var (userName, _) = GetUserInfo(request);
			await db.Widgets.Where(row => row.Id == id && row.CreatedBy == userName).ExecuteDeleteAsync();
			return Results.Ok();
		});
	}

	private static (string UserName, string? TimeZoneId) GetUserInfo(HttpRequest request)
	{
		var user = request.HttpContext.User;
		var timeZoneId = user.FindFirstValue(nameof(ApplicationUser.TimeZoneId));
		return (user.Identity?.Name ?? "<anon>", timeZoneId);
	}
}

using ApiAuthDemo.Data.Entities;
using ApiAuthDemo.Data.Entities.Conventions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ApiAuthDemo.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
	public string UserName { get; set; } = "system";
	public string? TimeZoneId { get; set; }

	public DbSet<Widget> Widgets { get; set; }

	public override int SaveChanges()
	{
		AuditEntities();
		return base.SaveChanges();
	}

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		AuditEntities();
		return base.SaveChangesAsync(cancellationToken);
	}

	private void AuditEntities()
	{
		foreach (var entity in ChangeTracker.Entries<BaseEntity>())
		{
			switch (entity.State)
			{
				case EntityState.Added:
					entity.Entity.CreatedBy = UserName;
					entity.Entity.DateCreated = LocalDateTime(TimeZoneId);
					break;
				case EntityState.Modified:
					entity.Entity.ModifiedBy = UserName;
					entity.Entity.DateModified = LocalDateTime(TimeZoneId);
					break;
			}
		}
	}

	private static DateTime LocalDateTime(string? timeZoneId)
	{
		var now = DateTime.UtcNow;
		if (timeZoneId == null) return now;

		var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
		return TimeZoneInfo.ConvertTimeFromUtc(now, timeZone);
	}
}

/// <summary>
/// because I'm applying migrations from the command line, I need to implement this interface
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
	private static IConfiguration Config => new ConfigurationBuilder()
		.AddJsonFile("appsettings.json", optional: false)
		.Build();

	public ApplicationDbContext CreateDbContext(string[] args)
	{
		var connectionString = Config.GetConnectionString("DefaultConnection");
		var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
		builder.UseSqlServer(connectionString);
		return new ApplicationDbContext(builder.Options);
	}
}


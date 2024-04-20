using ApiAuthDemo.Data.Entities.Conventions;
using Microsoft.EntityFrameworkCore;

namespace ApiAuthDemo.Extensions;

internal static class DbSetExtensions
{
	public static void Save<TEntity>(this DbSet<TEntity> entities, TEntity entity) where TEntity : BaseEntity
	{
		if (entity.Id == default)
		{
			entities.Add(entity);
		}
		else
		{
			entities.Update(entity);
		}
	}
}

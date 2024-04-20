using ApiAuthDemo.Data.Entities.Conventions;
using System.ComponentModel.DataAnnotations;

namespace ApiAuthDemo.Data.Entities;

public class Widget : BaseEntity
{
	[MaxLength(100)]
	public string Name { get; set; } = string.Empty;
	[MaxLength(255)]
	public string? Description { get; set; }
	public decimal Price { get; set; }
}

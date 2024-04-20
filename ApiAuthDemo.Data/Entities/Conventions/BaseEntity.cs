using System.ComponentModel.DataAnnotations;

namespace ApiAuthDemo.Data.Entities.Conventions;

public abstract class BaseEntity
{
	public int Id { get; set; }

	[MaxLength(50)]
	public string CreatedBy { get; set; } = "<anon>";
	public DateTime DateCreated { get; set; } = DateTime.UtcNow;

	[MaxLength(50)]
	public string? ModifiedBy { get; set; }
	public DateTime? DateModified { get; set; }
}

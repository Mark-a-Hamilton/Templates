namespace API.Entities;

/// <summary>
/// Base Entity creates a foundation for all entities in the system
/// </summary>
public abstract class BaseEntity
{
    [Key]
    public int PK_Id { get; set; }

    [Required]
    public DateTime CreatedOn { get; set; }

    [Required]
    [MaxLength(50)]
    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    [MaxLength(50)]
    public string? ModifiedBy { get; set; }
}
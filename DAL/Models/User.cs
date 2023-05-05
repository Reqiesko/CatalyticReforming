using System.ComponentModel.DataAnnotations;


namespace DAL;

public class User : EntityBase
{
    [Required]
    [StringLength(50)]
    public string Username { get; set; }

    [Required]
    [StringLength(50)]
    public string Password { get; set; }

    [Required]
    [StringLength(50)]
    public virtual UserRole Role { get; set; }

    public int RoleId { get; set; }

    public bool Access { get; set; }
}

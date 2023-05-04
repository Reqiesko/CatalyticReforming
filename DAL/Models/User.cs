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
    public string Role { get; set; }

    public bool Access { get; set; }
}

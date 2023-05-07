namespace DAL.Models.domain;

public class Installation : EntityBase
{
    public string Name { get; set; }
    public int ReactorId { get; set; }
    public virtual Reactor Reactor { get; set; }
}



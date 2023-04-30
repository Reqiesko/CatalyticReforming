namespace DAL;

public class Answer : EntityBase
{
    public string Text { get; set; }
    
    public bool IsSelected { get; set; }
}
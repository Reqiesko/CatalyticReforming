namespace DAL;

public class Answer : EntityBase
{
    public string Text { get; set; }
    public bool IsCorrect { get; set; }

    public Question Question { get; set; }
    public int QuestionId { get; set; }
}
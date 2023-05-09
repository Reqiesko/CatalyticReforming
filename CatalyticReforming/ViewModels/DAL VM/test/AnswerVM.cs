namespace CatalyticReforming.ViewModels.DAL_VM.test;

public class AnswerVM : ValidatableViewModel<AnswerValidator>, IDALVM
{
    public string Text { get; set; }
    public bool IsCorrect { get; set; }
    public int QuestionId { get; set; }

    public bool IsSelected { get; set; }
    public int Id { get; set; }
}



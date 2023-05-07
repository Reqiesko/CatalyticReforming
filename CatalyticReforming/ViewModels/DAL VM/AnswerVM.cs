using System;


namespace CatalyticReforming.ViewModels.DAL_VM;

public class AnswerVM : ViewModelBase, IDALVM 
{
    public string Text { get; set; }
    public bool IsCorrect { get; set; }
    public int QuestionId { get; set; }
    public int Id { get; set; }
    
    public bool IsSelected { get; set; }
}


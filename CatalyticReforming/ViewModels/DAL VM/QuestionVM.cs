using System.Collections.ObjectModel;


namespace CatalyticReforming.ViewModels.DAL_VM;

public class QuestionVM : ViewModelBase, IDALVM
{
    public string Text { get; set; }

    public ObservableCollection<AnswerVM> Answers { get; set; } = new();
    public int Id { get; set; }
}



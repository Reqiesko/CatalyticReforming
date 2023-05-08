namespace CatalyticReforming.ViewModels.DAL_VM.auth;

public class TestConfigVM : ViewModelBase, IDALVM
{
    public int Id { get; set; }
    public int NumberOfQuestions { get; set; }
    public int NumberOfQuestionsToPass { get; set; }
}

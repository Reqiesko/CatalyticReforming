using CatalyticReforming.Views.Shared;


namespace CatalyticReforming.Views.Admin.Testing;

public partial class EditQuestionControl : IViewWithVM<EditQuestionControlVM>
{
    public EditQuestionControl()
    {
        InitializeComponent();
        ViewModel = App.GetService<EditQuestionControlVM>();
        DataContext = this;
    }

    public EditQuestionControlVM ViewModel { get; set; }
}



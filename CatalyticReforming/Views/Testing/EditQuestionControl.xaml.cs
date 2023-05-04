using CatalyticReforming.ViewModels;
using CatalyticReforming.ViewModels.Testing;


namespace CatalyticReforming.Views.Testing;

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



using CatalyticReforming.Views.Shared;


namespace CatalyticReforming.Views.Researcher;

public partial class StudyControl : IViewWithVM<StudyControlVM>
{
    public StudyControl() 
    {
        InitializeComponent();
        ViewModel = App.GetService<StudyControlVM>();
        DataContext = this;
    }

    public StudyControlVM ViewModel { get; set; }
}

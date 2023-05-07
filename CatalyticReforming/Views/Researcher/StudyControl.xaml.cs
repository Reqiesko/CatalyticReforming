using System.Windows.Controls;

namespace CatalyticReforming.Views;

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

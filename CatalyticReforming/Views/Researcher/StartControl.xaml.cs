using CatalyticReforming.Views.Shared;


namespace CatalyticReforming.Views.Researcher;

public partial class StartControl : IViewWithVM<StartControlVM>
{
    public StartControl()
    {
        InitializeComponent();
        ViewModel = App.GetService<StartControlVM>();
        DataContext = this;
    }

    public StartControlVM ViewModel { get; set; }
}

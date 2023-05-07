using CatalyticReforming.ViewModels;


namespace CatalyticReforming.Views;

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

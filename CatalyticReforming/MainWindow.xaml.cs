using CatalyticReforming.ViewModels;
using CatalyticReforming.Views;


namespace CatalyticReforming;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : IViewWithVM<MainViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
        ViewModel = App.GetService<MainViewModel>();
        DataContext = ViewModel;
    }

    public MainViewModel ViewModel { get; set; }
}

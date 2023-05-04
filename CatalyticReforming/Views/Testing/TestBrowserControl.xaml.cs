using CatalyticReforming.ViewModels;
using CatalyticReforming.ViewModels.Testing;


namespace CatalyticReforming.Views.Testing;

public partial class TestBrowserControl : IViewWithVM<TestBrowserControlVM>
{
    public TestBrowserControl()
    {
        InitializeComponent();
        ViewModel = App.GetService<TestBrowserControlVM>();
        DataContext = this;
    }

    public TestBrowserControlVM ViewModel { get; set; }
}



using CatalyticReforming.Views.Shared;


namespace CatalyticReforming.Views.Admin.Testing;

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



using System.Windows.Controls;
using CatalyticReforming.ViewModels;

namespace CatalyticReforming.Views;

public partial class ResearchControl : IViewWithVM<ResearchControlVM>
{
    public ResearchControl()
    {
        InitializeComponent();
        ViewModel = App.GetService<ResearchControlVM>();
        DataContext = this;
    }

    public ResearchControlVM ViewModel { get; set; }
}
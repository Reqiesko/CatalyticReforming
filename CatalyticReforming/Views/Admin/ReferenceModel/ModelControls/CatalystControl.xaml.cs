using System.Windows.Controls;
using CatalyticReforming.Views.Shared;

namespace CatalyticReforming.Views.Admin.ReferenceModel.ModelControls;

public partial class CatalystControl : IViewWithVM<CatalystControlVM>
{
    public CatalystControl()
    {
        InitializeComponent();
        ViewModel = App.GetService<CatalystControlVM>();
        DataContext = this;
    }

    public CatalystControlVM ViewModel { get; set; }
}
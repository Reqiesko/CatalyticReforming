using System.Windows.Controls;
using CatalyticReforming.Views.Shared;

namespace CatalyticReforming.Views.Admin.ReferenceModel.ModelControls;

public partial class InstallationControl : IViewWithVM<InstallationControlVM>
{
    public InstallationControl()
    {
        InitializeComponent();
        ViewModel = App.GetService<InstallationControlVM>();
        DataContext = this;
    }

    public InstallationControlVM ViewModel { get; set; }
}
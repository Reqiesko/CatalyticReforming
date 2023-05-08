using System.Windows.Controls;
using CatalyticReforming.Views.Shared;

namespace CatalyticReforming.Views.Admin.ReferenceModel.ModelControls;

public partial class EditInstallationControl : IViewWithVM<EditInstallationControlVM>
{
    public EditInstallationControl()
    {
        InitializeComponent();
        ViewModel = App.GetService<EditInstallationControlVM>();
        DataContext = this;
    }

    public EditInstallationControlVM ViewModel { get; set; }
}
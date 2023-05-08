using System.Windows.Controls;
using CatalyticReforming.Views.Shared;

namespace CatalyticReforming.Views.Admin.ReferenceModel.ModelControls;

public partial class MaterialControl : IViewWithVM<MaterialControlVM>
{
    public MaterialControl()
    {
        InitializeComponent();
        ViewModel = App.GetService<MaterialControlVM>();
        DataContext = this;
    }

    public MaterialControlVM ViewModel { get; set; }
}
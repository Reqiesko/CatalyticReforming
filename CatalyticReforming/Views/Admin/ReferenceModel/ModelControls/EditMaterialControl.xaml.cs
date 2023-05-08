using System.Windows.Controls;

using CatalyticReforming.Views.Shared;


namespace CatalyticReforming.Views.Admin.ReferenceModel.ModelControls;

public partial class EditMaterialControl : IViewWithVM<EditMaterialControlVM>
{
    public EditMaterialControl()
    {
        InitializeComponent();
        ViewModel = App.GetService<EditMaterialControlVM>();
        DataContext = this;
    }

    public EditMaterialControlVM ViewModel { get; set; }
}
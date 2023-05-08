using System.Windows.Controls;

using CatalyticReforming.Views.Shared;


namespace CatalyticReforming.Views.Admin.ReferenceModel.ModelControls;

public partial class EditCatalystControl : IViewWithVM<EditCatalystControlVM>
{
    public EditCatalystControl()
    {
        InitializeComponent();
        ViewModel = App.GetService<EditCatalystControlVM>();
        DataContext = this;
    }

    public EditCatalystControlVM ViewModel { get; set; }
}
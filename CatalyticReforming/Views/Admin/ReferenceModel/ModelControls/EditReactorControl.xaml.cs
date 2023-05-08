using System.Windows.Controls;
using CatalyticReforming.Views.Shared;

namespace CatalyticReforming.Views.Admin.ReferenceModel.ModelControls;

public partial class EditReactorControl : IViewWithVM<EditReactorControlVM>
{
    public EditReactorControl()
    {
        InitializeComponent();
        ViewModel = App.GetService<EditReactorControlVM>();
        DataContext = this;
    }

    public EditReactorControlVM ViewModel { get; set; }
}
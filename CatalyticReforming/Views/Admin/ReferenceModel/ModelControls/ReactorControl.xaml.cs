using System.Windows.Controls;
using CatalyticReforming.Views.Shared;

namespace CatalyticReforming.Views.Admin.ReferenceModel.ModelControls;

public partial class ReactorControl : IViewWithVM<ReactorControlVM>
{
    public ReactorControl()
    {
        InitializeComponent();
        ViewModel = App.GetService<ReactorControlVM>();
        DataContext = this;
    }

    public ReactorControlVM ViewModel { get; set; }
}
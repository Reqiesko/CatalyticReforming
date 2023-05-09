using System.Windows.Controls;

namespace CatalyticReforming.Views.Admin.ReferenceModel;

public partial class ReferenceModelControl : UserControl
{
    public ReferenceModelControl()
    {
        InitializeComponent();
    }

    private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
       ResearchControl.ViewModel.UpdateReactors();
    }
}
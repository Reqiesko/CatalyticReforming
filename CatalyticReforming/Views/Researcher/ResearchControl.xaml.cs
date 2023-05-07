using System.Windows;

using CatalyticReforming.Views.Shared;


namespace CatalyticReforming.Views.Researcher;

public partial class ResearchControl : IViewWithVM<ResearchControlVM>
{
    public ResearchControl()
    {
        InitializeComponent();
        ViewModel = App.GetService<ResearchControlVM>();
        DataContext = this;
    }

    public ResearchControlVM ViewModel { get; set; }

    private void TemperatureCheckBox_OnChecked(object sender, RoutedEventArgs e)
    {
        if (TemperatureCheckBox.IsChecked == true)
        {
            MaterialCheckBox.IsEnabled = false;
        }
        else
        {
            MaterialCheckBox.IsEnabled = true;
        }
    }

    private void MaterialCheckBox_OnChecked(object sender, RoutedEventArgs e)
    {
        if (MaterialCheckBox.IsChecked == true)
        {
            TemperatureCheckBox.IsEnabled = false;
        }
        else
        {
            TemperatureCheckBox.IsEnabled = true;
        }
    }
}


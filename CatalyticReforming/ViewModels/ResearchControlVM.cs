using System.Collections.ObjectModel;
using CatalyticReforming.Commands;
using CatalyticReforming.Services;

namespace CatalyticReforming.ViewModels;

public class ResearchControlVM : ViewModelBase
{
    private NavigationService _navigationService;

    public ObservableCollection<string> InsallersCollection { get; set; }
    public ObservableCollection<string> ReactorsCollection { get; set; }
    public ObservableCollection<double> ReactorPressure { get; set; }
    public ObservableCollection<string> CatalystCollection { get; set; }
    public ObservableCollection<double> CatalystDensity { get; set; }
    public ObservableCollection<double> StrengthFactor { get; set; }
    public ObservableCollection<string> MaterialCollection { get; set; }
    public ObservableCollection<double> NaphthenicHydrocarbons { get; set; }
    public ObservableCollection<double> AromaticHydrocarbons { get; set; }
    public ObservableCollection<double> MaterialsInput { get; set; }
    public ObservableCollection<double> Temperature { get; set; }

    private RelayCommand _startResearchCommand;
    public ResearchControlVM(NavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    public RelayCommand StartResearchCommand
    {
        get
        {
            return _startResearchCommand ??= new RelayCommand(o =>
            {
                
            });
        }
    }
}
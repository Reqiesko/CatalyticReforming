using System.Collections.ObjectModel;
using CatalyticReforming.Commands;
using CatalyticReforming.Services;

namespace CatalyticReforming.ViewModels;

public class ResearchControlVM : ViewModelBase
{
    private NavigationService _navigationService;

    public ObservableCollection<string> InsallersCollection { get; set; }

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
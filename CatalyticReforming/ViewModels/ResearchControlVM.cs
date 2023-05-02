using CatalyticReforming.Services;

namespace CatalyticReforming.ViewModels;

public class ResearchControlVM : ViewModelBase
{
    private NavigationService _navigationService;
    public ResearchControlVM(NavigationService navigationService)
    {
        _navigationService = navigationService;
    }
}
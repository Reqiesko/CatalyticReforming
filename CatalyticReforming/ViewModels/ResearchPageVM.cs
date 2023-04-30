using CatalyticReforming.Services;

namespace CatalyticReforming.ViewModels;

public class ResearchPageVM : ViewModelBase
{
    private NavigationService _navigationService;
    public ResearchPageVM(NavigationService navigationService)
    {
        _navigationService = navigationService;
    }
}
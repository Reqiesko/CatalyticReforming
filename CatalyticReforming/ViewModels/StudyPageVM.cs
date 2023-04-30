using CatalyticReforming.Services;
using DAL;

namespace CatalyticReforming.ViewModels;

public class StudyPageVM : ViewModelBase
{
    private NavigationService _navigationService;
    private readonly User _user;

    public StudyPageVM(NavigationService navigationService, User user)
    {
        _navigationService = navigationService;
        _user = user;
    }
}
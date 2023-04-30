using System;
using CatalyticReforming.Commands;
using CatalyticReforming.Services;
using DAL;


namespace CatalyticReforming.ViewModels;

public class StartPageVM : ViewModelBase
{
    private readonly NavigationService _navigationService;
    private readonly User _user;

    private RelayCommand _openStudyPageCommand;
    private RelayCommand _openResearchPageCommand;
    
    public StartPageVM(NavigationService navigationService, User user)
    {
        _navigationService = navigationService;
        _user = user;
    }
    
    public RelayCommand OpenStudyPageCommand
    {
        get
        {
            return _openStudyPageCommand ??= new RelayCommand(o =>
            {
                _navigationService.CurrentViewModel = new StudyPageVM(_navigationService, _user);
            });
        }
    }
    
    public RelayCommand OpenResearchPageCommand
    {
        get
        {
            return _openResearchPageCommand ??= new RelayCommand(o =>
            {
                if (Convert.ToBoolean(_user.Access))
                {
                    _navigationService.CurrentViewModel = new ResearchPageVM(_navigationService);
                }
            });
        }
    }
}
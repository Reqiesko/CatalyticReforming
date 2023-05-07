using CatalyticReforming.Utils.Commands;
using CatalyticReforming.Utils.Services;
using CatalyticReforming.ViewModels;
using CatalyticReforming.Views.Auth;


namespace CatalyticReforming.Views.Researcher;

public class StartControlVM : ViewModelBase
{
    private readonly NavigationService _navigationService;
    private readonly UserService _userService;
    private RelayCommand _openResearchPageCommand;

    private RelayCommand _openStudyPageCommand;

    public StartControlVM(NavigationService navigationService, UserService userService)
    {
        _navigationService = navigationService;
        _userService = userService;
    }

    public RelayCommand OpenStudyPageCommand
    {
        get
        {
            return _openStudyPageCommand ??= new RelayCommand(o =>
            {
                _navigationService.ChangeContent<StudyControl>();
            });
        }
    }

    public RelayCommand OpenResearchPageCommand
    {
        get
        {
            return _openResearchPageCommand ??= new RelayCommand(o =>
            {
                _navigationService.ChangeContent<ResearchControl>();
            }, _=> _userService.CurrentUser != null && _userService.CurrentUser.Access);
        }
    }

    private RelayCommand _changeUserCommand;

    public RelayCommand ChangeUserCommand
    {
        get
        {
            return _changeUserCommand ??= new RelayCommand(o =>
            {
                _navigationService.ChangeContent<LoginControl>();
            });
        }
    }

}

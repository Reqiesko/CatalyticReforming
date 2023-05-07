using System.Linq;

using CatalyticReforming.Utils.Commands;
using CatalyticReforming.Utils.Services;
using CatalyticReforming.ViewModels;
using CatalyticReforming.ViewModels.DAL_VM;

using DAL;


namespace CatalyticReforming.Views.Auth;

public class RegistrationControlVM : ViewModelBase
{
    private readonly GenericRepository _genericRepository;
    private readonly NavigationService _navigationService;

    public RegistrationControlVM(GenericRepository genericRepository, NavigationService navigationService)
    {
        _genericRepository = genericRepository;
        _navigationService = navigationService;
        var userRole = genericRepository.GetAll<UserRoleVM, UserRole>(u => u.Name == "User").Result.First();
        NewUser.Role = userRole;
    }
    public UserVM NewUser { get; set; } = new UserVM();

    private RelayCommand _registerCommand;

    public RelayCommand RegisterCommand
    {
        get
        {
            return _registerCommand ??= new RelayCommand(async _ =>
            {
                await _genericRepository.Create<UserVM, User>(NewUser);
                _navigationService.ChangeContent<LoginControl>();
            }, _ => !string.IsNullOrEmpty(NewUser.Password) && !string.IsNullOrEmpty(NewUser.Username));
        }
    }

    private RelayCommand _toLoginCommand;

    public RelayCommand ToLoginCommand
    {
        get
        {
            return _toLoginCommand ??= new RelayCommand(o =>
            {
                _navigationService.ChangeContent<LoginControl>();
            });
        }
    }

}

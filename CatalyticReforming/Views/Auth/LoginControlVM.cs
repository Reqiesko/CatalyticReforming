using System.ComponentModel.DataAnnotations;
using System.Linq;

using CatalyticReforming.Utils.Commands;
using CatalyticReforming.Utils.Services;
using CatalyticReforming.ViewModels;
using CatalyticReforming.ViewModels.DAL_VM;
using CatalyticReforming.Views.Admin;
using CatalyticReforming.Views.Researcher;

using DAL.Models.auth;

using Mapster;


namespace CatalyticReforming.Views.Auth;

public class LoginControlVM : ViewModelBase
{
    private readonly NavigationService _navigationService;
    private readonly GenericRepository _repository;
    private readonly UserService _userService;

    private RelayCommand _loginCommand;
    private RelayCommand _registerCommand;

    public LoginControlVM(NavigationService navigationService, UserService userService, GenericRepository repository)
    {
        _navigationService = navigationService;
        _userService = userService;
        _repository = repository;
        _userService.CurrentUser = null;
    }

    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    public string ErrorMessage { get; set; }

    public RelayCommand LoginCommand
    {
        get
        {
            return _loginCommand ??= new RelayCommand(o =>
            {
                var user = _repository.GetAll<UserVM, User>(u => u.Username == Username && u.Password == Password).Result.First();

                if (user == null)
                {
                    ErrorMessage = "Error";

                    return;
                }

                if (user.Role.Name == "User")
                {
                    // Пользователь найден, выполняем необходимые действия, например, переходим на главную страницу приложения.
                    _userService.CurrentUser = user.Adapt<UserVM>();
                    _navigationService.ChangeContent<StartControl>();
                }
                else if (user.Role.Name == "Admin")
                {
                    _userService.CurrentUser = user.Adapt<UserVM>();
                    _navigationService.ChangeContent<AdminControl>();
                }
                else
                {
                    // Пользователь не найден, выводим сообщение об ошибке.
                    ErrorMessage = "Error";
                }
            });
        }
    }

    public RelayCommand RegisterCommand
    {
        get
        {
            return _registerCommand ??= new RelayCommand(_ =>
            {
                _navigationService.ChangeContent<RegistrationControl>();
            });
        }
    }
}



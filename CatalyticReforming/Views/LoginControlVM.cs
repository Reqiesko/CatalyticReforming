using System.ComponentModel.DataAnnotations;
using System.Linq;

using CatalyticReforming.Utils.Commands;
using CatalyticReforming.Utils.Services;
using CatalyticReforming.ViewModels;

using DAL;


namespace CatalyticReforming.Views;

public class LoginControlVM : ViewModelBase
{
    private readonly AppDbContext _dbContext;

    private readonly NavigationService _navigationService;

    private RelayCommand _loginCommand;

    public LoginControlVM(NavigationService navigationService)
    {
        _navigationService = navigationService;
        _dbContext = new AppDbContext();
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
                var user = _dbContext.Users.FirstOrDefault(u => u.Username == Username && u.Password == Password);

                if (user == null)
                {
                    ErrorMessage = "Error";

                    return;
                }

                if (user.Role.Name == "User")
                {
                    // Пользователь найден, выполняем необходимые действия, например, переходим на главную страницу приложения.
                    _navigationService.ChangeContent<StartControl>();
                }
                else if (user.Role.Name == "Admin")
                {
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
}

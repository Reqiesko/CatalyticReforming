using System.ComponentModel.DataAnnotations;
using CatalyticReforming.Commands;
using CatalyticReforming.Services;
using System.Linq;

using CatalyticReforming.Views;

using DAL;


namespace CatalyticReforming.ViewModels
{
    public class LoginControlVM : ViewModelBase
    {
        private readonly AppDbContext _dbContext;

        private readonly NavigationService _navigationService;

        private RelayCommand _loginCommand;

        [Required] public string Username { get; set; }
        [Required] public string Password { get; set; }
        public string ErrorMessage { get; set; }

        public LoginControlVM(NavigationService navigationService)
        {
            _navigationService = navigationService;
            _dbContext = new AppDbContext();
        }

        public RelayCommand LoginCommand
        {
            get
            {
                return _loginCommand ??= new RelayCommand(o =>
                {
                    var user = _dbContext.Users.FirstOrDefault(u => u.Username == Username && u.Password == Password);
                    if (user.Role == UserRoles.User)
                    {
                        // Пользователь найден, выполняем необходимые действия, например, переходим на главную страницу приложения.
                        _navigationService.ChangeContent<StartControl>();
                    }
                    else if (user.Role == UserRoles.Admin)
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
}

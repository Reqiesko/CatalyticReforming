using System.ComponentModel.DataAnnotations;
using CatalyticReforming.Commands;
using CatalyticReforming.Services;
using System.Linq;
using DAL;


namespace CatalyticReforming.ViewModels
{
    public class LoginPageVM : ViewModelBase
    {
        private readonly AppDbContext _dbContext;

        private readonly NavigationService _navigationService;

        private RelayCommand _loginCommand;

        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string ErrorMessage { get; set; }

        public LoginPageVM(NavigationService navigationService)
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
                    if (user is { Role: "user" })
                    {
                        // Пользователь найден, выполняем необходимые действия, например, переходим на главную страницу приложения.
                        //_navigationService.CurrentViewModel = new ComputePageVM(_navigationService);
                    }
                    else if (user is { Role: "admin" })
                    {
                        //_navigationService.CurrentViewModel = new AdminPageVM(_navigationService);
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

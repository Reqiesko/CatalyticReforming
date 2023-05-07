﻿using System.ComponentModel.DataAnnotations;
using System.Linq;

using CatalyticReforming.Utils.Commands;
using CatalyticReforming.Utils.Services;
using CatalyticReforming.Utils.Services.DialogService;
using CatalyticReforming.ViewModels;
using CatalyticReforming.ViewModels.DAL_VM;
using DAL;
using Mapster;


namespace CatalyticReforming.Views;

public class LoginControlVM : ViewModelBase
{
    private readonly AppDbContext _dbContext;

    private readonly NavigationService _navigationService;
    private readonly UserService _userService;

    private RelayCommand _loginCommand;

    public LoginControlVM(NavigationService navigationService, UserService userService)
    {
        _navigationService = navigationService;
        _userService = userService;
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
}

using System;
using System.Linq;
using System.Windows;
using CatalyticReforming.Utils.Commands;
using CatalyticReforming.Utils.Services;
using CatalyticReforming.ViewModels;
using CatalyticReforming.ViewModels.DAL_VM;
using CatalyticReforming.ViewModels.DAL_VM.auth;

using DAL.Models.auth;


namespace CatalyticReforming.Views.Auth;

public class RegistrationControlVM : ViewModelBase
{
    private readonly GenericRepository _genericRepository;
    private readonly NavigationService _navigationService;
    private readonly MessageBoxService _messageService;
    private RelayCommand _registerCommand;

    private RelayCommand _toLoginCommand;

    public RegistrationControlVM(GenericRepository genericRepository, NavigationService navigationService)
    {
        _genericRepository = genericRepository;
        _navigationService = navigationService;
        _messageService = new MessageBoxService();
        var userRole = genericRepository.GetAll<UserRoleVM, UserRole>(u => u.Name == "User").Result.First();
        NewUser.Role = userRole;
    }

    public UserVM NewUser { get; set; } = new();

    public RelayCommand RegisterCommand
    {
        get
        {
            return _registerCommand ??= new RelayCommand(async _ =>
            {
                if (_genericRepository.GetAll<UserVM, User>(u => u.Username == NewUser.Username).Result.Exists(u => u.Username == NewUser.Username))
                {
                    _messageService.Show("Ошибка!\nТакой пользователь уже существует!", caption: "Ошибка");
                    return;
                }
                await _genericRepository.Create<UserVM, User>(NewUser);
                _navigationService.ChangeContent<LoginControl>();
            }, _ => !string.IsNullOrEmpty(NewUser.Password) && !string.IsNullOrEmpty(NewUser.Username));
        }
    }

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



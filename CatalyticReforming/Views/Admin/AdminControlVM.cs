using System.Collections.ObjectModel;
using System.Windows;

using CatalyticReforming.Utils.Commands;
using CatalyticReforming.Utils.Default_Dialogs;
using CatalyticReforming.Utils.Services;
using CatalyticReforming.Utils.Services.DialogService;
using CatalyticReforming.ViewModels;
using CatalyticReforming.ViewModels.DAL_VM;
using CatalyticReforming.ViewModels.DAL_VM.auth;
using CatalyticReforming.Views.Auth;

using DAL.Models.auth;
using DAL.Models.test;

using Mapster;


namespace CatalyticReforming.Views.Admin;

public class AdminControlVM : ViewModelBase
{
    private readonly DefaultDialogs _defaultDialogs;
    private readonly MyDialogService _dialogService;
    private readonly NavigationService _navigationService;
    private readonly GenericRepository _repository;

    private RelayCommand _addUser;
    private RelayCommand _changeUserCommand;
    private RelayCommand _deleteUser;

    private RelayCommand _editUser;

    public AdminControlVM(MyDialogService dialogService, GenericRepository repository, DefaultDialogs defaultDialogs,
                          NavigationService navigationService)
    {
        _dialogService = dialogService;
        _repository = repository;
        _defaultDialogs = defaultDialogs;
        _navigationService = navigationService;
        Users = new ObservableCollection<UserVM>(_repository.GetAll<UserVM, User>().Result);
    }

    public ObservableCollection<UserVM> Users { get; set; }

    public RelayCommand AddUser
    {
        get
        {
            return _addUser ??= new RelayCommand(async _ =>
            {
                var newUser = await _dialogService.ShowDialog<EditUserControl>(new UserVM()) as UserVM;

                if (newUser is null)
                {
                    return;
                }

                var entity = await _repository.Create<UserVM, User>(newUser);
                newUser.Id = entity.Id;
                Users.Add(newUser);
            });
        }
    }

    public RelayCommand EditUser
    {
        get
        {
            return _editUser ??= new RelayCommand(async userVM =>
            {
                var newUser = await _dialogService.ShowDialog<EditUserControl>(userVM.Adapt<UserVM>()) as UserVM;

                if (newUser is null)
                {
                    return;
                }

                await _repository.Update<UserVM, User>(newUser);
                newUser.Adapt((UserVM) userVM);
            });
        }
    }

    public RelayCommand DeleteUser
    {
        get
        {
            return _deleteUser ??= new RelayCommand(async userVM =>
            {
                var mbRes = await _defaultDialogs.AreYouSureToDelete(" выбранного пользователя");

                if (mbRes != MessageBoxResult.Yes)
                {
                    return;
                }

                await _repository.Delete<UserVM, User>((UserVM) userVM);
                Users.Remove((UserVM) userVM);
            });
        }
    }

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



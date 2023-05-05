﻿using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

using CatalyticReforming.Utils.Commands;
using CatalyticReforming.Utils.Default_Dialogs;
using CatalyticReforming.Utils.Services;
using CatalyticReforming.Utils.Services.DialogService;
using CatalyticReforming.ViewModels;
using CatalyticReforming.ViewModels.DAL_VM;

using DAL;

using Mapster;


namespace CatalyticReforming.Views;

public class AdminControlVM : ViewModelBase
{
    private readonly MyDialogService _dialogService;
    private readonly GenericRepository _repository;
    private readonly DefaultDialogs _defaultDialogs;

    public AdminControlVM(MyDialogService dialogService, GenericRepository repository, DefaultDialogs defaultDialogs)
    {
        _dialogService = dialogService;
        _repository = repository;
        _defaultDialogs = defaultDialogs;
        Users = new ObservableCollection<UserVM>(_repository.GetAll<UserVM, User>().Result);

    }

    public ObservableCollection<UserVM> Users { get; set; }

    private RelayCommand _addUser;

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

    private RelayCommand _editUser;

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
    private RelayCommand _deleteUser;

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
                await _repository.Delete<QuestionVM, Question>((QuestionVM) userVM);
                Users.Remove((UserVM) userVM);

            });
        }
    }

}

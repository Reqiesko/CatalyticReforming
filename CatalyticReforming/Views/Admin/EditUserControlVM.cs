using System;
using System.Collections.ObjectModel;
using System.Linq;

using CatalyticReforming.Utils.Commands;
using CatalyticReforming.Utils.Services;
using CatalyticReforming.Utils.Services.DialogService.Interfaces;
using CatalyticReforming.ViewModels;
using CatalyticReforming.ViewModels.DAL_VM;

using DAL;


namespace CatalyticReforming.Views.Admin;

public class UserEditControlVM : ViewModelBase, IDataHolder, IResultHolder, IInteractionAware
{
    private readonly GenericRepository _repository;
    private RelayCommand _applyCommand;

    private RelayCommand _cancelCommand;

    public UserEditControlVM(GenericRepository repository)
    {
        _repository = repository;
        UserRoles = new ObservableCollection<UserRoleVM>(_repository.GetAll<UserRoleVM, UserRole>().Result);
    }

    public UserVM EditingUser { get; set; }
    public ObservableCollection<UserRoleVM> UserRoles { get; set; }

    public RelayCommand ApplyCommand
    {
        get
        {
            return _applyCommand ??= new RelayCommand(o =>
            {
                Result = EditingUser;
                FinishInteraction();
            });
        }
    }

    public RelayCommand CancelCommand
    {
        get
        {
            return _cancelCommand ??= new RelayCommand(o =>
            {
                Result = null;
                FinishInteraction();
            });
        }
    }

    public object Data
    {
        get => EditingUser;
        set
        {
            EditingUser = (UserVM) value;
            EditingUser.Role = UserRoles.First(x => x.Id == EditingUser.Role.Id);
        }
    }

    public Action FinishInteraction { get; set; }

    public object Result { get; set; }
}


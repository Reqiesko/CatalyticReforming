using System;
using System.Collections.ObjectModel;
using System.Linq;

using CatalyticReforming.Utils.Commands;
using CatalyticReforming.Utils.Services;
using CatalyticReforming.Utils.Services.DialogService.Interfaces;
using CatalyticReforming.ViewModels;
using CatalyticReforming.ViewModels.DAL_VM;

using DAL;


namespace CatalyticReforming.Views;

public class UserEditControlVM: ViewModelBase, IDataHolder, IResultHolder, IInteractionAware
{
    private readonly GenericRepository _repository;

    public UserEditControlVM(GenericRepository repository)
    {
        _repository = repository;
        UserRoles = new ObservableCollection<UserRoleVM>(_repository.GetAll<UserRoleVM, UserRole>().Result);
    }

    public UserVM EditingUser { get; set; }
    public ObservableCollection<UserRoleVM> UserRoles { get; set; }
    private RelayCommand _applyCommand;

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

    private RelayCommand _cancelCommand;

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

    public object Result { get; set; }
    public Action FinishInteraction { get; set; }
}

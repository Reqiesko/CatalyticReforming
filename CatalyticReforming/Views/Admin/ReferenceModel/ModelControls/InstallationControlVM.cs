using System.Collections.ObjectModel;
using System.Windows;
using CatalyticReforming.Utils.Commands;
using CatalyticReforming.Utils.Default_Dialogs;
using CatalyticReforming.Utils.Services;
using CatalyticReforming.Utils.Services.DialogService;
using CatalyticReforming.ViewModels;
using CatalyticReforming.ViewModels.DAL_VM.domain;
using CatalyticReforming.ViewModels.DAL_VM.test;
using DAL.Models.domain;
using DAL.Models.test;
using Mapster;
using Wpf.Ui.Services;

namespace CatalyticReforming.Views.Admin.ReferenceModel.ModelControls;

public class InstallationControlVM : ViewModelBase
{
    private RelayCommand _addReactor;
    private RelayCommand _editInstallation;
    private RelayCommand _deleteInstallation;
    private readonly MyDialogService _dialogService;
    private readonly DefaultDialogs _defaultDialogs;
    private readonly GenericRepository _repository;
    
    public ObservableCollection<InstallationVM> Installations { get; set; }
    public InstallationControlVM(MyDialogService dialogService, DefaultDialogs defaultDialogs,
        GenericRepository repository)
    {
        _dialogService = dialogService;
        _defaultDialogs = defaultDialogs;
        _repository = repository;
        Installations = new ObservableCollection<InstallationVM>(_repository.GetAll<InstallationVM, Installation>().Result);
    }

    public RelayCommand AddInstallation
    {
        get
        {
            return _addReactor ??= new RelayCommand(async o =>
            {
                var newInstallation = await _dialogService.ShowDialog<EditInstallationControl>(new InstallationVM()) as InstallationVM;

                if (newInstallation is null)
                {
                    return;
                }

                var entity = await _repository.Create<InstallationVM, Installation>(newInstallation);
                entity.Adapt(newInstallation);
                Installations.Add(newInstallation);
            });
        }
    }
    public RelayCommand EditInstallation
    {
        get
        {
            return _editInstallation ??= new RelayCommand(async installation =>
            {
                var res = await _dialogService.ShowDialog<EditInstallationControl>(installation.Adapt<InstallationVM>()) as InstallationVM;

                if (res is null)
                {
                    return;
                }

                await _repository.Update<InstallationVM, Installation>(res);
                res.Adapt((InstallationVM) installation);
            });
        }
    }

    public RelayCommand DeleteInstallation
    {
        get
        {
            return _deleteInstallation ??= new RelayCommand(async installation =>
            {
                var mbRes = await _defaultDialogs.AreYouSureToDelete("выбранную установку");

                if (mbRes != MessageBoxResult.Yes)
                {
                    return;
                }

                await _repository.Delete<InstallationVM, Installation>((InstallationVM) installation);
                Installations.Remove((InstallationVM) installation);
            });
        }
    }
}
using System;
using System.Collections.ObjectModel;
using System.Linq;
using CatalyticReforming.Utils.Commands;
using CatalyticReforming.Utils.Services;
using CatalyticReforming.Utils.Services.DialogService;
using CatalyticReforming.Utils.Services.DialogService.Interfaces;
using CatalyticReforming.ViewModels;
using CatalyticReforming.ViewModels.DAL_VM.domain;
using DAL.Models.domain;
using Mapster;

namespace CatalyticReforming.Views.Admin.ReferenceModel.ModelControls;

public class EditInstallationControlVM : ViewModelBase, IDataHolder, IResultHolder, IInteractionAware
{
    private RelayCommand _applyCommand;
    private RelayCommand _cancelCommand;
    private RelayCommand _addReactor;
    private MyDialogService _dialogService;

    public ObservableCollection<ReactorVM> Reactors { get; set; }
    
    public EditInstallationControlVM(MyDialogService dialogService, GenericRepository genericRepository)
    {
        _dialogService = dialogService;
        Reactors = genericRepository.GetAll<ReactorVM, Reactor>().Result.Adapt<ObservableCollection<ReactorVM>>();
    }

    public InstallationVM EditingInstallation { get; set; }
    
    
    public RelayCommand AddReactor
    {
        get
        {
            return _addReactor ??= new RelayCommand(async _ =>
            {
                if (await _dialogService.ShowDialog<EditReactorControl>(new ReactorVM()) is not ReactorVM res)
                {
                    return;
                }

                EditingInstallation.Reactor = res;
                Reactors.Add(res);
            });
        }
    }
    
    public RelayCommand ApplyCommand
    {
        get
        {
            return _applyCommand ??= new RelayCommand(o =>
            {
                Result = EditingInstallation;
                FinishInteraction();
            }, _=>!EditingInstallation.HasErrors);
        }
    }

    public RelayCommand CancelCommand
    {
        get
        {
            return _cancelCommand ??= new RelayCommand(o =>
            {
                FinishInteraction();
            });
        }
    }
    
    public object Data
    {
        get => EditingInstallation;
        set
        {
            EditingInstallation = (InstallationVM) value;

            
            if (EditingInstallation.Reactor is not null)
            {
                EditingInstallation.Reactor = Reactors.First(x => x.Id == EditingInstallation.Reactor.Id);
            }
        }
    }

    public Action FinishInteraction { get; set; }
    public object Result { get; set; }
}
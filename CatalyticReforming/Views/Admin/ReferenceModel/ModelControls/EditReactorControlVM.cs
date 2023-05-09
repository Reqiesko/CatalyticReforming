using System;
using CatalyticReforming.Utils.Commands;
using CatalyticReforming.Utils.Services.DialogService.Interfaces;
using CatalyticReforming.ViewModels;
using CatalyticReforming.ViewModels.DAL_VM.domain;

namespace CatalyticReforming.Views.Admin.ReferenceModel.ModelControls;

public class EditReactorControlVM : ViewModelBase, IDataHolder, IResultHolder, IInteractionAware
{
    private RelayCommand _applyCommand;
    private RelayCommand _cancelCommand;
    
    public ReactorVM EditingReactor { get; set; }
    public RelayCommand ApplyCommand
    {
        get
        {
            return _applyCommand ??= new RelayCommand(o =>
            {
                Result = EditingReactor;
                FinishInteraction();
            }, _=> !EditingReactor.HasErrors);
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
        get => EditingReactor;
        set => EditingReactor = (ReactorVM) value;
    }

    public Action FinishInteraction { get; set; }
    public object Result { get; set; }
}
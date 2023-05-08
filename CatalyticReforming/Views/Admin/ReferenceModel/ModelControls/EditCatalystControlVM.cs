using System;

using CatalyticReforming.Utils.Commands;
using CatalyticReforming.Utils.Services.DialogService.Interfaces;
using CatalyticReforming.ViewModels;
using CatalyticReforming.ViewModels.DAL_VM.domain;


namespace CatalyticReforming.Views.Admin.ReferenceModel.ModelControls;

public class EditCatalystControlVM : ViewModelBase, IDataHolder, IResultHolder, IInteractionAware
{
    public CatalystVM EditingCatalyst { get; set; }


    private RelayCommand _applyCommand;

    public RelayCommand ApplyCommand
    {
        get
        {
            return _applyCommand ??= new RelayCommand(o =>
            {
                Result = EditingCatalyst;
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
        get => EditingCatalyst;
        set => EditingCatalyst = (CatalystVM) value;
    }

    public object Result { get; set; }
    public Action FinishInteraction { get; set; }
}

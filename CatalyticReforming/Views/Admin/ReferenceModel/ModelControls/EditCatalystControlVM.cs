using System;
using System.Collections;
using System.ComponentModel;

using CatalyticReforming.Utils.Commands;
using CatalyticReforming.Utils.Services.DialogService.Interfaces;
using CatalyticReforming.ViewModels;
using CatalyticReforming.ViewModels.DAL_VM.domain;


namespace CatalyticReforming.Views.Admin.ReferenceModel.ModelControls;

public class EditCatalystControlVM : ViewModelBase, IDataHolder, IResultHolder, IInteractionAware
{
    private readonly CatalystValidator _catalystValidator;

    public EditCatalystControlVM(CatalystValidator catalystValidator)
    {
        _catalystValidator = catalystValidator;
    }
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
            }, _=> !EditingCatalyst.HasErrors);
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

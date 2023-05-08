using System;

using CatalyticReforming.Utils.Commands;
using CatalyticReforming.Utils.Services.DialogService.Interfaces;
using CatalyticReforming.ViewModels;
using CatalyticReforming.ViewModels.DAL_VM.domain;


namespace CatalyticReforming.Views.Admin.ReferenceModel.ModelControls;

public class EditMaterialControlVM: ViewModelBase, IDataHolder, IResultHolder, IInteractionAware
{
    public MaterialVM EditingMaterial { get; set; }
    public object Data
    {
        get => EditingMaterial;
        set => EditingMaterial = (MaterialVM) value;
    }

    public object Result { get; set; }
    public Action FinishInteraction { get; set; }
    
    private RelayCommand _applyCommand;
    private RelayCommand _cancelCommand;

    public RelayCommand ApplyCommand
    {
        get
        {
            return _applyCommand ??= new RelayCommand(o =>
            {
                Result = EditingMaterial;
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
}

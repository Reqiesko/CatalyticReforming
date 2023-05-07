using System;

using CatalyticReforming.Utils.Commands;
using CatalyticReforming.Utils.Services.DialogService.Interfaces;
using CatalyticReforming.ViewModels;
using CatalyticReforming.ViewModels.DAL_VM;


namespace CatalyticReforming.Views.Admin.Testing;

public class EditAnswerControlVM : ViewModelBase, IDataHolder, IResultHolder, IInteractionAware
{
    private RelayCommand _applyCommand;

    private RelayCommand _cancelCommand;

    public AnswerVM EditingAnswer { get; set; }

    public RelayCommand ApplyCommand
    {
        get
        {
            return _applyCommand ??= new RelayCommand(o =>
            {
                Result = EditingAnswer;
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
        get => EditingAnswer;
        set => EditingAnswer = (AnswerVM) value;
    }

    public Action FinishInteraction { get; set; }

    public object Result { get; set; }
}


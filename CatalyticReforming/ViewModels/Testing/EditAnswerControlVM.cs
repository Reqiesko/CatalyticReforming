using System;

using CatalyticReforming.Commands;
using CatalyticReforming.Services.DialogService;
using CatalyticReforming.ViewModels.DAL_VM;

using DAL;


namespace CatalyticReforming.ViewModels.Testing;

public class EditAnswerControlVM : ViewModelBase, IDataHolder, IResultHolder, IInteractionAware
{
    public EditAnswerControlVM()
    {
        
    }

    public AnswerVM EditingAnswer { get; set; }
    private RelayCommand _applyCommand;

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
        get => EditingAnswer;
        set => EditingAnswer = (AnswerVM) value;
    }

    public object Result { get; set; }
    public Action FinishInteraction { get; set; }
}

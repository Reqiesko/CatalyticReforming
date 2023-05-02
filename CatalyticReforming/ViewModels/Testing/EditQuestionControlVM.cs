using System;

using CatalyticReforming.Commands;
using CatalyticReforming.Services.DialogService;

using DAL;


namespace CatalyticReforming.ViewModels.Testing;

public class EditQuestionControlVM : ViewModelBase, IResultHolder, IDataHolder, IInteractionAware
{
    private readonly Func<AppDbContext> _contextCreator;

    public EditQuestionControlVM(Func<AppDbContext> contextCreator)
    {
        _contextCreator = contextCreator;
    }

    public Question EditingQuestion { get; set; }
    public object Result { get; }
    public object Data
    {
        get => EditingQuestion;
        set => EditingQuestion = (Question) value;
    }
    public Action FinishInteraction { get; set; }

    private RelayCommand _addAnswer;

    public RelayCommand AddAnswer
    {
        get
        {
            return _addAnswer ??= new RelayCommand(o =>
            {

            });
        }
    }

    private RelayCommand _editAnswer;

    public RelayCommand EditAnswer
    {
        get
        {
            return _editAnswer ??= new RelayCommand(o =>
            {

            });
        }
    }

    private RelayCommand _deleteAnswer;

    public RelayCommand DeleteAnswer
    {
        get
        {
            return _deleteAnswer ??= new RelayCommand(o =>
            {

            });
        }
    }

    private RelayCommand _applyCommand;

    public RelayCommand ApplyCommand
    {
        get
        {
            return _applyCommand ??= new RelayCommand(o =>
            {

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

            });
        }
    }

}

using System;
using System.Windows;

using CatalyticReforming.Commands;
using CatalyticReforming.Services;
using CatalyticReforming.Services.DialogService;
using CatalyticReforming.Utils.Default_Dialogs;
using CatalyticReforming.ViewModels.DAL_VM;
using CatalyticReforming.Views.Testing;

using DAL;

using Mapster;


namespace CatalyticReforming.ViewModels.Testing;

public class EditQuestionControlVM : ViewModelBase, IResultHolder, IDataHolder, IInteractionAware
{
    private readonly Func<AppDbContext> _contextCreator;
    private readonly MyDialogService _dialogService;
    private readonly GenericRepository _answerRepository;
    private readonly DefaultDialogs _defaultDialogs;

    public EditQuestionControlVM(Func<AppDbContext> contextCreator, MyDialogService dialogService, GenericRepository answerRepository, DefaultDialogs defaultDialogs)
    {
        _contextCreator = contextCreator;
        _dialogService = dialogService;
        _answerRepository = answerRepository;
        _defaultDialogs = defaultDialogs;
    }

    public QuestionVM EditingQuestion { get; set; }
    public object Result { get; set; }

    public object Data
    {
        get => EditingQuestion;
        set => EditingQuestion = (QuestionVM) value;
    }
    public Action FinishInteraction { get; set; }

    private RelayCommand _addAnswer;

    public RelayCommand AddAnswer
    {
        get
        {
            return _addAnswer ??= new RelayCommand(async _ =>
            {
                var res = await _dialogService.ShowDialog<EditAnswerControl>(new AnswerVM()) as AnswerVM;

                if (res is null)
                {
                    return;
                }
                EditingQuestion.Answers.Add(res);
            });
        }
    }

    private RelayCommand _editAnswer;

    public RelayCommand EditAnswer
    {
        get
        {
            return _editAnswer ??= new RelayCommand(async answer =>
            {
                var res = await _dialogService.ShowDialog<EditAnswerControl>(answer.Adapt<AnswerVM>()) as AnswerVM;

                if (res is null)
                {
                    return;
                }
                res.Adapt((AnswerVM)answer);
            });
        }
    }

    private RelayCommand _deleteAnswer;

    public RelayCommand DeleteAnswer
    {
        get
        {
            return _deleteAnswer ??= new RelayCommand(async answer=>
            {
                var res = await _defaultDialogs.AreYouSureToDelete("ответ");

                if (res != MessageBoxResult.Yes)
                {
                    return;
                }

                EditingQuestion.Answers.Remove((AnswerVM) answer);
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
                Result = EditingQuestion;
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
                FinishInteraction();
            });
        }
    }

}

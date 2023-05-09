using System;
using System.Windows;

using CatalyticReforming.Utils.Commands;
using CatalyticReforming.Utils.Default_Dialogs;
using CatalyticReforming.Utils.Services;
using CatalyticReforming.Utils.Services.DialogService;
using CatalyticReforming.Utils.Services.DialogService.Interfaces;
using CatalyticReforming.ViewModels;
using CatalyticReforming.ViewModels.DAL_VM;
using CatalyticReforming.ViewModels.DAL_VM.test;

using Mapster;


namespace CatalyticReforming.Views.Admin.Testing;

public class EditQuestionControlVM : ViewModelBase, IResultHolder, IDataHolder, IInteractionAware
{
    private readonly DefaultDialogs _defaultDialogs;
    private readonly MyDialogService _dialogService;
    private readonly GenericRepository _repository;

    private RelayCommand _addAnswer;

    private RelayCommand _applyCommand;

    private RelayCommand _cancelCommand;

    private RelayCommand _deleteAnswer;

    private RelayCommand _editAnswer;

    public EditQuestionControlVM(MyDialogService dialogService, GenericRepository repository,
                                 DefaultDialogs defaultDialogs)
    {
        _dialogService = dialogService;
        _repository = repository;
        _defaultDialogs = defaultDialogs;
    }

    public QuestionVM EditingQuestion { get; set; }

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

                res.Adapt((AnswerVM) answer);
            });
        }
    }

    public RelayCommand DeleteAnswer
    {
        get
        {
            return _deleteAnswer ??= new RelayCommand(async answer =>
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

    public RelayCommand ApplyCommand
    {
        get
        {
            return _applyCommand ??= new RelayCommand(o =>
            {
                Result = EditingQuestion;
                FinishInteraction();
            }, _=>!EditingQuestion.HasErrors);
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
        get => EditingQuestion;
        set => EditingQuestion = (QuestionVM) value;
    }

    public Action FinishInteraction { get; set; }
    public object Result { get; set; }
}




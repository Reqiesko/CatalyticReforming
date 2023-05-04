using System;
using System.Collections.ObjectModel;
using System.Windows;

using CatalyticReforming.Utils.Commands;
using CatalyticReforming.Utils.Default_Dialogs;
using CatalyticReforming.Utils.Services;
using CatalyticReforming.Utils.Services.DialogService;
using CatalyticReforming.ViewModels;
using CatalyticReforming.ViewModels.DAL_VM;

using DAL;

using Mapster;


namespace CatalyticReforming.Views.Testing;

public class TestBrowserControlVM : ViewModelBase
{
    private readonly Func<AppDbContext> _contextCreator;
    private readonly DefaultDialogs _defaultDialogs;
    private readonly MyDialogService _dialogService;
    private readonly GenericRepository _repository;
    private RelayCommand _addQuestion;

    private RelayCommand _deleteQuestion;

    private RelayCommand _editQuestion;

    public TestBrowserControlVM(Func<AppDbContext> contextCreator, MyDialogService dialogService, DefaultDialogs defaultDialogs,
                                GenericRepository repository)
    {
        _contextCreator = contextCreator;
        _dialogService = dialogService;
        _defaultDialogs = defaultDialogs;
        _repository = repository;

        using var context = _contextCreator();
        Questions = context.Questions.Adapt<ObservableCollection<QuestionVM>>();
    }

    public ObservableCollection<QuestionVM> Questions { get; set; }

    public RelayCommand AddQuestion
    {
        get
        {
            return _addQuestion ??= new RelayCommand(async o =>
            {
                var newQuestion = await _dialogService.ShowDialog<EditQuestionControl>(new QuestionVM()) as QuestionVM;

                if (newQuestion is null)
                {
                    return;
                }

                var entity = await _repository.Create<QuestionVM, Question>(newQuestion);
                newQuestion.Id = entity.Id;
                Questions.Add(newQuestion);
            });
        }
    }

    public RelayCommand EditQuestion
    {
        get
        {
            return _editQuestion ??= new RelayCommand(async question =>
            {
                var res = await _dialogService.ShowDialog<EditQuestionControl>(question.Adapt<QuestionVM>()) as QuestionVM;

                if (res is null)
                {
                    return;
                }

                await _repository.Update<QuestionVM, Question>(res);
                res.Adapt((QuestionVM) question);
            });
        }
    }

    public RelayCommand DeleteQuestion
    {
        get
        {
            return _deleteQuestion ??= new RelayCommand(async question =>
            {
                var mbRes = await _defaultDialogs.AreYouSureToDelete("выбранный вопрос");

                if (mbRes != MessageBoxResult.Yes)
                {
                    return;
                }

                await _repository.Delete<QuestionVM, Question>((QuestionVM) question);
                Questions.Remove((QuestionVM) question);
            });
        }
    }
}


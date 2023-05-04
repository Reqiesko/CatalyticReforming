using System;
using System.Collections.ObjectModel;
using System.Windows;

using CatalyticReforming.Commands;
using CatalyticReforming.Services;
using CatalyticReforming.Services.DialogService;
using CatalyticReforming.Utils.Default_Dialogs;
using CatalyticReforming.ViewModels.DAL_VM;
using CatalyticReforming.Views.Testing;

using DAL;

using Mapster;

using Wpf.Ui.Contracts;


namespace CatalyticReforming.ViewModels.Testing;

public class TestBrowserControlVM : ViewModelBase
{
    private readonly Func<AppDbContext> _contextCreator;
    private readonly MyDialogService _dialogService;
    private readonly DefaultDialogs _defaultDialogs;
    private readonly GenericRepository _repository;

    public TestBrowserControlVM(Func<AppDbContext> contextCreator, MyDialogService dialogService, DefaultDialogs defaultDialogs, GenericRepository repository)
    {
        _contextCreator = contextCreator;
        _dialogService = dialogService;
        _defaultDialogs = defaultDialogs;
        _repository = repository;

        using var context = _contextCreator();
        Questions = context.Questions.Adapt<ObservableCollection<QuestionVM>>();
    }

    public ObservableCollection<QuestionVM> Questions { get; set; }
    private RelayCommand _addQuestion;

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
                var entity = await _repository.Create<QuestionVM,Question>(newQuestion);
                newQuestion.Id = entity.Id;
                Questions.Add(newQuestion);
            });
        }
    }

    private RelayCommand _editQuestion;

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

                await _repository.Update<QuestionVM,Question>(res);
                res.Adapt((QuestionVM) question);
            });
        }
    }

    private RelayCommand _deleteQuestion;

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

                await _repository.Delete<QuestionVM,Question>((QuestionVM)question);
                Questions.Remove((QuestionVM) question);
            });
        }
    }


}

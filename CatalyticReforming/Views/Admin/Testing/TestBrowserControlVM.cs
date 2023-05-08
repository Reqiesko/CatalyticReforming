using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

using CatalyticReforming.Utils.Commands;
using CatalyticReforming.Utils.Default_Dialogs;
using CatalyticReforming.Utils.Services;
using CatalyticReforming.Utils.Services.DialogService;
using CatalyticReforming.ViewModels;
using CatalyticReforming.ViewModels.DAL_VM;
using CatalyticReforming.ViewModels.DAL_VM.auth;
using CatalyticReforming.ViewModels.DAL_VM.test;

using DAL.Models.test;

using Mapster;

using PropertyChanged;


namespace CatalyticReforming.Views.Admin.Testing;

public class TestBrowserControlVM : ViewModelBase
{
    private readonly DefaultDialogs _defaultDialogs;
    private readonly MyDialogService _dialogService;
    private readonly GenericRepository _repository;
    private RelayCommand _addQuestion;

    private RelayCommand _deleteQuestion;

    private RelayCommand _editQuestion;

    public TestBrowserControlVM(MyDialogService dialogService, DefaultDialogs defaultDialogs,
                                GenericRepository repository)
    {
        _dialogService = dialogService;
        _defaultDialogs = defaultDialogs;
        _repository = repository;

        Questions = _repository.GetAll<QuestionVM, Question>().Result.Adapt<ObservableCollection<QuestionVM>>();
        TestConfig = _repository.GetAll<TestConfigVM, TestConfig>().Result.First();
        TestConfig.Adapt(TestConfigTemp);
    }


#region test config

    public TestConfigVM TestConfig { get; set; }
    public TestConfigVM TestConfigTemp { get; set; } = new();
    
    public bool IsChanged =>
        TestConfigTemp != null && TestConfig != null && (TestConfig.NumberOfQuestions != TestConfigTemp.NumberOfQuestions ||
                                                         TestConfig.NumberOfQuestionsToPass != TestConfigTemp.NumberOfQuestionsToPass);

    private RelayCommand _applyTestConfigChanges;

    public RelayCommand ApplyTestConfigChanges
    {
        get
        {
            return _applyTestConfigChanges ??= new RelayCommand(async _ =>
            {
                TestConfigTemp.Adapt(TestConfig);
                await _repository.Update<TestConfigVM, TestConfig>(TestConfig);
            }, _=> IsChanged);
        }
    }

    private RelayCommand _cancelTestConfigChanges;

    public RelayCommand CancelTestConfigChanges
    {
        get
        {
            return _cancelTestConfigChanges ??= new RelayCommand(_ =>
            {
                TestConfig.Adapt(TestConfigTemp);
            }, _ => IsChanged);
        }
    }

#endregion

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




using System;
using System.Collections.ObjectModel;
using System.Linq;

using CatalyticReforming.Utils.Commands;
using CatalyticReforming.Utils.Services;
using CatalyticReforming.ViewModels;
using CatalyticReforming.ViewModels.DAL_VM;
using CatalyticReforming.ViewModels.DAL_VM.auth;
using CatalyticReforming.ViewModels.DAL_VM.test;
using CatalyticReforming.Views.Auth;

using DAL;
using DAL.Models.auth;
using DAL.Models.test;

using Mapster;

using Microsoft.EntityFrameworkCore;


namespace CatalyticReforming.Views.Researcher;

public class StudyControlVM : ViewModelBase
{
    private readonly NavigationService _navigationService;
    private readonly GenericRepository _repository;
    private readonly UserService _userService;
    private readonly MessageBoxService _messageBoxService;
    private RelayCommand _changeUserCommand;
    private RelayCommand _completeTestCommand;

    private RelayCommand _navigateBackCommand;

    public StudyControlVM(NavigationService navigationService, UserService userService,
                          GenericRepository repository, Func<AppDbContext> contextCreator)
    {
        _navigationService = navigationService;
        _userService = userService;
        _repository = repository;
        _messageBoxService = new MessageBoxService();
        TestConfig = _repository.GetAll<TestConfigVM, TestConfig>().Result.First();

        using var context = contextCreator();
        Questions = context.Questions
                           .OrderBy(r => EF.Functions.Random())
                           .Take(TestConfig.NumberOfQuestions)
                           .Adapt<ObservableCollection<QuestionVM>>();
    }

    public TestConfigVM TestConfig { get; set; }
    public ObservableCollection<QuestionVM> Questions { get; set; }

    public string Text { get; set; }

    public RelayCommand CompleteTestCommand
    {
        get
        {
            return _completeTestCommand ??= new RelayCommand(async _ =>
            {
                var score = Questions.Count(question => question.Answers.All(a => a.IsCorrect == a.IsSelected));

                if (score < TestConfig.NumberOfQuestionsToPass)
                {
                    _messageBoxService.Show($"Вы не прошли тест\nДля удачного прохождения требуется набрать {TestConfig.NumberOfQuestionsToPass} баллов.\nВаше количество баллов: {score}", "Внимание!");
                    return;
                }
                _messageBoxService.Show($"Вы успешно прошли тест\nВаше количество баллов: {score}\nЧтобы перейти к исследованию\n вернитесь на предыдущую страницу!", "Внимание!");
                _userService.CurrentUser.Access = true;
                await _repository.Update<UserVM, User>(_userService.CurrentUser);
            });
        }
    }

    public RelayCommand ChangeUserCommand
    {
        get
        {
            return _changeUserCommand ??= new RelayCommand(o =>
            {
                _navigationService.ChangeContent<LoginControl>();
            });
        }
    }

    public RelayCommand NavigateBackCommand
    {
        get
        {
            return _navigateBackCommand ??= new RelayCommand(o =>
            {
                _navigationService.ChangeContent<StartControl>();
            });
        }
    }
}



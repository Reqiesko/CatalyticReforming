using System.Collections.ObjectModel;
using System.Linq;

using CatalyticReforming.Utils.Commands;
using CatalyticReforming.Utils.Services;
using CatalyticReforming.ViewModels;
using CatalyticReforming.ViewModels.DAL_VM;
using CatalyticReforming.Views.Auth;

using DAL.Models.auth;
using DAL.Models.test;

using Mapster;


namespace CatalyticReforming.Views.Researcher;

public class StudyControlVM : ViewModelBase
{
    private readonly NavigationService _navigationService;
    private readonly GenericRepository _repository;
    private readonly UserService _userService;
    private RelayCommand _changeUserCommand;
    private RelayCommand _completeTestCommand;

    private RelayCommand _navigateBackCommand;

    public StudyControlVM(NavigationService navigationService, UserService userService,
                          GenericRepository repository)
    {
        _navigationService = navigationService;
        _userService = userService;
        _repository = repository;
        Questions = _repository.GetAll<QuestionVM, Question>().Result.Adapt<ObservableCollection<QuestionVM>>();
    }

    public ObservableCollection<QuestionVM> Questions { get; set; }

    public string Text { get; set; }

    public RelayCommand CompleteTestCommand
    {
        get
        {
            return _completeTestCommand ??= new RelayCommand(async _ =>
            {
                var score = Questions.Count(question => question.Answers.All(a => a.IsCorrect == a.IsSelected));

                if (score < 2)
                {
                    return;
                }

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



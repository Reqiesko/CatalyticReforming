using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CatalyticReforming.Utils.Commands;
using CatalyticReforming.Utils.Services;
using CatalyticReforming.Utils.Services.DialogService;
using CatalyticReforming.ViewModels;
using CatalyticReforming.ViewModels.DAL_VM;
using DAL;
using Mapster;


namespace CatalyticReforming.Views;

public class StudyControlVM : ViewModelBase
{
    private readonly UserService _userService;
    private readonly GenericRepository _repository;
    private NavigationService _navigationService;
    private RelayCommand _completeTestCommand;
    
    public ObservableCollection<QuestionVM> Questions { get; set; }
    
    public string Text { get; set; }
    public StudyControlVM(NavigationService navigationService, UserService userService, Func<AppDbContext> contextCreator, GenericRepository repository)
    {
        _navigationService = navigationService;
        _userService = userService;
        _repository = repository;
        using var context = contextCreator();
        Questions = context.Questions.Adapt<ObservableCollection<QuestionVM>>();
    }

    public RelayCommand CompleteTestCommand
    {
        get
        {
            return _completeTestCommand ??= new RelayCommand(async _ =>
            {
                var score = Questions.Count(question => question.Answers.All(a => a.IsCorrect == a.IsSelected));

                if (score < 2) return;
                _userService.CurrentUser.Access = true;
                await _repository.Update<UserVM, User>(_userService.CurrentUser);
            });
        }
    }
}

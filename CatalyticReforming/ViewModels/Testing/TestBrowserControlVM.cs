using System;
using System.Collections.ObjectModel;
using System.Windows;

using CatalyticReforming.Commands;
using CatalyticReforming.Services;
using CatalyticReforming.Services.DialogService;
using CatalyticReforming.Views.Testing;

using DAL;

using Mapster;

using Wpf.Ui.Contracts;


namespace CatalyticReforming.ViewModels.Testing;

public class TestBrowserControlVM : ViewModelBase
{
    private readonly Func<AppDbContext> _contextCreator;
    private readonly MyDialogService _dialogService;
    private readonly MessageBoxService _messageBoxService;

    public TestBrowserControlVM(Func<AppDbContext> contextCreator, MyDialogService dialogService, MessageBoxService messageBoxService)
    {
        _contextCreator = contextCreator;
        _dialogService = dialogService;
        _messageBoxService = messageBoxService;

        using var context = _contextCreator();
        Questions = new ObservableCollection<Question>(context.Questions);
    }

    public ObservableCollection<Question> Questions { get; set; }
    private RelayCommand _addQuestion;

    public RelayCommand AddQuestion
    {
        get
        {
            return _addQuestion ??= new RelayCommand(async o =>
            {
                var newQuestion = await _dialogService.ShowDialog<EditQuestionControl>(new Question());

                // await using var context = _contextCreator();
                //
                // context.Questions.Add((Question) newQuestion);
                // context.SaveChanges();
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
                var res = await _dialogService.ShowDialog<EditQuestionControl>(question.Adapt<Question>()) as Question;

                if (res is  null)
                {
                    return;
                }

                await using var context = _contextCreator();

                res.BuildAdapter()
                   .EntityFromContext(context)
                   .AdaptTo(question);
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
                var mbRes = await _messageBoxService.Show("Вы действительно хотите удалить выбранный вопрос?",
                                                          "Предупреждение",
                                                          MessageBoxButton.YesNo);

                if (mbRes != MessageBoxResult.Yes)
                {
                    return;
                }

                await using var context = _contextCreator();
                context.Questions.Remove((Question) question);
                await context.SaveChangesAsync();
                Questions.Remove((Question) question);

            });
        }
    }


}

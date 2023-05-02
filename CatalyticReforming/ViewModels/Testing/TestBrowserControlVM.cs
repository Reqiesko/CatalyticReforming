using System;
using System.Collections.ObjectModel;

using CatalyticReforming.Commands;
using CatalyticReforming.Services.DialogService;
using CatalyticReforming.Views.Testing;

using DAL;


namespace CatalyticReforming.ViewModels.Testing;

public class TestBrowserControlVM : ViewModelBase
{
    private readonly Func<AppDbContext> _contextCreator;
    private readonly MyDialogService _dialogService;

    public TestBrowserControlVM(Func<AppDbContext> contextCreator, MyDialogService dialogService)
    {
        _contextCreator = contextCreator;
        _dialogService = dialogService;

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
            return _editQuestion ??= new RelayCommand(o =>
            {

            });
        }
    }

    private RelayCommand _deleteQuestion;

    public RelayCommand DeleteQuestion
    {
        get
        {
            return _deleteQuestion ??= new RelayCommand(o =>
            {

            });
        }
    }


}

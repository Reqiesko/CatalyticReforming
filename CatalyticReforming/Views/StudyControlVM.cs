using System;
using System.Collections.Generic;

using CatalyticReforming.Utils.Services;
using CatalyticReforming.ViewModels;

using DAL;


namespace CatalyticReforming.Views;

public class StudyControlVM : ViewModelBase
{
    private readonly User _user;
    private NavigationService _navigationService;

    public StudyControlVM(NavigationService navigationService, User user)
    {
        _navigationService = navigationService;
        _user = user;

        Questions = new List<Question>
        {
            new()
            {
                Id = 0,
                Text = "Who is president of Russia?",
                Answers = new List<Answer>
                {
                    new()
                    {
                        Id = 0,
                        Text = "Putin",
                        IsCorrect = false,
                    },
                    new()
                    {
                        Id = 1,
                        Text = "Putin1",
                        IsCorrect = false,
                    },
                    new()
                    {
                        Id = 2,
                        Text = "Putin2",
                        IsCorrect = false,
                    },
                    new()
                    {
                        Id = 3,
                        Text = "Putin3",
                        IsCorrect = false,
                    },
                },
            },
        };

        Console.WriteLine("WWWW");
    }

    public List<Question> Questions { get; set; }
}

using System;
using System.Collections.Generic;
using CatalyticReforming.Services;
using DAL;

namespace CatalyticReforming.ViewModels;

public class StudyPageVM : ViewModelBase
{
    private NavigationService _navigationService;
    private readonly User _user;
    
    public List<Question> Questions { get; set; }
    
    public StudyPageVM(NavigationService navigationService, User user)
    {
        _navigationService = navigationService;
        _user = user;
        Questions = new List<Question>()
        {
            new Question()
            {
                Id = 0,
                Text = "Who is president of Russia?",
                Answers = new List<Answer>()
                {
                    new Answer()
                    {
                        Id = 0,
                        Text = "Putin",
                        IsSelected = false
                    },
                    new Answer()
                    {
                        Id = 1,
                        Text = "Putin1",
                        IsSelected = false
                    },
                    new Answer()
                    {
                        Id = 2,
                        Text = "Putin2",
                        IsSelected = false
                    },
                    new Answer()
                    {
                        Id = 3,
                        Text = "Putin3",
                        IsSelected = false
                    }
                
                }
            }
        };
        Console.WriteLine("WWWW");
    }
}
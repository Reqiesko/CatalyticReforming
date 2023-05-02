using System;
using CatalyticReforming.Commands;
using CatalyticReforming.Services;
using CatalyticReforming.Views;

using DAL;


namespace CatalyticReforming.ViewModels;

public class StartControlVM : ViewModelBase
{
    private readonly NavigationService _navigationService;

    private RelayCommand _openStudyPageCommand;
    private RelayCommand _openResearchPageCommand;
    
    public StartControlVM(NavigationService navigationService)
    {
        _navigationService = navigationService;
    }
    
    public RelayCommand OpenStudyPageCommand
    {
        get
        {
            return _openStudyPageCommand ??= new RelayCommand(o =>
            {
                _navigationService.ChangeContent<StudyControl>();
            });
        }
    }
    
    public RelayCommand OpenResearchPageCommand
    {
        get
        {
            return _openResearchPageCommand ??= new RelayCommand(o =>
            {
                _navigationService.ChangeContent<ResearchControl>();
            });
        }
    }
}
using CatalyticReforming.Commands;
using CatalyticReforming.Services;
using CatalyticReforming.Views;


namespace CatalyticReforming.ViewModels;

public class StartControlVM : ViewModelBase
{
    private readonly NavigationService _navigationService;
    private RelayCommand _openResearchPageCommand;

    private RelayCommand _openStudyPageCommand;

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

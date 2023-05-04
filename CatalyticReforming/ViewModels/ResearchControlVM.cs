using System.Collections.ObjectModel;
using System.Diagnostics;

using CatalyticReforming.Commands;
using CatalyticReforming.Services;

using Microsoft.Win32;


namespace CatalyticReforming.ViewModels;

public class ResearchControlVM : ViewModelBase
{
    private NavigationService _navigationService;

    private RelayCommand _startResearchCommand;

    public ResearchControlVM(NavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    public ObservableCollection<string> InsallersCollection { get; set; }
    public ObservableCollection<string> ReactorsCollection { get; set; }
    public ObservableCollection<double> ReactorPressure { get; set; }
    public ObservableCollection<string> CatalystCollection { get; set; }
    public ObservableCollection<double> CatalystDensity { get; set; }
    public ObservableCollection<double> StrengthFactor { get; set; }
    public ObservableCollection<string> MaterialCollection { get; set; }
    public ObservableCollection<double> NaphthenicHydrocarbons { get; set; }
    public ObservableCollection<double> AromaticHydrocarbons { get; set; }
    public ObservableCollection<double> MaterialsInput { get; set; }
    public ObservableCollection<double> Temperature { get; set; }

    public RelayCommand StartResearchCommand
    {
        get
        {
            return _startResearchCommand ??= new RelayCommand(o =>
            {
                var startInfo = new ProcessStartInfo();
                startInfo.FileName = GetInstallationPath("MATLAB") + "/bin/Matlab.exe"; // путь к исполняемому файлу MATLAB
                startInfo.Arguments = "\"x = -10:0.1:10; y = x.^2; plot(x,y);\"";       // код для выполнения
                Process.Start(startInfo);
            });
        }
    }

    private string GetInstallationPath(string programName)
    {
        // Открываем нужную ветку реестра
        using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall"))
        {
            // Проходим по всем ключам в ветке Uninstall
            foreach (var subKeyName in key.GetSubKeyNames())
            {
                using (var subKey = key.OpenSubKey(subKeyName))
                {
                    // Получаем значение DisplayName и проверяем, соответствует ли оно искомой программе
                    var displayNameValue = subKey.GetValue("DisplayName");

                    if (displayNameValue != null && displayNameValue.ToString().Contains(programName))
                    {
                        // Если нашли нужную программу, то получаем значение InstallLocation и возвращаем его
                        var installLocationValue = subKey.GetValue("InstallLocation");

                        if (installLocationValue != null)
                        {
                            return installLocationValue.ToString();
                        }
                    }
                }
            }
        }

        // Если не нашли программу, то возвращаем пустую строку
        return string.Empty;
    }
}

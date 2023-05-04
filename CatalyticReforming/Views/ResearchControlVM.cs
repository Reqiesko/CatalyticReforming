using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using CatalyticReforming.ViewModels;

using CatalyticReforming.Utils.Commands;
using CatalyticReforming.Utils.Services;
using CatalyticReforming.ViewModels;


using Microsoft.Win32;


namespace CatalyticReforming.Views;

public class ResearchControlVM : ViewModelBase
{
    private NavigationService _navigationService;

    private RelayCommand _startResearchCommand;
    
    public ResearchControlVM(NavigationService navigationService)
    {
        _navigationService = navigationService;
        MatlabCode = "% Создание таблицы/n" +
                     "T = 490:1:503; % диапазон температур" +
                     "/nG = 100:100:1000; % диапазон потока сырья/n" +
                     "[TT, GG] = meshgrid(T, G);/n" +
                     "yn = 0.1; % фиксированное значение/n" +
                     "ya = 0.2; % фиксированное значение/n" +
                     "F = targetFunction(TT, GG, yn, ya);/n" +
                     "tableData = [TT(:), GG(:), F(:)];/n" +
                     "tableHeaders = {'T', 'G', 'F'};/n" +
                     "tableResult = array2table(tableData, 'VariableNames', tableHeaders)/n" +
                     "% Построение графика/n" +
                     "figure;/n" +
                     "surf(TT, GG, F);/n" +
                     "title('Целевая функция');/n" +
                     "xlabel('T');/n" +
                     "ylabel('G');/n" +
                     "zlabel('F');";
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
    
    public string MatlabCode { get; set; }
    public RelayCommand StartResearchCommand
    {
        get
        {
            return _startResearchCommand ??= new RelayCommand(o =>
            {
                var startInfo = new ProcessStartInfo();
                startInfo.FileName = GetInstallationPath("MATLAB") + "/bin/Matlab.exe"; // путь к исполняемому файлу MATLAB
                startInfo.Arguments = "-nodesktop \"" + MatlabCode + "\"";      // код для выполнения
                startInfo.WorkingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/MatlabCodeFiles";
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

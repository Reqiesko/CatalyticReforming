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
    private RelayCommand _changeTemperatureCommand;
    private RelayCommand _changeMaterialCommand;
    private RelayCommand _materialCheckChangeCommand;
    private RelayCommand _temperatureCheckChangeCommand;
    private bool TemperatureCheckBox { get; set; }
    private bool MaterialCheckBox { get; set; }
    public string MatlabCode { get; set; }
    
    
    public ObservableCollection<string> InsallersCollection { get; set; }
    public ObservableCollection<string> ReactorsCollection { get; set; }
    public ObservableCollection<double> ReactorPressure { get; set; }
    public ObservableCollection<string> CatalystCollection { get; set; }
    public ObservableCollection<double> CatalystDensity { get; set; }
    public ObservableCollection<double> StrengthFactor { get; set; }
    public ObservableCollection<string> MaterialCollection { get; set; }
    public ObservableCollection<double> NaphthenicHydrocarbons { get; set; }
    public ObservableCollection<double> AromaticHydrocarbons { get; set; }
    public string MaterialsInput { get; set; }
    public string Temperature { get; set; }
    
    public ResearchControlVM(NavigationService navigationService)
    {
        _navigationService = navigationService;
        MaterialCheckBox = false;
        TemperatureCheckBox = false;
        
        MatlabCode = $"function [tableResult] = targetFunction()/n" +
                     "% Определение параметров функции/n" +
                     "a = 15.802;/n" +
                     "b = 0.03155;/n" +
                     "c = 0.95975;/n" +
                     "d = 2.4206;/n" +
                     "yn = 0.1;/n" +
                     "ya = 0.2;/n" +
                     "% Создание таблицы/n" +
                     "T = " + Temperature + ";/n" +
                     "G = "+ MaterialsInput + ";/n" +
                     "[TT, GG] = meshgrid(T, G);/n" +
                     "F = a - b * TT + c * GG - d*(yn - ya);/n" +
                     "tableData = [TT(:), GG(:), F(:)];/n" +
                     "tableHeaders = {'T', 'G', 'F'};/n" +
                     "tableResult = array2table(tableData, 'VariableNames', tableHeaders);/n" +
                     "% Построение графика/n" +
                     "figure;/n" +
                     "surf(TT, GG, F);/n" +
                     "title('Целевая функция');/n" +
                     "xlabel('T');/n" +
                     "ylabel('G');/n" +
                     "zlabel('F');/n" +
                     "end";
    }

    //uitable('Data',tableResult{:,:},'ColumnName',tableResult.Properties.VariableNames,'Units','Normalized','Position',[0,0,1,1]);
    //tableResult = targetFunction();
    
    
    public RelayCommand StartResearchCommand
    {
        get
        {
            return _startResearchCommand ??= new RelayCommand(o =>
            {
                var startInfo = new ProcessStartInfo();
                startInfo.FileName = GetInstallationPath("MATLAB") + "/bin/Matlab.exe"; // путь к исполняемому файлу MATLAB
                startInfo.Arguments = "-r \"" + MatlabCode + "\"";      // код для выполнения
                //startInfo.WorkingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/MatlabCodeFiles";
                //startInfo.UseShellExecute = false;
                Process.Start(startInfo);
            });
        }
    }


    public RelayCommand ChangeTemperatureCommand
    {
        get
        {
            return _changeTemperatureCommand ??= new RelayCommand(o =>
            {
                TemperatureCheckBox = TemperatureCheckBox != true;
                if (TemperatureCheckBox == true)
                {
                    Temperature = "490:1:503";
                    MatlabCode = $"function [tableResult] = targetFunction()/n" +
                                 "% Определение параметров функции/n" +
                                 "a = 15.802;/n" +
                                 "b = 0.03155;/n" +
                                 "c = 0.95975;/n" +
                                 "d = 2.4206;/n" +
                                 "yn = 0.1;/n" +
                                 "ya = 0.2;/n" +
                                 "% Создание таблицы/n" +
                                 "T = " + Temperature + ";/n" +
                                 "G = "+ MaterialsInput + ";/n" +
                                 "[TT, GG] = meshgrid(T, G);/n" +
                                 "F = a - b * TT + c * GG - d*(yn - ya);/n" +
                                 "tableData = [TT(:), GG(:), F(:)];/n" +
                                 "tableHeaders = {'T', 'G', 'F'};/n" +
                                 "tableResult = array2table(tableData, 'VariableNames', tableHeaders);/n" +
                                 "% Построение графика/n" +
                                 "figure;/n" +
                                 "surf(TT, GG, F);/n" +
                                 "title('Целевая функция');/n" +
                                 "xlabel('T');/n" +
                                 "ylabel('G');/n" +
                                 "zlabel('F');/n" +
                                 "end";
                }
                else
                {
                    return;
                }
            });
        }
    }
    
    public RelayCommand ChangeMaterialCommand
    {
        get
        {
            return _changeMaterialCommand ??= new RelayCommand(o =>
            {
                MaterialCheckBox = MaterialCheckBox != true;
                if (MaterialCheckBox == true)
                {
                    MaterialsInput = "8.87:0.1:16.04";
                    MatlabCode = $"function [tableResult] = targetFunction()/n" +
                                 "% Определение параметров функции/n" +
                                 "a = 15.802;/n" +
                                 "b = 0.03155;/n" +
                                 "c = 0.95975;/n" +
                                 "d = 2.4206;/n" +
                                 "yn = 0.1;/n" +
                                 "ya = 0.2;/n" +
                                 "% Создание таблицы/n" +
                                 "T = " + Temperature + ";/n" +
                                 "G = "+ MaterialsInput + ";/n" +
                                 "[TT, GG] = meshgrid(T, G);/n" +
                                 "F = a - b * TT + c * GG - d*(yn - ya);/n" +
                                 "tableData = [TT(:), GG(:), F(:)];/n" +
                                 "tableHeaders = {'T', 'G', 'F'};/n" +
                                 "tableResult = array2table(tableData, 'VariableNames', tableHeaders);/n" +
                                 "% Построение графика/n" +
                                 "figure;/n" +
                                 "surf(TT, GG, F);/n" +
                                 "title('Целевая функция');/n" +
                                 "xlabel('T');/n" +
                                 "ylabel('G');/n" +
                                 "zlabel('F');/n" +
                                 "end";
                }
                else
                {
                    return;
                }
            });
        }
    }
    
    public RelayCommand TemperatureCheckChangeCommand
    {
        get
        {
            return _temperatureCheckChangeCommand ??= new RelayCommand(o =>
            {
                TemperatureCheckBox = TemperatureCheckBox != true;
            });
        }
    }
    
    public RelayCommand MaterialCheckChangeCommand
    {
        get
        {
            return _materialCheckChangeCommand ??= new RelayCommand(o =>
            {
                MaterialCheckBox = MaterialCheckBox != true;
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

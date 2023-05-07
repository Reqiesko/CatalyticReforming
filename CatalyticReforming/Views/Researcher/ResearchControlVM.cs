using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using CatalyticReforming.ViewModels;

using CatalyticReforming.Utils.Commands;
using CatalyticReforming.Utils.Default_Dialogs;
using CatalyticReforming.Utils.Services;
using CatalyticReforming.ViewModels;


using Microsoft.Win32;


namespace CatalyticReforming.Views;

public class ResearchControlVM : ViewModelBase
{
    private NavigationService _navigationService;
    private readonly DefaultDialogs _defaultDialogs;
    
    private RelayCommand _startResearchCommand;
    private RelayCommand _changeTemperatureCommand;
    private RelayCommand _changeMaterialCommand;
    private RelayCommand _materialCheckChangeCommand;
    private RelayCommand _temperatureChangeCommand;
    private bool TemperatureCheckBox { get; set; }
    private bool MaterialCheckBox { get; set; }
    private string ChangeParameter { get; set; }
    public string MatlabCode { get; set; }
    
    
    public ObservableCollection<string> InsallersCollection { get; set; }
    public ObservableCollection<string> ReactorsCollection { get; set; }
    public ObservableCollection<double> ReactorPressure { get; set; }
    public ObservableCollection<string> CatalystCollection { get; set; }
    public ObservableCollection<double> CatalystDensity { get; set; }
    public ObservableCollection<double> StrengthFactor { get; set; }
    public ObservableCollection<string> MaterialCollection { get; set; }

    private string _naphthenicHydrocarbons;
    private string _aromaticHydrocarbons;

    public string NaphthenicHydrocarbons
    {
        get
        {
            return _naphthenicHydrocarbons;
        }
        set
        {
            if (_naphthenicHydrocarbons != value)
            {
                _naphthenicHydrocarbons = value;
                OnPropertyChanged(nameof(NaphthenicHydrocarbons));
                UpdateMatlabCode();
            }
        }
    }
    public string AromaticHydrocarbons
    {
        get
        {
            return _aromaticHydrocarbons;
        }
        set
        {
            if (_aromaticHydrocarbons != value)
            {
                _aromaticHydrocarbons = value;
                OnPropertyChanged(nameof(AromaticHydrocarbons));
                UpdateMatlabCode();
            }
        }
    }
    
    private string _temperature;
    public string Temperature
    {
        get { return _temperature; }
        set
        {
            if (_temperature != value)
            {
                _temperature = value;
                OnPropertyChanged(nameof(Temperature));
                UpdateMatlabCode();
            }
        }
    }

    private string _materialsInput;
    public string MaterialsInput
    {
        get { return _materialsInput; }
        set
        {
            if (_materialsInput != value)
            {
                _materialsInput = value;
                OnPropertyChanged(nameof(MaterialsInput));
                UpdateMatlabCode();
            }
        }
    }
    
    public ResearchControlVM(NavigationService navigationService, DefaultDialogs defaultDialogs)
    {
        _navigationService = navigationService;
        _defaultDialogs = defaultDialogs;
        MaterialCheckBox = false;
        TemperatureCheckBox = false;
        MatlabCode = $"function[tableResult, optimalValue, optimalOctaneNumber] = targetFunction()\n" +
                     "% Определение параметров функции\n" +
                     "a = 15.802;\n" +
                     "b = 0.03155;\n" +
                     "c = 0.95975;\n" +
                     "d = 2.4206;\n" +
                     "a1 = 2.517;\n" +
                     "b1 = 0.00455;\n" +
                     "c1 = 0.1449;\n" +
                     "d1 = 0.0221;\n" +
                     "yn = " + NaphthenicHydrocarbons + ";\n" +
                     "ya = " + AromaticHydrocarbons + ";\n" +
                     "parameter = " + $"'{ChangeParameter}'" + ";\n" +
                     "% Создание таблицы\n" +
                     "T = " + Temperature + ";\n" +
                     "G = " + MaterialsInput + ";\n" +
                     "[TT, GG] = meshgrid(T, G);\n" +
                     "F = abs(a - b * TT + c * GG - d*(yn - ya));\n" +
                     "tableData = [TT(:), GG(:), F(:)];\n" +
                     "tableHeaders = {'T', 'G', 'F'};\n" +
                     "tableResult = array2table(tableData, 'VariableNames', tableHeaders);\n" +
                     "% Отображение таблицы в новом окне\n" +
                     "figure;\n" +
                     "uitable('Data', tableData, 'ColumnName', tableHeaders);\n" +
                     "% Построение графика в новом окне\n" +
                     "figure;\n" +
                     "if strcmp(parameter, 'T')\n" +
                     "plot(T, F);\n" +
                     "title('Зависимость функции при различных Т');\n" +
                     "xlabel('T');\n" +
                     "ylabel('F');\n" +
                     "elseif strcmp(parameter, 'G')\n" +
                     "plot(G, F);\n" +
                     "title('Зависимость функции при различных G');\n" +
                     "xlabel('G');\n" +
                     "ylabel('F');\n" +
                     "end\n" +
                     "% Поиск оптимального значения параметра\n" +
                     "if strcmp(parameter, 'T')\n" +
                     "[~, index] = max(F);\n" +
                     "optimalValue = G(index);\n" +
                     "elseif strcmp(parameter, 'G')\n" +
                     "[~, index] = max(F);\n" +
                     "optimalValue = T(index);\n" +
                     "end\n" +
                     "% Расчет октанового числа при оптимальном значении параметра\n" +
                     "optimalOctaneNumber = a1 + b1 * optimalValue - c1 * optimalValue + d1*(yn - ya);\n" +
                     "end";
    }

    //uitable('Data',tableResult{:,:},'ColumnName',tableResult.Properties.VariableNames,'Units','Normalized','Position',[0,0,1,1]);
    //tableResult = targetFunction();
    
    
    public RelayCommand StartResearchCommand
    {
        get
        {
            return _startResearchCommand ??= new RelayCommand(async o =>
            {
                if (MaterialCheckBox == true || TemperatureCheckBox == true)
                {
                    var scriptPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                                     "/MatlabCodeFiles/targetFunction.m";
                    var matlabCodeFile = new StreamWriter(scriptPath);
                    matlabCodeFile.Write(MatlabCode);
                    matlabCodeFile.Close();
                    var arguments = $"[tableResult, optimalValue, optimalOctaneNumber] = targetFunction();";
                    var startInfo = new ProcessStartInfo();
                    startInfo.FileName = GetInstallationPath("MATLAB") + "/bin/Matlab.exe"; // путь к исполняемому файлу MATLAB
                    startInfo.Arguments = $"-r \"run('{scriptPath}'); arguments;\"";      // код для выполнения
                    startInfo.WorkingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/MatlabCodeFiles";
                    //startInfo.UseShellExecute = false;
                    Process.Start(startInfo);
                }
                else
                {
                    var mbRes = await _defaultDialogs.ShowWarning("Выберите варьируемый параметр");
                }
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
                    MaterialsInput = "10";
                    ChangeParameter = "G";
                    UpdateMatlabCode();
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
                    Temperature = "500";
                    ChangeParameter = "T";
                    UpdateMatlabCode();
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
            return _temperatureChangeCommand ??= new RelayCommand(o =>
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
    
    private void UpdateMatlabCode()
    {
        string matlabCode = $"function[tableResult, optimalValue, optimalOctaneNumber] = targetFunction()\n" +
                            "% Определение параметров функции\n" +
                            "a = 15.802;\n" +
                            "b = 0.03155;\n" +
                            "c = 0.95975;\n" +
                            "d = 2.4206;\n" +
                            "a1 = 2.517;\n" +
                            "b1 = 0.00455;\n" +
                            "c1 = 0.1449;\n" +
                            "d1 = 0.0221;\n" +
                            "yn = " + NaphthenicHydrocarbons + ";\n" +
                            "ya = " + AromaticHydrocarbons + ";\n" +
                            "parameter = " + $"'{ChangeParameter}'" + ";\n" +
                            "% Создание таблицы\n" +
                            "T = " + Temperature + ";\n" +
                            "G = " + MaterialsInput + ";\n" +
                            "[TT, GG] = meshgrid(T, G);\n" +
                            "F = abs(a - b * TT + c * GG - d*(yn - ya));\n" +
                            "tableData = [TT(:), GG(:), F(:)];\n" +
                            "tableHeaders = {'T', 'G', 'F'};\n" +
                            "tableResult = array2table(tableData, 'VariableNames', tableHeaders);\n" +
                            "% Отображение таблицы в новом окне\n" +
                            "figure;\n" +
                            "uitable('Data', tableData, 'ColumnName', tableHeaders);\n" +
                            "% Построение графика в новом окне\n" +
                            "figure;\n" +
                            "if strcmp(parameter, 'G')\n" +
                            "plot(T, F);\n" +
                            "title('Зависимость функции при различных Т');\n" +
                            "xlabel('T');\n" +
                            "ylabel('F');\n" +
                            "elseif strcmp(parameter, 'T')\n" +
                            "plot(G, F);\n" +
                            "title('Зависимость функции при различных G');\n" +
                            "xlabel('G');\n" +
                            "ylabel('F');\n" +
                            "end\n" +
                            "% Поиск оптимального значения параметра\n" +
                            "if strcmp(parameter, 'T')\n" +
                            "[~, index] = max(F);\n" +
                            "optimalValue = G(index);\n" +
                            "elseif strcmp(parameter, 'G')\n" +
                            "[~, index] = max(F);\n" +
                            "optimalValue = T(index);\n" +
                            "end\n" +
                            "% Расчет октанового числа при оптимальном значении параметра\n" +
                            "optimalOctaneNumber = a1 + b1 * optimalValue - c1 * optimalValue + d1*(yn - ya);\n" +
                            "end";
        MatlabCode = matlabCode;
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
    
    private RelayCommand _changeUserCommand;

    public RelayCommand ChangeUserCommand
    {
        get
        {
            return _changeUserCommand ??= new RelayCommand(o =>
            {
                _navigationService.ChangeContent<LoginControl>();
            });
        }
    }

    private RelayCommand _navigateBackCommand;

    public RelayCommand NavigateBackCommand
    {
        get
        {
            return _navigateBackCommand ??= new RelayCommand(o =>
            {
                _navigationService.ChangeContent<StartControl>();
            });
        }
    }

}

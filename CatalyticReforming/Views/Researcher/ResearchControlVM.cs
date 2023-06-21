using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

using CatalyticReforming.Utils.Commands;
using CatalyticReforming.Utils.Default_Dialogs;
using CatalyticReforming.Utils.Services;
using CatalyticReforming.ViewModels;
using CatalyticReforming.ViewModels.DAL_VM.domain;
using CatalyticReforming.ViewModels.DAL_VM.test;
using CatalyticReforming.Views.Auth;

using DAL.Models.domain;

using Microsoft.Win32;


namespace CatalyticReforming.Views.Researcher;

public class ResearchControlVM : ViewModelBase
{
    private readonly DefaultDialogs _defaultDialogs;

    private RelayCommand _changeMaterialCommand;
    private RelayCommand _changeTemperatureCommand;

    private RelayCommand _changeUserCommand;
    private RelayCommand _materialCheckChangeCommand;
    private RelayCommand _selectedMethodChangedCommand;
    private RelayCommand _startTestMatlabCommand;

    private string _materialsInput;

    private double _naphthenicHydrocarbons;
    private double _aromaticHydrocarbons;

    private RelayCommand _navigateBackCommand;
    private readonly NavigationService _navigationService;

    private RelayCommand _startResearchCommand;

    private string _temperature;
    private RelayCommand _temperatureChangeCommand;
    private MaterialVM _selectedMaterial;
    private string _selectedMethod;
    private double _octaineNumberBounds;
    private RelayCommand _openStudyBookCommand;

    public ResearchControlVM(NavigationService navigationService, DefaultDialogs defaultDialogs, GenericRepository repository)
    {
        _navigationService = navigationService;
        _defaultDialogs = defaultDialogs;
        MaterialCheckBox = false;
        TemperatureCheckBox = false;

        Installations = new ObservableCollection<InstallationVM>(repository.GetAll<InstallationVM, Installation>().Result);
        SelectedInstallation = Installations.First();

        Materials = new ObservableCollection<MaterialVM>(repository.GetAll<MaterialVM, Material>().Result);
        SelectedMaterial = Materials.First();

        Catalysts = new ObservableCollection<CatalystVM>(repository.GetAll<CatalystVM, Catalyst>().Result);
        SelectedCatalyst = Catalysts.First();

        Methods = new ObservableCollection<string>(new List<string> { "Метод золотого сечения", "Метод половинного деления", "Метод сканирования", "Метод чисел Фибоначчи" });
        SelectedMethod = Methods.First();

        TestFunctions = new ObservableCollection<TestFunctionVM>(new List<TestFunctionVM>
        {
            new() {FunctionContent = "F = x + log(x) - x^3", Id = 0},
            new() {FunctionContent = "F = x^4 + x^2 + x + 1", Id = 1},
            new() {FunctionContent = "F = x - exp(x^2)", Id = 2},
            new() {FunctionContent = "F = 1/x + sqrt(x)", Id = 3},
        });
        SelectedFunction = TestFunctions.First();

        MatlabCode = "function[tableResult, optimalValue, optimalOctaneNumber] = targetFunction()\n" +
                     "% Определение параметров функции\n" +
                     "a2 = 0.0802;\n" +
                     "b2 = 0.814;\n" +
                     "c2 = 0.975;\n" +
                     "d2 = 0.206;\n" +
                     "e = 0.0181;\n" +
                     "f = 0.093;\n" +
                     "a3 = 0.802;\n" +
                     "b3 = 0.814;\n" +
                     "c3 = 0.975;\n" +
                     "d3 = 0.206;\n" +
                     "e1 = 0.0181;\n" +
                     "f1 = 0.093;\n" +
                     "%yn = " + NaphthenicHydrocarbons + ";\n" +
                     "%ya = " + AromaticHydrocarbons + ";\n" +
                     "parameter = " + $"'{ChangeParameter}'" + ";\n" +
                     "% Создание таблицы\n" +
                     "T = " + Temperature + ";\n" +
                     "G = " + MaterialsInput + ";\n" +
                     "[TT, GG] = meshgrid(T, G);\n" +
                     "F = - (a2 + b2 .* (G - 40.3) + c2 .* (T-482)+ d2 .* (G-40.3) .* (T-482) + e .*(G-40.3).^2 + f .* (T-482).^2);\n" +
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
                     "% Поиск оптимального значения параметра методом золотого сечения(с помощью fminbnd)\n" +
                     "if strcmp(parameter, 'T')\n" +
                     "f = @(x)+ ( a2 + b2 * (x - 40.3) + c2 * (T-482)+ d2 * (x-40.3) * (T-482) + e *(x-40.3)^2 + f *(T-482)^2);\n" +
                     "[optimalValue, ~] = fminbnd(f, G(1), G(end));\n" +
                     "elseif strcmp(parameter, 'G')\n" +
                     "f = @(x)+ (a2 + b2 * (G - 40.3) + c2 * (x-482)+ d2 * (G-40.3) * (x-482) + e* (G-40.3)^2 + f *(x-482)^2);\n" +
                     "[optimalValue, ~] = fminbnd(f, T(1), T(end));\n" +
                     "end\n" +
                     "    \r % Расчет октанового числа при оптимальном значении параметра\r\n" +
                     "    if strcmp(parameter, 'T')\r\n" +
                     "       optimalOctaneNumber = abs(a3+b3*(optimalValue-40.3)+c3*(T-482)+d3*(optimalValue-40.3)*(T-482)+e1*(optimalValue-40.3)^2+f1*(T-482)^2);\r\n" +
                     "       fprintf('\\nОктановое число: ');\r\n       " +
                     "       disp(optimalOctaneNumber);\r\n       " +
                     "       fprintf('Оптимальный расход сырья: ');\r\n      " +
                     "       disp(optimalValue);\n" +
                     "    elseif strcmp(parameter, 'G')\r\n" +
                     "       optimalOctaneNumber = abs(a3+b3*(G-40.3)+c3*(optimalValue-482)+d3*(G-40.3)*(optimalValue-482)+e1*(G-40.3)^2+f1*(optimalValue-482)^2);\r\n" +
                     "       fprintf('\\nОктановое число: ');\r\n       " +
                     "       disp(optimalOctaneNumber);\r\n       " +
                     "       fprintf('Оптимальная температура: ');\r\n      " +
                     "       disp(optimalValue);\n" +
                     "    end\r\n" +
                     "    % Проверка критерия октанового числа\r\n" +
                     "    targetOctaneNumber =" + $"{OctaineNumberBounds};" +
                     "  % Критериальное значение октанового числа\r\n" +
                     "    if optimalOctaneNumber < targetOctaneNumber\r\n" +
                     "        fprintf('Не удалось получить решение, это может быть вызвано слишком большим значением критериального ограничения, попробуйте его снизить.\\n');\r\n" +
                     "    end";
    }

    private bool TemperatureCheckBox { get; set; }
    private bool MaterialCheckBox { get; set; }
    private string ChangeParameter { get; set; }
    public string MatlabCode { get; set; }

    public double OctaineNumberBounds
    {
        get => _octaineNumberBounds;
        set
        {
            if (_octaineNumberBounds != value)
            {
                _octaineNumberBounds = value;
                OnPropertyChanged();
                UpdateMatlabCode();
            }
        }
    }

    public ObservableCollection<InstallationVM> Installations { get; set; }
    public InstallationVM SelectedInstallation { get; set; }
    public ObservableCollection<CatalystVM> Catalysts { get; set; }

    public CatalystVM SelectedCatalyst { get; set; }
    public ObservableCollection<MaterialVM> Materials { get; set; }

    public MaterialVM SelectedMaterial
    {
        get => _selectedMaterial;
        set
        {
            if (_selectedMaterial != value)
            {
                _selectedMaterial = value;
                NaphthenicHydrocarbons = _selectedMaterial.NaphthenicHydrocarbonsContent;
                AromaticHydrocarbons = _selectedMaterial.AromaticHydrocarbonsContent;
                OnPropertyChanged();
                UpdateMatlabCode();
            }
        }
    }

    public ObservableCollection<TestFunctionVM> TestFunctions { get; set; }

    public TestFunctionVM SelectedFunction { get; set; }

    public double NaphthenicHydrocarbons
    {
        get => _naphthenicHydrocarbons;
        set
        {
            if (_naphthenicHydrocarbons != value)
            {
                _naphthenicHydrocarbons = value;
                OnPropertyChanged();
                UpdateMatlabCode();
            }
        }
    }

    public double AromaticHydrocarbons
    {
        get => _aromaticHydrocarbons;
        set
        {
            if (_aromaticHydrocarbons != value)
            {
                _aromaticHydrocarbons = value;
                OnPropertyChanged();
                UpdateMatlabCode();
            }
        }
    }

    public string Temperature
    {
        get => _temperature;
        set
        {
            if (_temperature != value)
            {
                _temperature = value;
                OnPropertyChanged();
                UpdateMatlabCode();
            }
        }
    }

    public string MaterialsInput
    {
        get => _materialsInput;
        set
        {
            if (_materialsInput != value)
            {
                _materialsInput = value;
                OnPropertyChanged();
                UpdateMatlabCode();
            }
        }
    }

    public ObservableCollection<string> Methods { get; set; }

    public string SelectedMethod
    {
        get => _selectedMethod;
        set
        {
            if (_selectedMethod != value)
            {
                _selectedMethod = value;
                OnPropertyChanged();
                UpdateMatlabCode();
            }
        }
    }

    public RelayCommand StartResearchCommand
    {
        get
        {
            return _startResearchCommand ??= new RelayCommand(async o =>
            {
                if (MaterialCheckBox || TemperatureCheckBox)
                {
                    var scriptPath = GetDirectoryName("targetFunction.m");
                    var matlabCodeFile = new StreamWriter(scriptPath);
                    matlabCodeFile.Write(MatlabCode);
                    matlabCodeFile.Close();
                    ExecMatlab("", scriptPath);
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

                if (TemperatureCheckBox)
                {
                    Temperature = "490:1:503";
                    MaterialsInput = "10";
                    ChangeParameter = "G";
                    UpdateMatlabCode();
                }
                else
                {
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

                if (MaterialCheckBox)
                {
                    MaterialsInput = "8.87:0.1:16.04";
                    Temperature = "500";
                    ChangeParameter = "T";
                    UpdateMatlabCode();
                }
                else
                {
                }
            });
        }
    }

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

    public RelayCommand StartTestMatlabCommand
    {
        get
        {
            return _startTestMatlabCommand ??= new RelayCommand(o =>
            {
                if (TestFunctions == null)
                    return;
                switch (SelectedFunction.Id)
                {
                    case 0:
                        ExecMatlab("", GetDirectoryName("exampleFunction.m"));
                        break;
                    case 1:
                        ExecMatlab("", GetDirectoryName("exampleFunction1.m"));
                        break;
                    case 2:
                        ExecMatlab("", GetDirectoryName("exampleFunction2.m"));
                        break;
                    case 3:
                        ExecMatlab("", GetDirectoryName("exampleFunction3.m"));
                        break;
                }
            });
        }
    }

    public RelayCommand OpenStudyBookCommand
    {
        get
        {
            return _openStudyBookCommand ??= new RelayCommand(o =>
            {
                var proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = "Методы оптимизации в среде MATLAB.pdf";
                proc.StartInfo.UseShellExecute = true;
                proc.Start();

            });
        }
    }

    private void UpdateMatlabCode()
    {
        if (_selectedMethod == null)
            return;
        switch (_selectedMethod)
        {
            case "Метод золотого сечения":
                {
                    var matlabCode = "function[tableResult, optimalValue, optimalOctaneNumber] = targetFunction()\n" +
                                     "% Определение параметров функции\n" +
                                     "a2 = 0.0802;\n" +
                                     "b2 = 0.814;\n" +
                                     "c2 = 0.975;\n" +
                                     "d2 = 0.206;\n" +
                                     "e = 0.0181;\n" +
                                     "f = 0.093;\n" +
                                     "a3 = 0.802;\n" +
                                     "b3 = 0.814;\n" +
                                     "c3 = 0.975;\n" +
                                     "d3 = 0.206;\n" +
                                     "e1 = 0.0181;\n" +
                                     "f1 = 0.093;\n" +
                                     "%yn = " + NaphthenicHydrocarbons + ";\n" +
                                     "%ya = " + AromaticHydrocarbons + ";\n" +
                                     "parameter = " + $"'{ChangeParameter}'" + ";\n" +
                                     "% Создание таблицы\n" +
                                     "T = " + Temperature + ";\n" +
                                     "G = " + MaterialsInput + ";\n" +
                                     "[TT, GG] = meshgrid(T, G);\n" +
                                     "F = - (a2 + b2 .* (G - 40.3) + c2 .* (T-482)+ d2 .* (G-40.3) .* (T-482) + e .*(G-40.3).^2 + f .* (T-482).^2);\n" +
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
                                     "% Поиск оптимального значения параметра методом золотого сечения(с помощью fminbnd)\n" +
                                     "if strcmp(parameter, 'T')\n" +
                                     "f = @(x)+ ( a2 + b2 * (x - 40.3) + c2 * (T-482)+ d2 * (x-40.3) * (T-482) + e *(x-40.3)^2 + f *(T-482)^2);\n" +
                                     "[optimalValue, ~] = fminbnd(f, G(1), G(end));\n" +
                                     "elseif strcmp(parameter, 'G')\n" +
                                     "f = @(x)+ (a2 + b2 * (G - 40.3) + c2 * (x-482)+ d2 * (G-40.3) * (x-482) + e* (G-40.3)^2 + f *(x-482)^2);\n" +
                                     "[optimalValue, ~] = fminbnd(f, T(1), T(end));\n" +
                                     "end\n" +
                                     "    \r % Расчет октанового числа при оптимальном значении параметра\r\n" +
                                     "    if strcmp(parameter, 'T')\r\n" +
                                     "       optimalOctaneNumber = abs(a3+b3*(optimalValue-40.3)+c3*(T-482)+d3*(optimalValue-40.3)*(T-482)+e1*(optimalValue-40.3)^2+f1*(T-482)^2);\r\n" +
                                     "       fprintf('\\nОктановое число: ');\r\n       " +
                                     "       disp(optimalOctaneNumber);\r\n       " +
                                     "       fprintf('Оптимальный расход сырья: ');\r\n      " +
                                     "       disp(optimalValue);\n" +
                                     "       disp(tableResult);\n" +
                                     "    elseif strcmp(parameter, 'G')\r\n" +
                                     "       optimalOctaneNumber = abs(a3+b3*(G-40.3)+c3*(optimalValue-482)+d3*(G-40.3)*(optimalValue-482)+e1*(G-40.3)^2+f1*(optimalValue-482)^2);\r\n" +
                                     "       fprintf('\\nОктановое число: ');\r\n       " +
                                     "       disp(optimalOctaneNumber);\r\n       " +
                                     "       fprintf('Оптимальная температура: ');\r\n      " +
                                     "       disp(optimalValue);\n" +
                                     "       disp(tableResult);\n" +
                                     "    end\r\n" +
                                     "    % Проверка критерия октанового числа\r\n" +
                                     "    targetOctaneNumber =" + $" {OctaineNumberBounds};" +
                                     "  % Критериальное значение октанового числа\r\n" +
                                     "    if optimalOctaneNumber < targetOctaneNumber\r\n" +
                                     "        fprintf('Не удалось получить решение, это может быть вызвано слишком большим значением критериального ограничения, попробуйте его снизить.\\n');\r\n" +
                                     "    end";

                    MatlabCode = matlabCode;
                    break;
                }
            case "Метод половинного деления":
                {
                    var matlabCode = "function[tableResult, optimalValue, optimalOctaneNumber] = targetFunction()\n" +
                                     "% Определение параметров функции\n" +
                                     "a2 = 0.0802;\n" +
                                     "b2 = 0.814;\n" +
                                     "c2 = 0.975;\n" +
                                     "d2 = 0.206;\n" +
                                     "e = 0.0181;\n" +
                                     "f = 0.093;\n" +
                                     "a3 = 0.802;\n" +
                                     "b3 = 0.814;\n" +
                                     "c3 = 0.975;\n" +
                                     "d3 = 0.206;\n" +
                                     "e1 = 0.0181;\n" +
                                     "f1 = 0.093;\n" +
                                     "%yn = " + NaphthenicHydrocarbons + ";\n" +
                                     "%ya = " + AromaticHydrocarbons + ";\n" +
                                     "parameter = " + $"'{ChangeParameter}'" + ";\n" +
                                     "% Создание таблицы\n" +
                                     "T = " + Temperature + ";\n" +
                                     "G = " + MaterialsInput + ";\n" +
                                     "[TT, GG] = meshgrid(T, G);\n" +
                                     "F = - (a2 + b2 .* (G - 40.3) + c2 .* (T-482)+ d2 .* (G-40.3) .* (T-482) + e .*(G-40.3).^2 + f .* (T-482).^2);\n" +
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
                                     "% Поиск оптимального значения параметра методом половинного деления (бисекции)\r\n" +
                                     "    tol = 0.001;" +
                                     "  % Погрешность\r\n " +
                                     "   iter = 0;\r\n " +
                                     "   if strcmp(parameter, 'T')\r\n  " +
                                     "      G1 = G(1);\r\n " +
                                     "       G2 = G(end);\r\n" +
                                     "        while (abs(G2 - G1) > tol)\r\n " +
                                     "           curr = (G1 + G2) / 2;\r\n " +
                                     "           F1 = - (a2 + b2 * (G - 40.3) + c2 * (T-482)+ d2 * (G-40.3) * (T-482) + e *(G-40.3).^2 + f * (T-482).^2);\r\n" +
                                     "           F2 = - (a2 + b2 * (curr - 40.3) + c2 * (T-482)+ d2 * (curr-40.3) * (T-482) + e *(curr-40.3).^2 + f * (T-482).^2);\r\n" +
                                     "           if (F1 > F2)\r\n " +
                                     "               G2 = curr;\r\n " +
                                     "           else\r\n " +
                                     "               G1 = curr;\r\n " +
                                     "           end\r\n" +
                                     "            iter = iter + 1;\r\n" +
                                     "            fprintf('Iteration: %d, a: %.6f, b: %.6f\\n', iter, G1, G2);\r\n " +
                                     "       end\r\n" +
                                     "        optimalValue = (G1 + G2) / 2;\r\n" +
                                     "    elseif strcmp(parameter, 'G')\r\n" +
                                     "        T1 = T(1);\r\n" +
                                     "        T2 = T(end);\r\n" +
                                     "        while (abs(T2 - T1) > tol)\r\n" +
                                     "            curr = (T1 + T2) / 2;\r\n" +
                                     "            F1 = - (a2 + b2 * (G - 40.3) + c2 * (T-482)+ d2 * (G-40.3) * (T-482) + e *(G-40.3).^2 + f * (T-482).^2);\r\n" +
                                     "            F2 = - (a2 + b2 * (G - 40.3) + c2 * (curr-482)+ d2 * (G-40.3) * (curr-482) + e *(G-40.3).^2 + f * (curr-482).^2);\r\n" +
                                     "            if (F1 > F2)\r\n" +
                                     "                T2 = curr;\r\n" +
                                     "            else\r\n " +
                                     "               T1 = curr;\r\n " +
                                     "           end\r\n" +
                                     "            iter = iter + 1;\r\n" +
                                     "            fprintf('Iteration: %d, a: %.6f, b: %.6f\\n', iter, T1, T2);\r\n " +
                                     "       end\r\n" +
                                     "        optimalValue = (T1 + T2) / 2;\r\n" +
                                     "    end\r\n" +
                                     "    \r % Расчет октанового числа при оптимальном значении параметра\r\n" +
                                     "    if strcmp(parameter, 'T')\r\n" +
                                     "       optimalOctaneNumber = abs(a3+b3*(optimalValue-40.3)+c3*(T-482)+d3*(optimalValue-40.3)*(T-482)+e1*(optimalValue-40.3)^2+f1*(T-482)^2);\r\n" +
                                     "       fprintf('\\nОктановое число: ');\r\n       " +
                                     "       disp(optimalOctaneNumber);\r\n       " +
                                     "       fprintf('Оптимальный расход сырья: ');\r\n      " +
                                     "       disp(optimalValue);\n" +
                                     "       disp(tableResult);\n" +
                                     "    elseif strcmp(parameter, 'G')\r\n" +
                                     "       optimalOctaneNumber = abs(a3+b3*(G-40.3)+c3*(optimalValue-482)+d3*(G-40.3)*(optimalValue-482)+e1*(G-40.3)^2+f1*(optimalValue-482)^2);\r\n" +
                                     "       fprintf('\\nОктановое число: ');\r\n       " +
                                     "       disp(optimalOctaneNumber);\r\n       " +
                                     "       fprintf('Оптимальная температура: ');\r\n      " +
                                     "       disp(optimalValue);\n" +
                                     "       disp(tableResult);\n" +
                                     "    end\r\n" +
                                     "    % Проверка критерия октанового числа\r\n" +
                                     "    targetOctaneNumber =" + $" {OctaineNumberBounds};" +
                                     "  % Критериальное значение октанового числа\r\n" +
                                     "    if optimalOctaneNumber < targetOctaneNumber\r\n" +
                                     "        fprintf('Не удалось получить решение, это может быть вызвано слишком большим значением критериального ограничения, попробуйте его снизить.\\n');\r\n" +
                                     "    end";

                    MatlabCode = matlabCode;
                    break;
                }
            case "Метод сканирования":
                {
                    var matlabCode = "function[tableResult, optimalValue, optimalOctaneNumber] = targetFunction()\n" +
                                     "% Определение параметров функции\n" +
                                     "a2 = 0.0802;\n" +
                                     "b2 = 0.814;\n" +
                                     "c2 = 0.975;\n" +
                                     "d2 = 0.206;\n" +
                                     "e = 0.0181;\n" +
                                     "f = 0.093;\n" +
                                     "a3 = 0.802;\n" +
                                     "b3 = 0.814;\n" +
                                     "c3 = 0.975;\n" +
                                     "d3 = 0.206;\n" +
                                     "e1 = 0.0181;\n" +
                                     "f1 = 0.093;\n" +
                                     "%yn = " + NaphthenicHydrocarbons + ";\n" +
                                     "%ya = " + AromaticHydrocarbons + ";\n" +
                                     "parameter = " + $"'{ChangeParameter}'" + ";\n" +
                                     "% Создание таблицы\n" +
                                     "T = " + Temperature + ";\n" +
                                     "G = " + MaterialsInput + ";\n" +
                                     "[TT, GG] = meshgrid(T, G);\n" +
                                     "F = - (a2 + b2 .* (G - 40.3) + c2 .* (T-482)+ d2 .* (G-40.3) .* (T-482) + e .*(G-40.3).^2 + f .* (T-482).^2);\n" +
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
                                     "% Поиск оптимального значения параметра методом сканирования\r\n" +
                                     "    if strcmp(parameter, 'T')\r\n" +
                                     "        optimalValue = G(1);\r\n " +
                                     "       minF =  - (a2 + b2 * (optimalValue - 40.3) + c2 * (T-482)+ d2 * (optimalValue-40.3) * (T-482) + e *(optimalValue-40.3)^2 + f * (T-482)^2);\r\n" +
                                     "        for i = 2:length(G)\r\n" +
                                     "            f =   - (a2 + b2 * (G(i) - 40.3) + c2 * (T-482)+ d2 * (G(i)-40.3) * (T-482) + e *(G(i)-40.3)^2 + f * (T-482)^2);\r\n" +
                                     "            if f > minF\r\n" +
                                     "                minF = f;\r\n " +
                                     "               optimalValue = G(i);\r\n" +
                                     "            end\r\n " +
                                     "       end\r\n    elseif strcmp(parameter, 'G')\r\n" +
                                     "        optimalValue = T(1);\r\n" +
                                     "        minF = - (a2 + b2 * (G - 40.3) + c2 * (optimalValue-482)+ d2 * (G-40.3) * (optimalValue-482) + e *(G-40.3)^2 + f * (optimalValue-482)^2);\r\n" +
                                     "        for i = 2:length(T)\r\n" +
                                     "            f = - (a2 + b2 * (G - 40.3) + c2 * (T(i)-482)+ d2 * (G-40.3) * (T(i)-482) + e *(G-40.3)^2 + f * (T(i)-482)^2);\r\n" +
                                     "            if f > minF\r\n" +
                                     "                minF = f;\r\n" +
                                     "                optimalValue = T(i);\r\n" +
                                     "            end\r\n" +
                                     "        end\r\n" +
                                     "    end\n" +
                                     "    \r % Расчет октанового числа при оптимальном значении параметра\r\n" +
                                     "    if strcmp(parameter, 'T')\r\n" +
                                     "       optimalOctaneNumber = abs(a3+b3*(optimalValue-40.3)+c3*(T-482)+d3*(optimalValue-40.3)*(T-482)+e1*(optimalValue-40.3)^2+f1*(T-482)^2);\r\n" +
                                     "       fprintf('\\nОктановое число: ');\r\n       " +
                                     "       disp(optimalOctaneNumber);\r\n       " +
                                     "       fprintf('Оптимальный расход сырья: ');\r\n      " +
                                     "       disp(optimalValue);\n" +
                                     "       disp(tableResult);\n" +
                                     "    elseif strcmp(parameter, 'G')\r\n" +
                                     "       optimalOctaneNumber = abs(a3+b3*(G-40.3)+c3*(optimalValue-482)+d3*(G-40.3)*(optimalValue-482)+e1*(G-40.3)^2+f1*(optimalValue-482)^2);\r\n" +
                                     "       fprintf('\\nОктановое число: ');\r\n       " +
                                     "       disp(optimalOctaneNumber);\r\n       " +
                                     "       fprintf('Оптимальная температура: ');\r\n      " +
                                     "       disp(optimalValue);\n" +
                                     "       disp(tableResult);\n" +
                                     "    end\r\n" +
                                     "    % Проверка критерия октанового числа\r\n" +
                                     "    targetOctaneNumber =" + $" {OctaineNumberBounds};" +
                                     "  % Критериальное значение октанового числа\r\n" +
                                     "    if optimalOctaneNumber < targetOctaneNumber\r\n" +
                                     "        fprintf('Не удалось получить решение, это может быть вызвано слишком большим значением критериального ограничения, попробуйте его снизить.\\n');\r\n" +
                                     "    end";

                    MatlabCode = matlabCode;
                    break;
                }
            case "Метод чисел Фибоначчи":
                {
                    var matlabCode = "function[tableResult, optimalValue, optimalOctaneNumber] = targetFunction()\n" +
                                     "% Определение параметров функции\n" +
                                     "a2 = 0.0802;\n" +
                                     "b2 = 0.814;\n" +
                                     "c2 = 0.975;\n" +
                                     "d2 = 0.206;\n" +
                                     "e = 0.0181;\n" +
                                     "f = 0.093;\n" +
                                     "a3 = 0.802;\n" +
                                     "b3 = 0.814;\n" +
                                     "c3 = 0.975;\n" +
                                     "d3 = 0.206;\n" +
                                     "e1 = 0.0181;\n" +
                                     "f1 = 0.093;\n" +
                                     "%yn = " + NaphthenicHydrocarbons + ";\n" +
                                     "%ya = " + AromaticHydrocarbons + ";\n" +
                                     "parameter = " + $"'{ChangeParameter}'" + ";\n" +
                                     "% Создание таблицы\n" +
                                     "T = " + Temperature + ";\n" +
                                     "G = " + MaterialsInput + ";\n" +
                                     "[TT, GG] = meshgrid(T, G);\n" +
                                     "F = - (a2 + b2 .* (G - 40.3) + c2 .* (T-482)+ d2 .* (G-40.3) .* (T-482) + e .*(G-40.3).^2 + f .* (T-482).^2);\n" +
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
                                     "  % Поиск оптимального значения параметра методом чисел Фибоначчи\r\n" +
                                     "    if strcmp(parameter, 'T')\r\n" +
                                     "        n = length(G);\r\n" +
                                     "        fib = fibonacci(n+1);\n" +
                                     "  % Генерация чисел Фибоначчи\r\n" +
                                     "        I = 1:n;\r\n" +
                                     "        K = I + fib(n-1);\r\n" +
                                     "        J = I + fib(n-2);\r\n" +
                                     "        \r\n" +
                                     "        a_k = G(K);\r\n" +
                                     "        a_j = G(J);\r\n " +
                                     "       \r\n " +
                                     "       f_k = -(a2 + b2 .* (a_k - 40.3) + c2 .* (T - 482) + d2 .* (a_k - 40.3) .* (T - 482) + e .* (a_k - 40.3).^2 + f .* (T - 482).^2);\r\n" +
                                     "        f_j = -(a2 + b2 .* (a_j - 40.3) + c2 .* (T - 482) + d2 .* (a_j - 40.3) .* (T - 482) + e .* (a_j - 40.3).^2 + f .* (T - 482).^2);\r\n" +
                                     "        \r\n" +
                                     "        for i = 3:n\r\n" +
                                     "            if f_k(i) < f_j(i)\r\n" +
                                     "                a_j(i) = a_k(i);\r\n" +
                                     "                a_k(i) = a_j(i) + (G(n+1) - a_j(i)) * fib(n-i+2) / fib(n-i+3);\r\n" +
                                     "                f_j(i) = f_k(i);\r\n" +
                                     "                f_k(i) = -(a2 + b2 .* (a_k(i) - 40.3) + c2 .* (T - 482) + d2 .* (a_k(i) - 40.3) .* (T - 482) + e .* (a_k(i) - 40.3).^2 + f .* (T - 482).^2);\r\n" +
                                     "            else\r\n" +
                                     "                a_k(i) = a_j(i);\r\n" +
                                     "                a_j(i) = a_k(i) + (a_k(i) - a_j(i-1)) * fib(n-i+1) / fib(n-i+3);\r\n" +
                                     "                f_k(i) = f_j(i);\r\n" +
                                     "                f_j(i) = (a2 + b2 .* (a_j(i) - 40.3) + c2 .* (T - 482) + d2 .* (a_j(i) - 40.3) .* (T - 482) + e .* (a_j(i) - 40.3).^2 + f .* (T - 482).^2);\r\n" +
                                     "            end\r\n" +
                                     "        end\r\n" +
                                     "        \r\n" +
                                     "        [~, index] = min(f_k);\r\n" +
                                     "        optimalValue = a_k(index);\r\n" +
                                     "        \r\n    elseif strcmp(parameter, 'G')\r\n" +
                                     "        n = length(T);\r\n" +
                                     "        fib = fibonacci(n+1);  % Генерация чисел Фибоначчи\r\n" +
                                     "        I = 1:n;\r\n" +
                                     "        K = I + fib(n-2);\r\n" +
                                     "        J = I + fib(n-2);\r\n" +
                                     "        \r\n        a_k = T(K);\r\n" +
                                     "        a_j = T(J);\r\n" +
                                     "        \r\n        f_k = -(a2 + b2 .* (G - 40.3) + c2 .* (a_k - 482) + d2 .* (G - 40.3) .* (a_k - 482) + e .* (G - 40.3).^2 + f .* (a_k - 482).^2);\r\n" +
                                     "        f_j = -(a2 + b2 .* (G - 40.3) + c2 .* (a_j - 482) + d2 .* (G - 40.3) .* (a_j - 482) + e .* (G - 40.3).^2 + f .* (a_j - 482).^2);\r\n" +
                                     "        \r\n        for i = 3:n\r\n" +
                                     "            if f_k(i) < f_j(i)\r\n" +
                                     "                a_j(i) = a_k(i);\r\n" +
                                     "                a_k(i) = a_j(i) + (T(n+1) - a_j(i)) * fib(n-i+2) / fib(n-i+3);\r\n" +
                                     "                f_j(i) = f_k(i);\r\n" +
                                     "                f_k(i) = -(a2 + b2 .* (G - 40.3) + c2 .* (a_k(i) - 482) + d2 .* (G - 40.3) .* (a_k(i) - 482) + e .* (G - 40.3).^2 + f .* (a_k(i) - 482).^2);\r\n" +
                                     "            else\r\n" +
                                     "                a_k(i) = a_j(i);\r\n" +
                                     "                a_j(i) = a_k(i) + (a_k(i) - a_j(i-1)) * fib(n-i+1) / fib(n-i+3);\r\n" +
                                     "                f_k(i) = f_j(i);\r\n" +
                                     "                f_j(i) = -(a2 + b2 .* (G - 40.3) + c2 .* (a_j(i) - 482) + d2 .* (G - 40.3) .* (a_j(i) - 482) + e .* (G - 40.3).^2 + f .* (a_j(i) - 482).^2);\r\n" +
                                     "            end\r\n" +
                                     "        end\r\n" +
                                     "        \r\n" +
                                     "        [~, index] = min(f_k);\r\n" +
                                     "        optimalValue = a_k(index);\r\n" +
                                     "    end" +
                                     "    \r % Расчет октанового числа при оптимальном значении параметра\r\n" +
                                     "    if strcmp(parameter, 'T')\r\n" +
                                     "       optimalOctaneNumber = abs(a3+b3*(optimalValue-40.3)+c3*(T-482)+d3*(optimalValue-40.3)*(T-482)+e1*(optimalValue-40.3)^2+f1*(T-482)^2);\r\n" +
                                     "       fprintf('\\nОктановое число: ');\r\n       " +
                                     "       disp(optimalOctaneNumber);\r\n       " +
                                     "       fprintf('Оптимальный расход сырья: ');\r\n      " +
                                     "       disp(optimalValue);\n" +
                                     "       disp(tableResult);\n" +
                                     "    elseif strcmp(parameter, 'G')\r\n" +
                                     "       optimalOctaneNumber = abs(a3+b3*(G-40.3)+c3*(optimalValue-482)+d3*(G-40.3)*(optimalValue-482)+e1*(G-40.3)^2+f1*(optimalValue-482)^2);\r\n" +
                                     "       fprintf('\\nОктановое число: ');\r\n       " +
                                     "       disp(optimalOctaneNumber);\r\n       " +
                                     "       fprintf('Оптимальная температура: ');\r\n      " +
                                     "       disp(optimalValue);\n" +
                                     "       disp(tableResult);\n" +
                                     "    end\r\n" +
                                     "    % Проверка критерия октанового числа\r\n" +
                                     "    targetOctaneNumber =" + $" {OctaineNumberBounds};" +
                                     "  % Критериальное значение октанового числа\r\n" +
                                     "    if optimalOctaneNumber < targetOctaneNumber\r\n" +
                                     "        fprintf('Не удалось получить решение, это может быть вызвано слишком большим значением критериального ограничения, попробуйте его снизить.\\n');\r\n" +
                                     "    end";

                    MatlabCode = matlabCode;
                    break;
                }
        }

    }

    private static string GetInstallationPath(string programName)
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

    private static string GetDirectoryName(string fileName)
    {
        return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
               $"/MatlabCodeFiles/{fileName}";
    }

    private static void ExecMatlab(string arguments, string scriptPath)
    {
        var startInfo = new ProcessStartInfo();
        startInfo.FileName = GetInstallationPath("MATLAB") + "/bin/Matlab.exe"; // путь к исполняемому файлу MATLAB
        startInfo.Arguments = $"-r \"run('{scriptPath}'); {arguments}\"";        // код для выполнения
        startInfo.WorkingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/MatlabCodeFiles";
        Process.Start(startInfo);
    }
}
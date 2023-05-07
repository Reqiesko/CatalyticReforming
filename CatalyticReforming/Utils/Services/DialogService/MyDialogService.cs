using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using CatalyticReforming.Utils.Services.DialogService.Interfaces;
using CatalyticReforming.Views;
using CatalyticReforming.Views.Shared;

using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Window;


namespace CatalyticReforming.Utils.Services.DialogService;

public class MyDialogService
{
    private readonly MainWindow _mainWindow;


    public MyDialogService(MainWindow mainWindow)
    {
        _mainWindow = mainWindow;
    }

    private Grid CreateDialogGrid(UIElement content)
    {
        var grid = new Grid();

        grid.RowDefinitions.Add(new RowDefinition
                                    {Height = GridLength.Auto,});

        grid.RowDefinitions.Add(new RowDefinition());
        grid.Children.Add(content);
        Grid.SetRow(content, 1);
        var titleBar = new TitleBar();

        titleBar.Tray = new NotifyIcon
        {
            FocusOnLeftClick = true,
            MenuOnRightClick = true,
        };

        grid.Children.Add(titleBar);

        return grid;
    }

    private FluentWindow CreateWindow(object content)
    {
        var window = new FluentWindow();
        window.Content = CreateDialogGrid(content as UIElement);
        window.WindowStartupLocation = WindowStartupLocation.CenterScreen;

        return window;
    }

    /// <summary>
    ///     Показать диалоговое окно
    /// </summary>
    /// <param name="data">Передаваемые данные</param>
    /// <typeparam name="T">Тип элемента для отображения в диалоговом окне</typeparam>
    /// <returns>Результат работы диалогового окна</returns>
    public Task<object?> ShowDialog<T>(object data = null) where T : FrameworkElement, IViewWithVM
    {
        var tcs = new TaskCompletionSource<object?>();
        var e = App.GetService<T>() as IViewWithVM;
        var viewModel = e.ViewModelObject;
        var window = CreateWindow(e);

        if (viewModel is IDataHolder dataHolder)
        {
            dataHolder.Data = data;
        }

        if (viewModel is IInteractionAware interactionAware)
        {
            interactionAware.FinishInteraction = () =>
            {
                Debug.WriteLine("Диалог скрылся");
                window.Close();
                window.Closed -= OnClosed;
            };
        }


        window.Closed += OnClosed;

        void OnClosed(object? o, EventArgs eventArgs)
        {
            if (viewModel is IResultHolder resultHolder)
            {
                Debug.WriteLine("Вернулся результат");
                tcs.SetResult(resultHolder.Result);
            }
        }

        window.Show();

        return tcs.Task;
    }
}



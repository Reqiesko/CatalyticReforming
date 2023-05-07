using System;

using CatalyticReforming.Views;

using Microsoft.Extensions.DependencyInjection;


namespace CatalyticReforming.Utils.Services;

public class NavigationService
{
    private readonly MainWindow _mainWindow;
    private readonly IServiceProvider _serviceProvider;

    public NavigationService(MainWindow mainWindow, IServiceProvider serviceProvider)
    {
        _mainWindow = mainWindow;
        _serviceProvider = serviceProvider;
    }

    public void ChangeContent(object content)
    {
        _mainWindow.RootContent.Content = content;
    }

    public void ChangeContent<T>()
    {
        var content = _serviceProvider.GetService<T>();
        _mainWindow.RootContent.Content = content;
    }
}


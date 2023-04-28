using System.Windows;
using CatalyticReforming.ViewModels;
using CatalyticReforming.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CatalyticReforming
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection() { };

            services.AddSingleton<NavigationService, NavigationService>();
            services.AddSingleton<MainViewModel, MainViewModel>();
            services.AddSingleton<MainWindow, MainWindow>();

            var serviceProvider = services.BuildServiceProvider();
            var mainWindow = serviceProvider.GetService<MainWindow>();

            var navigationService = serviceProvider.GetService<NavigationService>();
            navigationService.CurrentViewModel = serviceProvider.GetService<LoginPageVM>();

            mainWindow?.Show();
            base.OnStartup(e);
        }
    }
}

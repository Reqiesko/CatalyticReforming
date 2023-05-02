using System;
using System.Windows;
using CatalyticReforming.ViewModels;
using CatalyticReforming.Services;
using CatalyticReforming.Views;

using Microsoft.Extensions.DependencyInjection;

namespace CatalyticReforming
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static IServiceProvider _serviceProvider;
        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection() { };


        #region Services
            services.AddSingleton<NavigationService, NavigationService>();
        #endregion


        #region VM And Views

            services.AddSingleton<MainViewModel, MainViewModel>();
            services.AddSingleton<MainWindow, MainWindow>();
            services.AddTransient<AdminControlVM>();
            services.AddTransient<AdminControl>();
            services.AddTransient<StartControlVM>();
            services.AddTransient<StartControl>();
            services.AddTransient<LoginControlVM>();
            services.AddTransient<LoginControl>();
            services.AddTransient<StudyControlVM>();
            services.AddTransient<StartControl>();
            services.AddTransient<ResearchControlVM>();

        #endregion
            

            
            _serviceProvider = services.BuildServiceProvider();
            var mainWindow = _serviceProvider.GetService<MainWindow>();

            var navigationService = _serviceProvider.GetService<NavigationService>();
            navigationService.ChangeContent<LoginControl>(); 

            mainWindow?.Show();
        }

        public static T GetService<T>()
        {
            return _serviceProvider.GetService<T>();
        }
    }
}

using System;
using System.Windows;
using CatalyticReforming.ViewModels;
using CatalyticReforming.Services;
using CatalyticReforming.Services.DialogService;
using CatalyticReforming.ViewModels.Testing;
using CatalyticReforming.Views;
using CatalyticReforming.Views.Testing;

using DAL;

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
            services.AddSingleton<NavigationService>();
            services.AddSingleton<MyDialogService>();
            services.AddTransient<AppDbContext>();
            services.AddTransient(provider => new Func<AppDbContext>(() => provider.GetService<AppDbContext>()));
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
            services.AddTransient<ResearchControl>();
            services.AddTransient<ResearchControlVM>();
            services.AddTransient<TestBrowserControlVM>();
            services.AddTransient<TestBrowserControl>();
            services.AddTransient<EditQuestionControl>();
            services.AddTransient<EditQuestionControlVM>();
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

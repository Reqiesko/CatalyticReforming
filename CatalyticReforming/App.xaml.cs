﻿using System;
using System.Reflection;
using System.Windows;

using Autofac;
using Autofac.Extensions.DependencyInjection;

using CatalyticReforming.Services;
using CatalyticReforming.ViewModels;
using CatalyticReforming.Services.DialogService;
using CatalyticReforming.ViewModels.Testing;
using CatalyticReforming.Views;
using CatalyticReforming.Views.Testing;

using DAL;

using Microsoft.Extensions.DependencyInjection;

using Wpf.Ui.Contracts;
using Wpf.Ui.Services;

using NavigationService = CatalyticReforming.Services.NavigationService;


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
            var builder = new ContainerBuilder();
            builder.Populate(new ServiceCollection());
        #region Services

            builder.RegisterType<NavigationService>().AsSelf().SingleInstance();
            builder.RegisterType<MyDialogService>().AsSelf().SingleInstance();
            builder.RegisterType<MessageBoxService>().AsSelf().SingleInstance();
            builder.RegisterType<AppDbContext>().AsSelf();
        #endregion


        #region VM And Views

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                   .Where(t => t.Name.EndsWith("Window"))
                   .AsSelf();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                   .Where(t => t.Name.EndsWith("Control"))
                   .AsSelf();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                   .Where(t => t.Name.EndsWith("VM"))
                   .AsSelf();
        #endregion

            builder.RegisterType<MainWindow>().AsSelf().SingleInstance();


            var container = builder.Build();
            _serviceProvider = new AutofacServiceProvider(container);
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

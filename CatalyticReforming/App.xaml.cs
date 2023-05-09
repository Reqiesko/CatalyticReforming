using System;
using System.Reflection;
using System.Windows;

using Autofac;
using Autofac.Extensions.DependencyInjection;

using CatalyticReforming.Utils.Default_Dialogs;
using CatalyticReforming.Utils.Services;
using CatalyticReforming.Utils.Services.DialogService;
using CatalyticReforming.Views;
using CatalyticReforming.Views.Auth;

using DAL;

using Mapster;

using Microsoft.Extensions.DependencyInjection;


namespace CatalyticReforming;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private static IServiceProvider _serviceProvider;

    protected override void OnStartup(StartupEventArgs e)
    {
    #region Registration dependencies

        var builder = new ContainerBuilder();
        builder.Populate(new ServiceCollection());


    #region Services

        builder.RegisterType<NavigationService>().AsSelf().SingleInstance();
        builder.RegisterType<GenericRepository>().AsSelf().SingleInstance();
        builder.RegisterType<DefaultDialogs>().AsSelf().SingleInstance();
        builder.RegisterType<MyDialogService>().AsSelf().SingleInstance();
        builder.RegisterType<MessageBoxService>().AsSelf().SingleInstance();
        builder.RegisterType<UserService>().AsSelf().SingleInstance();
        builder.RegisterType<AppDbContext>().AsSelf();

    #endregion


    #region Validators

        builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
               .Where(t => t.Name.EndsWith("Validator"))
               .AsSelf();

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

    #endregion


    #region Mapping

        TypeAdapterConfig.GlobalSettings.Default.PreserveReference(true);

    #endregion


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


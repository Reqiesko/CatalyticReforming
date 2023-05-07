using System.Windows;
using System.Windows.Controls;

using CatalyticReforming.Views.Shared;


namespace CatalyticReforming.Views.Auth;

public partial class RegistrationControl : IViewWithVM<RegistrationControlVM>
{
    public RegistrationControl()
    {
        InitializeComponent();
        ViewModel = App.GetService<RegistrationControlVM>();
        DataContext = this;
    }

    public RegistrationControlVM ViewModel { get; set; }
    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext != null)
        {
            ViewModel.NewUser.Password = ((PasswordBox) sender).Password;
        }
    }
}


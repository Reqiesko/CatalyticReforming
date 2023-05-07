using System.Windows;
using System.Windows.Controls;

using CatalyticReforming.Views.Shared;


namespace CatalyticReforming.Views.Auth;

public partial class LoginControl : IViewWithVM<LoginControlVM>
{
    public LoginControl()
    {
        ViewModel = App.GetService<LoginControlVM>();
        DataContext = this;
        InitializeComponent();
    }

    public LoginControlVM ViewModel { get; set; }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext != null)
        {
            ViewModel.Password = ((PasswordBox) sender).Password;
        }
    }
}

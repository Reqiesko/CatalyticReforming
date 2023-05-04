using System.Windows;
using System.Windows.Controls;

using CatalyticReforming.ViewModels;


namespace CatalyticReforming.Views;

public partial class LoginControl : IViewWithVM<LoginControlVM>
{
    public LoginControl()
    {
        ViewModel = App.GetService<LoginControlVM>();
        DataContext = ViewModel;
        InitializeComponent();
    }

    public LoginControlVM ViewModel { get; set; }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext != null)
        {
            ((dynamic) DataContext).Password = ((PasswordBox) sender).Password;
        }
    }
}

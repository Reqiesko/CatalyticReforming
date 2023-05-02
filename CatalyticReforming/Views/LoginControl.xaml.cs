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
    
    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (this.DataContext != null)
        { ((dynamic)this.DataContext).Password = ((PasswordBox)sender).Password; }
    }

    public LoginControlVM ViewModel { get; set; }
}
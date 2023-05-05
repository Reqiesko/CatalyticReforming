using System.Windows.Controls;

using CatalyticReforming.ViewModels;


namespace CatalyticReforming.Views;

public partial class EditUserControl : IViewWithVM<UserEditControlVM>
{
    public EditUserControl()
    {
        InitializeComponent();
        ViewModel = App.GetService<UserEditControlVM>();
        DataContext = this;
    }

    public UserEditControlVM ViewModel { get; set; }
}


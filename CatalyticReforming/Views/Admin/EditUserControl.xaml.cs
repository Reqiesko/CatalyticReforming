using CatalyticReforming.Views.Shared;


namespace CatalyticReforming.Views.Admin;

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


namespace CatalyticReforming.ViewModels.DAL_VM;

public class UserVM : ViewModelBase, IDALVM
{
    public string Username { get; set; }
    public string Password { get; set; }
    public UserRoleVM Role { get; set; }
    public bool Access { get; set; }
    public int Id { get; set; }
}


namespace CatalyticReforming.ViewModels.DAL_VM;

public class UserVM : ViewModelBase, IDALVM
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public bool Access { get; set; }
}

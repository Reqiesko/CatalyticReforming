namespace CatalyticReforming.ViewModels.DAL_VM.domain;

public class InstallationVM : ViewModelBase, IDALVM
{
    public string Name { get; set; }
    public int ReactorId { get; set; }
    public virtual ReactorVM Reactor { get; set; }
    public int Id { get; set; }
}
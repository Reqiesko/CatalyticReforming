namespace CatalyticReforming.ViewModels.DAL_VM.domain;

public class InstallationVM : ValidatableViewModel<InstallationValidator>, IDALVM
{
    public string Name { get; set; }
    public int ReactorId { get; set; }
    public virtual ReactorVM Reactor { get; set; }
    public int Id { get; set; }
}
namespace CatalyticReforming.ViewModels.DAL_VM.domain;

public class MaterialVM : ValidatableViewModel<MaterialValidator>, IDALVM
{
    public string Name { get; set; }
    public double NaphthenicHydrocarbonsContent { get; set; }
    public double AromaticHydrocarbonsContent { get; set; }
    public int Id { get; set; }
}
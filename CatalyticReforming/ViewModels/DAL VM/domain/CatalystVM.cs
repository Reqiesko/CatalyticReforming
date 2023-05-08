namespace CatalyticReforming.ViewModels.DAL_VM.domain;

public class CatalystVM : ViewModelBase, IDALVM
{
    public string Name { get; set; }
    public double Density { get; set; }
    public double StrengthFactor { get; set; }
    public int Id { get; set; }
}
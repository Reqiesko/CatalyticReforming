namespace CatalyticReforming.ViewModels.DAL_VM.domain;

public class ReactorVM : ViewModelBase, IDALVM
{
    public string Name { get; set; }
    public double Pressure { get; set; }
    public int Id { get; set; }
}
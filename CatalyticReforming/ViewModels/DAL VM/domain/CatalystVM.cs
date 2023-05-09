using System;
using System.Collections;
using System.ComponentModel;


namespace CatalyticReforming.ViewModels.DAL_VM.domain;

public class CatalystVM : ValidatableViewModel<CatalystValidator>, IDALVM
{
    private CatalystValidator _catalystValidator = new CatalystValidator();
    public string Name { get; set; }
    public double Density { get; set; }
    public double StrengthFactor { get; set; }
    public int Id { get; set; }
  
}
﻿namespace DAL.Models.domain;

public class Material : EntityBase
{
    public string Name { get; set; }
    public double NaphthenicHydrocarbonsContent { get; set; }
    public double AromaticHydrocarbonsContent { get; set; }
}



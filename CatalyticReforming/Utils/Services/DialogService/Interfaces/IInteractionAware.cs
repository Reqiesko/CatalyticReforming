using System;


namespace CatalyticReforming.Utils.Services.DialogService.Interfaces;

public interface IInteractionAware
{
    Action FinishInteraction { get; set; }
}



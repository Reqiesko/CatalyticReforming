using System;


namespace CatalyticReforming.Services.DialogService;

public interface IInteractionAware
{
    Action FinishInteraction { get; set; }

}

using System;

using CatalyticReforming.Services.DialogService;


namespace CatalyticReforming.ViewModels.Testing;

public class EditQuestionControlVM : ViewModelBase, IResultHolder, IDataHolder, IInteractionAware
{
    public EditQuestionControlVM()
    {
        
    }

    public object Result { get; }
    public object Data { get; set; }
    public Action FinishInteraction { get; set; }
}

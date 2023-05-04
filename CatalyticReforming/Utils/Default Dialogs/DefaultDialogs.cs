using System.Threading.Tasks;
using System.Windows;

using CatalyticReforming.Services;
using CatalyticReforming.Services.DialogService;


namespace CatalyticReforming.Utils.Default_Dialogs;

public class DefaultDialogs
{
    private readonly MessageBoxService _messageBoxService;

    public DefaultDialogs(MessageBoxService messageBoxService)
    {
        _messageBoxService = messageBoxService;
    }
    public  async Task<MessageBoxResult> AreYouSureToDelete(string toDelete)
    {
        return await _messageBoxService.Show($"Вы действительно хотите удалить {toDelete}?",
                                      "Предупреждение",
                                      MessageBoxButton.YesNo);
    }
}

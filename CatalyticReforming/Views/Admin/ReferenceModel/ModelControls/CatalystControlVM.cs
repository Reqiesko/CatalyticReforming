using System.Collections.ObjectModel;
using System.Windows;

using CatalyticReforming.Utils.Commands;
using CatalyticReforming.Utils.Default_Dialogs;
using CatalyticReforming.Utils.Services;
using CatalyticReforming.Utils.Services.DialogService;
using CatalyticReforming.ViewModels;
using CatalyticReforming.ViewModels.DAL_VM.auth;
using CatalyticReforming.ViewModels.DAL_VM.domain;

using DAL.Models.auth;
using DAL.Models.domain;

using Mapster;


namespace CatalyticReforming.Views.Admin.ReferenceModel.ModelControls;

public class CatalystControlVM : ViewModelBase
{
    private readonly GenericRepository _repository;
    private readonly MyDialogService _dialogService;
    private readonly DefaultDialogs _defaultDialogs;
    public CatalystControlVM(GenericRepository repository, MyDialogService dialogService, DefaultDialogs defaultDialogs)
    {
        _repository = repository;
        _dialogService = dialogService;
        _defaultDialogs = defaultDialogs;
        Catalysts = new ObservableCollection<CatalystVM>(repository.GetAll<CatalystVM, Catalyst>().Result);
    }
    public ObservableCollection<CatalystVM> Catalysts { get; set; }

    private RelayCommand _addCatalyst;

    public RelayCommand AddCatalyst
    {
        get
        {
            return _addCatalyst ??= new RelayCommand(async _ =>
            {
                var res = await _dialogService.ShowDialog<EditCatalystControl>(new CatalystVM()) as CatalystVM;

                if (res is null)
                {
                    return;
                }

                var entity = await _repository.Create<CatalystVM, Catalyst>(res);
                res.Id = entity.Id;
                Catalysts.Add(res);
            });
        }
    }

    private RelayCommand _editCatalyst;

    public RelayCommand EditCatalyst
    {
        get
        {
            return _editCatalyst ??= new RelayCommand(async catalystVm =>
            {
                var res = await _dialogService.ShowDialog<EditCatalystControl>(catalystVm.Adapt<CatalystVM>()) as CatalystVM;

                if (res is null)
                {
                    return;
                }

                await _repository.Update<CatalystVM, Catalyst>(res);
                res.Adapt((CatalystVM) catalystVm);
            });
        }
    }

    private RelayCommand _deleteCatalyst;
    
    public RelayCommand DeleteCatalyst
    {
        get
        {
            return _deleteCatalyst ??= new RelayCommand(async catalystVm =>
            {
                var mbRes = await _defaultDialogs.AreYouSureToDelete("выбранный катализатор");

                if (mbRes != MessageBoxResult.Yes)
                {
                    return;
                }

                await _repository.Delete<CatalystVM, Catalyst>((CatalystVM) catalystVm);
                Catalysts.Remove((CatalystVM) catalystVm);
            });
        }
    }

}

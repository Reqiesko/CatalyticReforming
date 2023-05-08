using System.Collections.ObjectModel;
using System.Windows;

using CatalyticReforming.Utils.Commands;
using CatalyticReforming.Utils.Default_Dialogs;
using CatalyticReforming.Utils.Services;
using CatalyticReforming.Utils.Services.DialogService;
using CatalyticReforming.ViewModels;
using CatalyticReforming.ViewModels.DAL_VM.domain;

using DAL.Models.domain;

using Mapster;


namespace CatalyticReforming.Views.Admin.ReferenceModel.ModelControls;

public class MaterialControlVM : ViewModelBase
{
    private readonly GenericRepository _repository;
    private readonly DefaultDialogs _defaultDialogs;
    private readonly MyDialogService _dialogService;

    public MaterialControlVM(GenericRepository repository, DefaultDialogs defaultDialogs, MyDialogService dialogService)
    {
        _repository = repository;
        _defaultDialogs = defaultDialogs;
        _dialogService = dialogService;
        Materials = new ObservableCollection<MaterialVM>(_repository.GetAll<MaterialVM, Material>().Result);
    }

    public ObservableCollection<MaterialVM> Materials { get; set; }
    
    private RelayCommand _addMaterial;

    public RelayCommand AddMaterial
    {
        get
        {
            return _addMaterial ??= new RelayCommand(async _ =>
            {
                var res = await _dialogService.ShowDialog<EditMaterialControl>(new MaterialVM()) as MaterialVM;

                if (res is null)
                {
                    return;
                }

                var entity = await _repository.Create<MaterialVM, Material>(res);
                res.Id = entity.Id;
                Materials.Add(res);
            });
        }
    }

    private RelayCommand _editMaterial;

    public RelayCommand EditMaterial
    {
        get
        {
            return _editMaterial ??= new RelayCommand(async materialVm =>
            {
                var res = await _dialogService.ShowDialog<EditMaterialControl>(materialVm.Adapt<MaterialVM>()) as MaterialVM;

                if (res is null)
                {
                    return;
                }

                await _repository.Update<MaterialVM, Material>(res);
                res.Adapt((MaterialVM) materialVm);
            });
        }
    }

    private RelayCommand _deleteMaterial;
    
    public RelayCommand DeleteMaterial
    {
        get
        {
            return _deleteMaterial ??= new RelayCommand(async materialVm =>
            {
                var mbRes = await _defaultDialogs.AreYouSureToDelete("выбранный материал");

                if (mbRes != MessageBoxResult.Yes)
                {
                    return;
                }

                await _repository.Delete<MaterialVM, Material>((MaterialVM) materialVm);
                Materials.Remove((MaterialVM) materialVm);
            });
        }
    }

}
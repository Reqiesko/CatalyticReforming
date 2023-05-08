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

public class ReactorControlVM : ViewModelBase
{
    private readonly GenericRepository _repository;
    private readonly DefaultDialogs _defaultDialogs;
    private readonly MyDialogService _dialogService;

    public ReactorControlVM(GenericRepository repository, DefaultDialogs defaultDialogs, MyDialogService dialogService)
    {
        _repository = repository;
        _defaultDialogs = defaultDialogs;
        _dialogService = dialogService;
        Reactors = new ObservableCollection<ReactorVM>(_repository.GetAll<ReactorVM, Reactor>().Result);
    }

    public ObservableCollection<ReactorVM> Reactors { get; set; }
    private RelayCommand _addReactor;

    public RelayCommand AddReactor
    {
        get
        {
            return _addReactor ??= new RelayCommand(async _ =>
            {
                var res = await _dialogService.ShowDialog<EditReactorControl>(new ReactorVM()) as ReactorVM;

                if (res is null)
                {
                    return;
                }

                var entity = await _repository.Create<ReactorVM, Reactor>(res);
                res.Id = entity.Id;
                Reactors.Add(res);
            });
        }
    }

    private RelayCommand _editReactor;

    public RelayCommand EditReactor
    {
        get
        {
            return _editReactor ??= new RelayCommand(async reactorVm =>
            {
                var res = await _dialogService.ShowDialog<EditReactorControl>(reactorVm.Adapt<ReactorVM>()) as ReactorVM;

                if (res is null)
                {
                    return;
                }

                await _repository.Update<ReactorVM, Reactor>(res);
                res.Adapt((ReactorVM) reactorVm);
            });
        }
    }

    private RelayCommand _deleteMaterial;
    
    public RelayCommand DeleteReactor
    {
        get
        {
            return _deleteMaterial ??= new RelayCommand(async reactorVm =>
            {
                var mbRes = await _defaultDialogs.AreYouSureToDelete("выбранный реактор");

                if (mbRes != MessageBoxResult.Yes)
                {
                    return;
                }

                await _repository.Delete<ReactorVM, Reactor>((ReactorVM) reactorVm);
                Reactors.Remove((ReactorVM) reactorVm);
            });
        }
    }
}
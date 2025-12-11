using System.Collections.ObjectModel;
using System.Windows.Input;
using TopoCentras.Core.Interfaces;
using TopoCentras.Core.Models;
using TopoCentras.Wpf.Commands;

namespace TopoCentras.Wpf.ViewModels;

public class UzsakymaiViewModel : BaseViewModel
{
    private readonly IUzsakymasService _uzsakymasService;


    public UzsakymaiViewModel(IUzsakymasService uzsakymasService, CreateUzsakymasViewModel createUzsakymasViewModel)
    {
        _uzsakymasService = uzsakymasService;
        CreateVM = createUzsakymasViewModel;


        LoadUzsakymaiCommand = new RelayCommand(async _ => await LoadUzsakymaiAsync());


        DeleteUzsakymasCommand = new RelayCommand(async param => await DeleteUzsakymasAsync(param as Uzsakymas),
            param => param is Uzsakymas);
    }


    public ObservableCollection<Uzsakymas> Uzsakymai { get; } = new();

    public CreateUzsakymasViewModel CreateVM { get; }


    public ICommand LoadUzsakymaiCommand { get; }
    public ICommand DeleteUzsakymasCommand { get; }

    private async Task DeleteUzsakymasAsync(Uzsakymas? uzsakymas)
    {
        if (uzsakymas == null) return;
        await _uzsakymasService.DeleteUzsakymasAsync(uzsakymas.UzsakymasId);
        Uzsakymai.Remove(uzsakymas);
    }


    public async Task LoadUzsakymaiAsync()
    {
        Uzsakymai.Clear();
        var list = await _uzsakymasService.GetAllUzsakymasAsync();
        foreach (var u in list) Uzsakymai.Add(u);
    }
}
using System.Collections.ObjectModel;
using System.Windows.Input;
using TopoCentras.Core.Interfaces;
using TopoCentras.Core.Models;
using TopoCentras.Wpf.Commands;

namespace TopoCentras.Wpf.ViewModels;

public class KlientaiViewModel : BaseViewModel
{
    private readonly IKlientasService _klientasService;
    private readonly IUzsakymasService _uzsakymasService;

    private string _pavadinimas = string.Empty;

    public KlientaiViewModel(IKlientasService klientasService, IUzsakymasService uzsakymasService)
    {
        _klientasService = klientasService;
        _uzsakymasService = uzsakymasService;
        LoadKlientaiCommand = new RelayCommand(async _ => await LoadKlientaiAsync());
        CreateKlientasCommand = new RelayCommand(async _ => await CreateKlientasAsync(), _ => CanCreate);
        DeleteKlientasCommand = new RelayCommand(async param => await DeleteKlientasAsync(param as Klientas),
            param => param is Klientas);

        _ = LoadKlientaiAsync();
    }

    public ObservableCollection<Klientas> Klientai { get; set; } = new();

    public string Pavadinimas
    {
        get => _pavadinimas;
        set
        {
            if (_pavadinimas != value)
            {
                _pavadinimas = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanCreate));
            }
        }
    }

    public bool CanCreate => !string.IsNullOrWhiteSpace(Pavadinimas);


    public ICommand LoadKlientaiCommand { get; }

    public ICommand CreateKlientasCommand { get; }
    public ICommand DeleteKlientasCommand { get; }

    public async Task UpdateKlientasAsync(Klientas klientas)
    {
        await _klientasService.UpdateKlientasAsync(klientas);
    }

    public async Task DeleteKlientasAsync(Klientas? klientas)
    {
        if (klientas == null) return;
        await _klientasService.DeleteKlientasAsync(klientas.KlientasId);
        Klientai.Remove(klientas);
    }

    public async Task LoadKlientaiAsync()
    {
        Klientai.Clear();
        var usedIds = await _uzsakymasService.GetKlientasIdsWithUzsakymaiAsync();
        var list = await _klientasService.GetAllKlientasAsync();
        foreach (var k in list)
        {
            k.CanDelete = !usedIds.Contains(k.KlientasId);
            Klientai.Add(k);
        }
    }

    public async Task CreateKlientasAsync()
    {
        await _klientasService.CreateKlientasAsync(Pavadinimas);

        Pavadinimas = string.Empty;

        await LoadKlientaiAsync();
    }
}
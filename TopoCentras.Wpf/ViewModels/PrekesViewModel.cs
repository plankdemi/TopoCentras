using System.Collections.ObjectModel;
using System.Windows.Input;
using TopoCentras.Core.Interfaces;
using TopoCentras.Core.Models;
using TopoCentras.Wpf.Commands;

namespace TopoCentras.Wpf.ViewModels;

public class PrekesViewModel : BaseViewModel
{
    private readonly IPrekeService _prekeService;
    private readonly IUzsakymasService _uzsakymasService;

    private string _gamintojas = string.Empty;

    public string _kainaText = string.Empty;

    private string _pavadinimas = string.Empty;

    public PrekesViewModel(IPrekeService prekeService, IUzsakymasService uzsakymasService)
    {
        _prekeService = prekeService;
        _uzsakymasService = uzsakymasService;
        LoadPrekesCommand = new RelayCommand(async _ => await LoadPrekesAsync());
        CreatePrekeCommand = new RelayCommand(async _ => await CreatePrekeAsync(), _ => CanCreate);
        DeletePrekeCommand =
            new RelayCommand(async param => await DeletePrekeAsync(param as Preke), param => param is Preke);

        _ = LoadPrekesAsync();
    }

    public ObservableCollection<Preke> Prekes { get; set; } = new();

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

    public string Gamintojas
    {
        get => _gamintojas;
        set
        {
            if (_gamintojas != value)
            {
                _gamintojas = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanCreate));
            }
        }
    }

    public string KainaText
    {
        get => _kainaText;
        set
        {
            if (_kainaText != value)
            {
                _kainaText = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanCreate));
            }
        }
    }

    public bool CanCreate =>
        !string.IsNullOrWhiteSpace(Pavadinimas) &&
        !string.IsNullOrWhiteSpace(Gamintojas) &&
        decimal.TryParse(KainaText, out var k) && k >= 0;

    public ICommand LoadPrekesCommand { get; }
    public ICommand CreatePrekeCommand { get; }
    public ICommand DeletePrekeCommand { get; }

    public async Task DeletePrekeAsync(Preke? preke)
    {
        if (preke == null) return;
        await _prekeService.DeletePrekeAsync(preke.PrekeId);
        Prekes.Remove(preke);
    }

    public async Task UpdatePrekeAsync(Preke preke)
    {
        await _prekeService.UpdatePrekeAsync(preke);
    }

    public async Task LoadPrekesAsync()
    {
        Prekes.Clear();
        var usedIds = await _uzsakymasService.GetPrekeIdsWithUzsakymaiAsync();
        var list = await _prekeService.GetAllPrekeAsync();
        foreach (var p in list)
        {
            p.CanDelete = !usedIds.Contains(p.PrekeId);
            Prekes.Add(p);
        }
    }

    public async Task CreatePrekeAsync()
    {
        if (!decimal.TryParse(KainaText, out var kaina) || kaina < 0) return;

        await _prekeService.CreatePrekeAsync(Pavadinimas, Gamintojas, kaina);

        Pavadinimas = string.Empty;
        Gamintojas = string.Empty;
        KainaText = string.Empty;

        await LoadPrekesAsync();
    }
}
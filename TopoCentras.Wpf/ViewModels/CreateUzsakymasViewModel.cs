using System.Collections.ObjectModel;
using System.Windows.Input;
using TopoCentras.Core.Interfaces;
using TopoCentras.Core.Models;
using TopoCentras.Wpf.Commands;

namespace TopoCentras.Wpf.ViewModels;

public class CreateUzsakymasViewModel : BaseViewModel
{
    private readonly IKlientasService _klientasService;
    private readonly IPrekeService _prekeService;
    private readonly IUzsakymasService _uzsakymasService;

    private Uzsakymas? _editingUzsakymas;

    private Klientas? _selectedKlientas;


    public CreateUzsakymasViewModel(
        IKlientasService klientasService,
        IPrekeService prekeService,
        IUzsakymasService uzsakymasService)
    {
        _klientasService = klientasService;
        _prekeService = prekeService;
        _uzsakymasService = uzsakymasService;

        LoadKlientaiCommand = new RelayCommand(async _ => await LoadKlientaiAsync());
        LoadPrekesCommand = new RelayCommand(async _ => await LoadPrekesAsync());
        CreateUzsakymasCommand = new RelayCommand(async _ => await SaveUzsakymasAsync());
    }

    public ObservableCollection<PrekeSelection> Prekes { get; } = new();
    public ObservableCollection<Klientas> Klientai { get; } = new();

    public Klientas? SelectedKlientas
    {
        get => _selectedKlientas;
        set
        {
            if (_selectedKlientas == value) return;
            _selectedKlientas = value;
            OnPropertyChanged();
        }
    }

    public bool IsEditMode => _editingUzsakymas != null;

    public ICommand LoadKlientaiCommand { get; }
    public ICommand LoadPrekesCommand { get; }
    public ICommand CreateUzsakymasCommand { get; }

    public async Task LoadKlientaiAsync()
    {
        Klientai.Clear();
        var list = await _klientasService.GetAllKlientasAsync();
        foreach (var k in list) Klientai.Add(k);
    }

    public async Task LoadPrekesAsync()
    {
        Prekes.Clear();
        var list = await _prekeService.GetAllPrekeAsync();
        foreach (var p in list) Prekes.Add(new PrekeSelection(p));
    }

    public async Task InitForCreateAsync()
    {
        _editingUzsakymas = null;
        await LoadKlientaiAsync();
        await LoadPrekesAsync();

        SelectedKlientas = null;
        foreach (var p in Prekes)
        {
            p.IsSelected = false;
            p.Kiekis = 1;
        }
    }

    public async Task InitForEditAsync(Uzsakymas uzsakymas)
    {
        _editingUzsakymas = uzsakymas;
        await LoadKlientaiAsync();
        await LoadPrekesAsync();

        SelectedKlientas = Klientai.FirstOrDefault(k => k.KlientasId == uzsakymas.KlientasId);

        foreach (var ps in Prekes)
        {
            var existing = uzsakymas.UzsakymasPrekes.FirstOrDefault(up => up.PrekeId == ps.Preke.PrekeId);
            if (existing != null)
            {
                ps.IsSelected = true;
                ps.Kiekis = existing.Kiekis;
            }
            else
            {
                ps.IsSelected = false;
                ps.Kiekis = 1;
            }
        }
    }

    public async Task SaveUzsakymasAsync()
    {
        if (SelectedKlientas is null) return;
        var selected = Prekes.Where(ps => ps.IsSelected && ps.Kiekis > 0).ToList();
        if (!selected.Any()) return;

        var mapping = selected.ToDictionary(
            ps => ps.Preke.PrekeId,
            ps => ps.Kiekis);
        if (_editingUzsakymas == null)
            await _uzsakymasService.CreateUzsakymasAsync(
                SelectedKlientas.KlientasId,
                mapping);
        else
            await _uzsakymasService.UpdateUzsakymasAsync(
                _editingUzsakymas.UzsakymasId,
                SelectedKlientas.KlientasId,
                mapping);
        SelectedKlientas = null;
        foreach (var ps in Prekes)
        {
            ps.IsSelected = false;
            ps.Kiekis = 1;
        }
    }
}
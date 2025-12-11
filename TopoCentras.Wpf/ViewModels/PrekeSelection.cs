using TopoCentras.Core.Models;

namespace TopoCentras.Wpf.ViewModels;

public class PrekeSelection : BaseViewModel
{
    private bool _isSelected;

    private int _Kiekis = 1;

    public PrekeSelection(Preke preke)
    {
        Preke = preke;
    }

    public Preke Preke { get; }

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected == value) return;
            _isSelected = value;
            OnPropertyChanged();
        }
    }

    public int Kiekis
    {
        get => _Kiekis;
        set
        {
            if (_Kiekis == value) return;
            _Kiekis = value;
            OnPropertyChanged();
        }
    }
}
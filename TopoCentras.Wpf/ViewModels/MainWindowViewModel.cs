namespace TopoCentras.Wpf.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
    public MainWindowViewModel(
        UzsakymaiViewModel uzsakymaiViewModel,
        PrekesViewModel prekesViewModel,
        KlientaiViewModel klientaiViewModel
    )
    {
        UzsakymaiVM = uzsakymaiViewModel;
        PrekesVM = prekesViewModel;
        KlientaiVM = klientaiViewModel;
    }

    public UzsakymaiViewModel UzsakymaiVM { get; }
    public PrekesViewModel PrekesVM { get; }
    public KlientaiViewModel KlientaiVM { get; }
}
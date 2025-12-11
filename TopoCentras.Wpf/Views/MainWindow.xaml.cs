using System.Windows;
using System.Windows.Controls;
using TopoCentras.Wpf.ViewModels;

namespace TopoCentras.Wpf.Views;

public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    private async void OnTabSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.Source is not TabControl) return;
        if (DataContext is not MainWindowViewModel vm) return;

        var tabControl = (TabControl)sender;
        var tabItem = tabControl.SelectedItem as TabItem;
        var header = tabItem?.Header as string;

        switch (header)
        {
            case "Klientai":
                await vm.KlientaiVM.LoadKlientaiAsync();
                break;

            case "Prekes":
                await vm.PrekesVM.LoadPrekesAsync();
                break;

            case "Užsakymai":
                await vm.UzsakymaiVM.LoadUzsakymaiAsync();
                break;
        }
    }
}
using System.Windows;
using System.Windows.Controls;
using TopoCentras.Wpf.ViewModels;

namespace TopoCentras.Wpf.Views;

public partial class CreateUzsakymasView : UserControl
{
    public CreateUzsakymasView()
    {
        InitializeComponent();
    }

    public event RoutedEventHandler? OrderCreated;
    public event RoutedEventHandler? OrderCancelled;

    private async void OnCreateUzsakymasClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is CreateUzsakymasViewModel vm)
        {
            await vm.SaveUzsakymasAsync();
            OrderCreated?.Invoke(this, new RoutedEventArgs());
        }
    }

    private void OnCancelUzsakymasClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is CreateUzsakymasViewModel) OrderCancelled?.Invoke(this, new RoutedEventArgs());
    }
}
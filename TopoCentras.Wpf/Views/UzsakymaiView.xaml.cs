using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TopoCentras.Core.Models;
using TopoCentras.Wpf.ViewModels;

namespace TopoCentras.Wpf.Views;

public partial class UzsakymaiView : UserControl
{
    public UzsakymaiView()
    {
        InitializeComponent();
    }

    private async void OnCreateUzsakymasClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is UzsakymaiViewModel vm)
        {
            await vm.CreateVM.InitForCreateAsync();
            CreateUzsakymasView.Visibility = Visibility.Visible;
        }
    }

    private async void OnUzsakymaiGridDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (UzsakymaiGrid.SelectedItem is not Uzsakymas uzsakymas)
            return;

        if (DataContext is not UzsakymaiViewModel vm)
            return;

        await vm.CreateVM.InitForEditAsync(uzsakymas);
        CreateUzsakymasView.Visibility = Visibility.Visible;
    }


    private async void CreateUzsakymasView_OnOrderCreated(object sender, RoutedEventArgs e)
    {
        if (DataContext is UzsakymaiViewModel vm) await vm.LoadUzsakymaiAsync();
        CreateUzsakymasView.Visibility = Visibility.Collapsed;
    }

    private void CreateUzsakymasView_OnOrderCancelled(object sender, RoutedEventArgs e)
    {
        CreateUzsakymasView.Visibility = Visibility.Collapsed;
    }
}
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using TopoCentras.Core.Models;
using TopoCentras.Wpf.ViewModels;

namespace TopoCentras.Wpf.Views;

public partial class KlientaiView : UserControl
{
    public KlientaiView()
    {
        InitializeComponent();
    }
    
    private void KlientaiGrid_OnRowEditEnding(object? sender, DataGridRowEditEndingEventArgs e)
    {
        if (e.EditAction != DataGridEditAction.Commit)
            return;

        if (DataContext is not KlientaiViewModel vm)
            return;

        if (e.Row.Item is not Klientas klientas)
            return;
        
        Dispatcher.BeginInvoke(new Action(async () =>
        {
            await vm.UpdateKlientasAsync(klientas);
        }), DispatcherPriority.Background);
    }
}
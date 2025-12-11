using System.Windows.Controls;
using System.Windows.Threading;
using TopoCentras.Core.Models;
using TopoCentras.Wpf.ViewModels;

namespace TopoCentras.Wpf.Views;

public partial class PrekesView : UserControl
{
    public PrekesView()
    {
        InitializeComponent();
    }
    
    private void PrekesGrid_OnRowEditEnding(object? sender, DataGridRowEditEndingEventArgs e)
    {
        if (e.EditAction != DataGridEditAction.Commit)
            return;

        if (DataContext is not PrekesViewModel vm)
            return;

        if (e.Row.Item is not Preke preke)
            return;
        
        Dispatcher.BeginInvoke(new Action(async () =>
        {
            await vm.UpdatePrekeAsync(preke);
        }), DispatcherPriority.Background);
    }
}
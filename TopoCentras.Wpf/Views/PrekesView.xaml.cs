using System.Windows.Controls;
using System.Windows.Input;
using TopoCentras.Core.Models;
using TopoCentras.Wpf.ViewModels;

namespace TopoCentras.Wpf.Views;

public partial class PrekesView : UserControl
{
    public PrekesView()
    {
        InitializeComponent();
    }

    private async void PrekesGrid_OnKeyUp(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter)
            return;

        PrekesGrid.CommitEdit(DataGridEditingUnit.Cell, true);
        PrekesGrid.CommitEdit(DataGridEditingUnit.Row, true);

        if (DataContext is not PrekesViewModel vm)
            return;

        if (PrekesGrid.CurrentItem is not Preke preke)
            return;


        await vm.UpdatePrekeAsync(preke);
    }
}
using System.Windows.Controls;
using System.Windows.Input;
using TopoCentras.Core.Models;
using TopoCentras.Wpf.ViewModels;

namespace TopoCentras.Wpf.Views;

public partial class KlientaiView : UserControl
{
    public KlientaiView()
    {
        InitializeComponent();
    }


    private async void KlientaiGrid_OnKeyUp(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter)
            return;

        KlientaiGrid.CommitEdit(DataGridEditingUnit.Cell, true);
        KlientaiGrid.CommitEdit(DataGridEditingUnit.Row, true);

        if (DataContext is not KlientaiViewModel vm)
            return;

        if (KlientaiGrid.CurrentItem is not Klientas klientas)
            return;


        await vm.UpdateKlientasAsync(klientas);
    }
}
using System;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Mp3Player.ViewModels;
using ReactiveUI;

namespace Mp3Player.Views;

public partial class MusicsDataGridView : UserControl
{
    
    public MusicsDataGridView( )
    {
        InitializeComponent();
        //DataContext = new MusicsDataGridViewModel();

    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        //((MusicsDataGridViewModel)this.DataContext).MainWindowViewModel = MainWindowViewModel.Main;
    }

    private void DataGrid_OnDoubleTapped(object? sender, RoutedEventArgs e)
    {
        var datagrid = sender as DataGrid;

        if (e.Source.GetType() == typeof(Border))
        {
            datagrid.SelectedItem = null;
            datagrid.SelectedIndex = -1;
            return;
        };
        ((MusicsDataGridViewModel) this.DataContext).DoubleClickMusic();
    }

    
    private void Control_OnContextRequested(object? sender, ContextRequestedEventArgs e)
    {
        ((MusicsDataGridViewModel) this.DataContext).ShowContextMenu();
        //e.Handled = true;
    }

    private void DataGrid_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        Console.WriteLine(e);  
    }
    private void DataGrid_OnLoadingRow(object? sender, DataGridRowEventArgs e)
    {
        //_musics.Add(e.Row);
    }

    private void DataGrid_OnTapped(object? sender, RoutedEventArgs e)
    {
        if (e.Source.GetType() == typeof(Border))
        {
            var datagrid = sender as DataGrid;
            datagrid.SelectedIndex = -1;
            datagrid.SelectedItem = null;
            return;;
        }
    }
}
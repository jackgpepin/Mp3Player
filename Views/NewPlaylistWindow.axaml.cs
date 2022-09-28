using System;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Mp3Player.ViewModels;
using ReactiveUI;

namespace Mp3Player.Views;

public partial class NewPlaylistWindow : ReactiveWindow<NewPlaylistWindowViewModel>
{
    public NewPlaylistWindow()
    {
        InitializeComponent();
        this.WhenActivated(d => d(ViewModel!.CreateCommand.Subscribe(Close)));
        this.WhenActivated(d => d(ViewModel!.ChooseFilesCommand.RegisterHandler(DoShowChooseFiles)));
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void Cancel_OnClick(object? sender, RoutedEventArgs e)
    {   
        Close();
    }

    private async void ChooseFiles(object? sender, RoutedEventArgs e)
    {
    }

    public async Task DoShowChooseFiles(InteractionContext<Unit, string[]> context)
    {   
        var dialog = new OpenFileDialog();
        dialog.AllowMultiple = true;
        var files = await dialog.ShowAsync(this);
        context.SetOutput(files);
    }
}
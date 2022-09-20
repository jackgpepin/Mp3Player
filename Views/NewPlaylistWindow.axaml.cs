using System;
using Avalonia;
using Avalonia.Controls;
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
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
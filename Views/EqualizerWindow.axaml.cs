using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Mp3Player.ViewModels;
using ReactiveUI;

namespace Mp3Player.Views;

public partial class EqualizerWindow : ReactiveWindow<EqualizerWindowViewModel>
{
    public EqualizerWindow()
    {
        InitializeComponent();
        this.FindControl<Canvas>("Canvas").PointerPressed += (sender, args) =>
        {
            PlatformImpl.BeginMoveDrag(args);
        };
       
            
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        
    }

    private void CancelButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}
using System;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Avalonia.ReactiveUI;
using Mp3Player.ViewModels;
using ReactiveUI;

namespace Mp3Player.Views;

public partial class NewPlaylistWindow : ReactiveWindow<NewPlaylistWindowViewModel>
{
    private bool m_Done = false;

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
        var iv = this.GetObservable(Window.IsVisibleProperty);
        iv.Subscribe(value =>
        {
            if (value && !m_Done) {
                m_Done = true;
                CenterWindow();
            }
        });
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
    private async void CenterWindow()
    {
        if (this.WindowStartupLocation == WindowStartupLocation.Manual)
            return;

        Screen screen = null;
        while (screen == null) {
            await Task.Delay(1);
            screen = this.Screens.ScreenFromVisual(this);
        }

        if (this.WindowStartupLocation == WindowStartupLocation.CenterScreen) {
            var x = (int)Math.Floor(screen.Bounds.Width / 2 - this.Bounds.Width / 2);
            var y = (int)Math.Floor(screen.Bounds.Height / 2 - (this.Bounds.Height + 30) / 2);

            this.Position = new PixelPoint(x, y);
        } else if (this.WindowStartupLocation == WindowStartupLocation.CenterOwner) {
            var pw = this.Owner as Window;
            if (pw != null) {
                var x = (int)Math.Floor(pw.Bounds.Width / 2 - this.Bounds.Width / 2 + pw.Position.X);
                var y = (int)Math.Floor(pw.Bounds.Height / 2 - (this.Bounds.Height + 30) / 2 + pw.Position.Y);

                this.Position = new PixelPoint(x, y);
            }
        }
    }
}
using System;
using System.Reactive;
using Mp3Player.Models;
using ReactiveUI;

namespace Mp3Player.ViewModels;

public class NewPlaylistWindowViewModel : ViewModelBase
{
    private string _playlistName = string.Empty;
    
    public string PlaylistName
    {
        get => _playlistName;
        set => this.RaiseAndSetIfChanged(ref _playlistName, value);
    }
    public NewPlaylistWindowViewModel()
    {
        CreateCommand = ReactiveCommand.Create(() =>
        {
            var playlist = new Playlist(PlaylistName);
            var playlistViewModel = new PlaylistViewModel(playlist);
            return playlistViewModel;
        });
    }
    public ReactiveCommand<Unit, PlaylistViewModel> CreateCommand { get; set; }
}
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
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

    private ObservableCollection<string> _playlistFiles;
    public ObservableCollection<string> PlaylistFiles
    {
        get => _playlistFiles;
        set => this.RaiseAndSetIfChanged(ref _playlistFiles, value);
    }
    public NewPlaylistWindowViewModel()
    {
        PlaylistFiles = new ObservableCollection<string>();
        CreateCommand = ReactiveCommand.Create(() =>
        {
            var playlist = new Playlist(PlaylistName);
            foreach (var file in PlaylistFiles)
            {
                playlist.PlaylistFiles.Add(new PlaylistFile(file));
            }
            playlist.Create();
            var playlistViewModel = new PlaylistViewModel(playlist);
            
            return playlistViewModel;
        });
        ChooseFilesCommand = new Interaction<Unit, string[]>();
        AddFilesCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            string[] files = await ChooseFilesCommand.Handle(Unit.Default);
            if(files == null) return;
            foreach (var file in files)
            {
             PlaylistFiles.Add(file);   
            }
        });
    }
    public ReactiveCommand<Unit, PlaylistViewModel> CreateCommand { get; set; }
    public ReactiveCommand<Unit, Unit> AddFilesCommand { get; set; }
    public Interaction<Unit, string[]> ChooseFilesCommand { get; set; }
}
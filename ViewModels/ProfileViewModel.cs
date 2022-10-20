using System.Collections.Generic;
using System.Collections.ObjectModel;
using Mp3Player.Models;
using ReactiveUI;

namespace Mp3Player.ViewModels;

public class ProfileViewModel : ViewModelBase
{
    private string _name;

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    private string _id;

    public string Id
    {
        get => _id;
        set => this.RaiseAndSetIfChanged(ref _id, value);
    }

    private ObservableCollection<PlaylistViewModel> _playlists;

    public ObservableCollection<PlaylistViewModel> Playlists
    {
        get => _playlists;
        set => this.RaiseAndSetIfChanged(ref _playlists, value);
    }

    private Profile _profile;
    
    public ProfileViewModel(Profile profile)
    {
        _profile = profile;
        Name = profile.Name;
        Id = profile.Id;
        Playlists = new ObservableCollection<PlaylistViewModel>();
        foreach (var playlist in profile.Playlists)
        {
            Playlists.Add(new PlaylistViewModel(playlist));
        }
    }

    public void Update()
    {
        if (Id == "0000") return;
        _profile.Name = Name;
        _profile.Playlists = new List<Playlist>();
        foreach (var playlist in Playlists)
        {
            playlist._playlist.PlaylistFiles = new List<PlaylistFile>();
            foreach (var music in playlist.Musics)
            {
                playlist._playlist.PlaylistFiles.Add(music.PlaylistFile);
            }
            _profile.Playlists.Add(playlist._playlist);
        }
        
        _profile.Update();
    }
    public void Delete()
    {
        _profile.Delete();
    }
}
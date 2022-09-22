using System.Collections.Generic;
using System.Collections.ObjectModel;
using Mp3Player.Models;
using ReactiveUI;

namespace Mp3Player.ViewModels;

public class PlaylistViewModel : ViewModelBase
{
    private Playlist _playlist;
    private string _name;
    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    private ViewModelBase _content;

    public ViewModelBase Content
    {
        get => _content;
        set
        {
            this.RaiseAndSetIfChanged(ref _content, value);
            //((MusicsDataGridViewModel) Content).Playlist = this;

        }
    }
    private ObservableCollection<MusicViewModel> _musics;

    public ObservableCollection<MusicViewModel> Musics
    {
        get => _musics;
        set
        {
            this.RaiseAndSetIfChanged(ref _musics, value);
            
        }
    }
    
    public PlaylistViewModel(Playlist playlist)
    {
        _playlist = playlist;
        Musics = new ObservableCollection<MusicViewModel>();
        Name = playlist.Name;
    }

    public void Create()
    {
        _playlist.Create();
    }

    public void Save()
    {
        _playlist.PlaylistFiles = new List<PlaylistFile>();
        foreach (var music in Musics)
        {
            _playlist.PlaylistFiles.Add(music.PlaylistFile);
        }
        _playlist.Save();
    }
    public void Delete()
    {
        _playlist.Delete();
    }

    public void RemoveFile(PlaylistFile file)
    {
        _playlist.RemoveFile(file);
    }
    public void Rename()
    {
        _playlist.Rename();
    }
    
}
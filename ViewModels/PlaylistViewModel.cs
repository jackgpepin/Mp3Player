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
        set => this.RaiseAndSetIfChanged(ref _content, value);
    }
    
    public PlaylistViewModel(Playlist playlist)
    {
        _playlist = playlist;
        Name = playlist.Name;
    }

    public void Create()
    {
        _playlist.Create();
    }

    public void Save()
    {
        _playlist.Save();
    }
    public void Delete()
    {
        _playlist.Delete();
    }

    public void Rename()
    {
        _playlist.Rename();
    }
    
}
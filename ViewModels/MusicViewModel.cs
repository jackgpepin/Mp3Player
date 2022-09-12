using System;
using System.Linq;
using LibVLCSharp.Shared;
using ReactiveUI;

namespace Mp3Player.ViewModels;

public class MusicViewModel:ViewModelBase
{
    private string _title;

    public string Title
    {
        get => _title;
        set => this.RaiseAndSetIfChanged(ref _title, value);
    }

    private Timer _duration;
    public Timer Duration
    {
        get => _duration;
        set => this.RaiseAndSetIfChanged(ref _duration, value);
    }

    private Uri _path;

    public Uri Path
    {
        get => _path;
        set => this.RaiseAndSetIfChanged(ref _path, value);
    }

    private MediaPlayer _mPlayer;

    public MediaPlayer MPlayer
    {
        get => _mPlayer;
        set => this.RaiseAndSetIfChanged(ref _mPlayer, value);
    }

    private bool _isNowPlaying;
    public bool IsNowPlaying
    {
        get => _isNowPlaying;
        set => this.RaiseAndSetIfChanged(ref _isNowPlaying, value);
    }

    private Media _media;
    public MusicViewModel(string path, LibVLC libVlc)
    {
        IsNowPlaying = false;
        Title = path.Split("/").Last();
        //Duration = duration;
        Path = new Uri(path);
        _media = new Media(libVlc, Path);
        _media.Parse(MediaParseOptions.ParseLocal);
        
        MPlayer = new MediaPlayer(_media);
       
        //MPlayer.Play(media);
        _media.MetaChanged += (sender, args) =>
        {
            Title = _media.Meta(MetadataType.Title);
        };
        MPlayer.LengthChanged += (sender, args) =>
        {
            Duration = new Timer();
            TimeSpan time = TimeSpan.FromMilliseconds(MPlayer.Length);
            Duration.SetActual(time);
        };
    }

    public void Play()
    {
        MPlayer.Play(_media);
    }
}
using System;
using System.Linq;
using LibVLCSharp.Shared;
using Mp3Player.Models;
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

    private string _artist;
    public string Artist
    {
        get => _artist;
        set => this.RaiseAndSetIfChanged(ref _artist, value);
    }

    private string _album;
    public string Album
    {
        get => _album;
        set => this.RaiseAndSetIfChanged(ref _album, value);
    }

    private string _artPath;
    public string ArtPath { get; set; }
    
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
    private PlaylistFile _playlistFile;

    public PlaylistFile PlaylistFile
    {
        get => _playlistFile;
        set => this.RaiseAndSetIfChanged(ref _playlistFile, value);
    }

    private bool _alreadyPlayed = false;
    private LibVLC _libVlc = null;
    public MusicViewModel(PlaylistFile playlistFile, LibVLC libVlc)
    {
        _libVlc = libVlc;
        PlaylistFile = playlistFile;
        IsNowPlaying = false;
        Title = playlistFile.FilePath.Split("/").Last();
        //Duration = duration;
        Path = new Uri(playlistFile.FilePath);
        _media = new Media(libVlc, Path);
        _media.Parse(MediaParseOptions.ParseLocal);
        MPlayer = new MediaPlayer(_media);
        
        //MPlayer.Play(media);
        _media.Parse();
        _media.ParsedChanged += (sender, args) =>
        {
            Duration = new Timer();
            TimeSpan time = TimeSpan.FromMilliseconds(_media.Duration);
            Duration.SetActual(time);
        };
        _media.MetaChanged += (sender, args) =>
        {
            Title = _media.Meta(MetadataType.Title);
            Artist = _media.Meta(MetadataType.Artist);
            Album = _media.Meta(MetadataType.Album);
            ArtPath = _media.Meta(MetadataType.ArtworkURL);
            //_media.Meta(MetadataType.TrackTotal);
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
        if (_alreadyPlayed)
        {
            //MPlayer = MPlayer.End;
        }
        _alreadyPlayed = true;
        MPlayer.Play(_media);
    }
}
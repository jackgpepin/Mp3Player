using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using LibVLCSharp.Shared;
using Mp3Player.Configs;
using Mp3Player.Enums;
using Mp3Player.Models;
using ReactiveUI;

namespace Mp3Player.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private static MainWindowViewModel _main;

        public static MainWindowViewModel Main { get; set; }
        private Uri _actualFileUri;

        public Uri ActualFileUri
        {
            get => _actualFileUri;
            set => this.RaiseAndSetIfChanged(ref _actualFileUri, value);
        }

        private string _title;

        public string Title
        {
            get => _title;
            set => this.RaiseAndSetIfChanged(ref _title, value);
        }
        
        private Timer _actualTime = new Timer();

        public Timer ActualTime
        {
            get => _actualTime;
            set => this.RaiseAndSetIfChanged(ref _actualTime, value);
        }

        private Timer _totalDuration = new Timer();
        public Timer TotalDuration
        {
            get => _totalDuration;
            set => this.RaiseAndSetIfChanged(ref _totalDuration, value);
        }
        private Timer _audioLength = new Timer();

        public Timer AudioLength
        {
            get => _audioLength;
            set => this.RaiseAndSetIfChanged(ref _audioLength, value);
        }
        
        private float _position;
        public float Position
        {                

            get => _position;
            set
            {
                this.RaiseAndSetIfChanged(ref _position, value);
                if (Position != SelectedPlayingMusic.MPlayer.Position)
                    SelectedPlayingMusic.MPlayer.Position = Position;
            }
        }

        private PlayerStatus _status = PlayerStatus.Paused;

        public PlayerStatus Status
        {
            get => _status;
            set => this.RaiseAndSetIfChanged(ref _status, value);
        }
        
        private int _volume;
        public int Volume
        {
            get => _volume;
            set
            {
                this.RaiseAndSetIfChanged(ref _volume, value);
                if (Volume != SelectedPlayingMusic.MPlayer.Volume)
                    SelectedPlayingMusic.MPlayer.Volume = Volume;
            }
        }
        public LibVLC _libVlc;
        private Media _media;
        public Media ActualMedia
        {
            get => _media;
            set
            {
                this.RaiseAndSetIfChanged(ref _media, value);
            }
        }
        private MediaPlayer _mediaP;
        public MediaPlayer _mediaPlayer
        {
            get => _mediaP;
            set => this.RaiseAndSetIfChanged(ref _mediaP, value);
        }

        private ObservableCollection<MusicViewModel> _musics;

        public ObservableCollection<MusicViewModel> Musics
        {
            get => _musics;
            set
            {
                this.RaiseAndSetIfChanged(ref _musics, value);
                MostListen = Musics;
            }
        }

        private MusicViewModel _selectedPlayingMusic;
        private bool _endingMusic = false;
        public MusicViewModel SelectedPlayingMusic
        {
            get => _selectedPlayingMusic;
            set
            {
                if(SelectedPlayingMusic != null && SelectedPlayingMusic.MPlayer.IsPlaying)
                    SelectedPlayingMusic.MPlayer.Stop();
                this.RaiseAndSetIfChanged(ref _selectedPlayingMusic, value);
                if (SelectedPlayingMusic != null)
                {
                    SelectedPlayingMusic.MPlayer.Mute = Muted;
                    SelectedPlayingMusic.MPlayer.SetEqualizer(EqualizerWindowViewModel.Equalizer);
                    var a = SelectedPlayingMusic.MPlayer.Media.Meta(MetadataType.Album);
                    Console.WriteLine();
                }
                SelectedMusic = SelectedPlayingMusic;
                SelectedPlayingMusic.MPlayer.PositionChanged += (sender, args) =>
                {
                    //Position = ((SelectedMusic.MPlayer.Position * 100) / 1);
                    Position = SelectedPlayingMusic.MPlayer.Position;
                    var time = TimeSpan.FromMilliseconds(SelectedPlayingMusic.MPlayer.Time);
                    ActualTime.SetActual(time);
                };
                SelectedPlayingMusic.MPlayer.LengthChanged += (sender, args) =>
                {
                    TotalDuration.SetActual(TimeSpan.FromMilliseconds(SelectedPlayingMusic.MPlayer.Length));
                };
                SelectedPlayingMusic.MPlayer.EndReached += (sender, args) =>
                {
                    if(_endingMusic) return;
                    _endingMusic = true;
                   Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        if (LoopMode == LoopMode.RepeatOne)
                        {
                            SelectedPlayingMusic.Play();
                            _endingMusic = false;
                            return;
                        }
                        NextCommand.Execute();
                        Debug.WriteLine("END");
                        _endingMusic = false;
                    });
                   Debug.WriteLine(SelectedPlayingMusic.Path);
                };
                SelectedPlayingMusic.MPlayer.Playing += (sender, args) => Status = PlayerStatus.Playing;
                SelectedPlayingMusic.MPlayer.Paused += (sender, args) => Status = PlayerStatus.Paused;
                SelectedPlayingMusic.MPlayer.Stopped += (sender, args) => Status = PlayerStatus.Paused;

                if (SelectedPlayingMusic != null)
                {
                    ContextMenuItems = _setMusicListContextMenuItemsFromSelectedMusic();
                }
            }
        }
        
        private MusicViewModel _selectedMusic;
        public MusicViewModel SelectedMusic
        {
            get => _selectedMusic;
            set => this.RaiseAndSetIfChanged(ref _selectedMusic, value);
        }

        private ObservableCollection<MusicViewModel> _mostListen;

        public ObservableCollection<MusicViewModel> MostListen
        {
            get => _mostListen;
            set => this.RaiseAndSetIfChanged(ref _mostListen, value);
        }

        private ViewModelBase _playlistContent;

        public ViewModelBase PlaylistContent
        {
            get => _playlistContent;
            set => this.RaiseAndSetIfChanged(ref _playlistContent, value);
        }

        private ObservableCollection<PlaylistViewModel> _playlists;

        public ObservableCollection<PlaylistViewModel> Playlists
        {
            get => _playlists;
            set => this.RaiseAndSetIfChanged(ref _playlists, value);
        }

        private PlaylistViewModel _selectedPlaylist;

        public PlaylistViewModel SelectedPlaylist
        {
            get => _selectedPlaylist;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedPlaylist, value);
                if (SelectedPlaylist != null)
                    PlaylistContent = SelectedPlaylist.Content;
            }
        }

        private ObservableCollection<MenuItem> _playlistsMenuItems;

        public ObservableCollection<MenuItem> PlaylistsMenuItems
        {
            get => _playlistsMenuItems;
            set => this.RaiseAndSetIfChanged(ref _playlistsMenuItems, value);
        }

        private LoopMode _loopMode;

        public LoopMode LoopMode
        {
            get => _loopMode;
            set => this.RaiseAndSetIfChanged(ref _loopMode, value);
        }

        private bool _shuffle;
        public bool Shuffle
        {
            get => _shuffle;
            set => this.RaiseAndSetIfChanged(ref _shuffle, value);
        }

        private bool _muted = false;
        public bool Muted
        {
            get => _muted;
            set
            {
                this.RaiseAndSetIfChanged(ref _muted, value);
                if (Musics != null)
                {
                    foreach (var music in Musics)
                    {
                        music.MPlayer.Mute = Muted;
                    }
                }
            }
        }
        private List<MusicViewModel> _playedMusics;

        private EqualizerWindowViewModel _equalizerWindowViewModel;

        public EqualizerWindowViewModel EqualizerWindowViewModel
        {
            get => _equalizerWindowViewModel;
            set => this.RaiseAndSetIfChanged(ref _equalizerWindowViewModel, value);
        }
        
        public MainWindowViewModel()
        {
            var settings = Settings.Initialize();
            Main = this;
            var args = Environment.GetCommandLineArgs();
            try
            {
                _libVlc = new LibVLC(enableDebugLogs: true);
                EqualizerWindowViewModel = new EqualizerWindowViewModel();
                Musics = new ObservableCollection<MusicViewModel>()
                {
                };
            }
            catch (Exception e)
            {
                
            }
            _initializePlaylists();
            LoopMode = LoopMode.RepeatAll;
            Console.WriteLine("");
            if (args != null)
            {
                foreach (var arg in args)
                {
                    if (arg.EndsWith(".mp3") || arg.EndsWith("mp4") || arg.EndsWith(("wav")) || arg.EndsWith(("ogg")))
                    {
                        MusicViewModel music = null;
                        try{
                            music = new MusicViewModel(new PlaylistFile(arg), _libVlc);
                            Musics.Add(music);
                        }catch(Exception e){}
                    }
                }
            }
            //ActualFileUri = new Uri("/home/denny/Music/a.mp3");
            PlayCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (SelectedPlayingMusic == null)
                {
                     
                    SelectedPlayingMusic = Musics.First();
                    //SelectedMusic?.Play();
                    SelectedPlayingMusic.MPlayer.Play();
                    Musics.First(m => m == SelectedPlayingMusic).IsNowPlaying = true;
                }
                else
                {
                    if (!SelectedPlayingMusic.MPlayer.CanPause)
                    {
                        SelectedPlayingMusic.Play();
                    }
                    SelectedPlayingMusic.MPlayer.Pause();
                }
                _mediaPlayer?.Dispose();
                /*ActualMedia = new Media(_libVlc, ActualFileUri);
                 _mediaPlayer = new MediaPlayer(ActualMedia);
                 _mediaPlayer.Play(ActualMedia);
                 _mediaPlayer.PositionChanged += (sender, args) =>
                 {
                    Position = ((_mediaPlayer.Position * 100) / 1);
                    var time = TimeSpan.FromMilliseconds(_mediaPlayer.Time);
                    ActualTime.SetActual(time);
                 };
                 ActualMedia.MetaChanged += (sender, args) => Title = _media.Meta(MetadataType.Title);

                 _mediaPlayer.LengthChanged += (sender, args) => AudioLength.SetActual(TimeSpan.FromMilliseconds(_mediaPlayer.Length));*/
            });
            PauseCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                _mediaPlayer?.Pause();
            });
            StopCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                _mediaPlayer?.Stop();
                Position = 0;
            });
            RestartCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                _mediaPlayer?.Stop();
                _mediaPlayer.Position = 0;

            });
            SelectMusicCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                //_playedMusics = new List<MusicViewModel>();
                Musics = ((MusicsDataGridViewModel) PlaylistContent).Playlist.Musics;
                SelectedMusic = Musics.First(m => m == ((MusicsDataGridViewModel) PlaylistContent).SelectedMusic);
           
                SelectedPlayingMusic = SelectedMusic;
                SelectedPlayingMusic.Play();
                //_playedMusics.Add
                _setActualPlayingMusicBackground();
            });
            PreviousCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if(Shuffle)
                    _shuffleMusic();
                else
                {
                    int index = Musics.ToList().IndexOf(SelectedPlayingMusic);
                    if ((index - 1) <= Musics.Count && index - 1 >=0)
                    {
                        SelectedPlayingMusic = Musics[index-1];
                        SelectedPlayingMusic.Play();
                        _setActualPlayingMusicBackground();

                    }
                }
                
            });
            NextCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if(Shuffle)
                    _shuffleMusic();
                else
                {
                    int index = Musics.ToList().IndexOf(SelectedPlayingMusic);
                    if ((index + 2) <= Musics.Count)
                    {
                        SelectedPlayingMusic = Musics[index+1];
                        SelectedPlayingMusic.Play();
                        _setActualPlayingMusicBackground();

                    }
                }
                
            });
            
            RemoveCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (SelectedMusic == null) return;
                Musics.Remove(SelectedMusic);
            });
            RemoveAllCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                Musics.Clear();
            });

            PlayAllCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (Musics.Count == 0) return;
                SelectedMusic = Musics.First();
                SelectedPlayingMusic.Play();
                _setActualPlayingMusicBackground();

            });
            AddMusicCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                string[] files = (await ShowOpenFileDialog.Handle(Unit.Default));
                if (files == null) return;
                List<MusicViewModel> musics = new List<MusicViewModel>();
                foreach (var file in files)
                {
                    musics.Add(new MusicViewModel(new PlaylistFile(file), _libVlc));
                }

                foreach (var music in musics)
                {
                    Musics.Add(music);
                }

            });
            CreateNewPlaylistCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var newPlaylistWindow = new NewPlaylistWindowViewModel();
                var playlist = await ShowNewPlaylistWindow.Handle(newPlaylistWindow);
                if (playlist == null) return;
                playlist.Content = new MusicsDataGridViewModel(playlist,this);
                foreach (var file in playlist._playlist.PlaylistFiles)
                {
                    playlist.Musics.Add(new MusicViewModel(file, _libVlc));
                }
                Playlists.Add(playlist);
                
            });
            ShowOpenFileDialog = new Interaction<Unit, string[]>();
            ShowNewPlaylistWindow = new Interaction<NewPlaylistWindowViewModel, PlaylistViewModel>();
            ShowOpenFolderDialog = new Interaction<Unit, string>();
            DeletePlaylistCommand = ReactiveCommand.Create(() =>
            {
                var playlist = SelectedPlaylist;
                if (SelectedPlaylist.Musics.Any(m => m.IsNowPlaying))
                {
                   SelectedPlaylist.Musics.First(m=>m.IsNowPlaying).MPlayer.Stop();
                   SelectedPlaylist = null;
                }
                SelectedPlaylist.Delete();
                SelectedPlaylist = Playlists.First();

                Playlists.Remove(playlist);
                
            });

            ChangeLoopModeCommand = ReactiveCommand.Create(() =>
            {
                if (LoopMode == LoopMode.RepeatAll)
                    LoopMode = LoopMode.RepeatOne;
                else if (LoopMode == LoopMode.RepeatOne)
                    LoopMode = LoopMode.RepeatAll;
            });

            ToggleShuffleCommand = ReactiveCommand.Create(() =>
            {
                Shuffle = !Shuffle;
            });

            ToggleMutedCommand = ReactiveCommand.Create(() =>
            {
                Muted = !Muted;
            });

            ShowEqualizerDialog = new Interaction<EqualizerWindowViewModel, Unit>();
            OpenEqualizerCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                await ShowEqualizerDialog.Handle(EqualizerWindowViewModel);
            });
        }

        private void _shuffleMusic()
        {
            var index = Random.Shared.NextInt64((long) Musics.Count - 1);
            SelectedPlayingMusic = Musics[(int)index];
            SelectedPlayingMusic.Play();
            _setActualPlayingMusicBackground();
        }
        private void _initializePlaylists()
        {
            Playlists = new ObservableCollection<PlaylistViewModel>();
            _loadFilesFromCMDArgs();
            //Playlists.Add(new PlaylistViewModel("Favoritas"){Content = new MusicsDataGridViewModel(this)});
            PlaylistContent = Playlists.First().Content;
            var playlists = Playlist.GetPlaylists();
            foreach(Playlist playlist in playlists)
            {
                var playlistViewModel = new PlaylistViewModel(playlist);
                foreach (var file in playlist.PlaylistFiles)
                {
                    playlistViewModel.Musics.Add(new MusicViewModel(file, _libVlc));
                }
                playlistViewModel.Content = new MusicsDataGridViewModel(playlistViewModel, this);
                Playlists.Add(playlistViewModel);
            }
        }

        private void _loadFilesFromCMDArgs()
        {
            
            var playlist = new PlaylistViewModel(new Playlist("Default"));
            playlist.Content = new MusicsDataGridViewModel(playlist, this);
            //var playlist = new PlaylistViewModel(){Content = new MusicsDataGridViewModel(this)};

            var args = Environment.GetCommandLineArgs();
            if (args != null)
            {
                foreach (var arg in args)
                {
                    if (arg.EndsWith(".mp3") || arg.EndsWith("mp4") || arg.EndsWith(("wav")) || arg.EndsWith(("ogg")))
                    {
                        MusicViewModel music = null;
                        try
                        {
                            music = new MusicViewModel(new PlaylistFile(arg), _libVlc);
                            ((MusicsDataGridViewModel)playlist.Content).Playlist.Musics.Add(music);
                            //Musics.Add(music);
                        }
                        catch(Exception e){}
                    }
                }
            }
            Playlists.Add(playlist);
        }

        private void _setActualPlayingMusicBackground()
        {
            if(Musics.Any(m=>m.IsNowPlaying))
                Musics.First(m => m.IsNowPlaying).IsNowPlaying = false;
            Musics.First(m => m == SelectedPlayingMusic).IsNowPlaying = true;

        }
        
        private async Task PlayNext()
        {
            
        }
        public ReactiveCommand<Unit, Unit> PlayCommand { get; set; }
        public ReactiveCommand<Unit, Unit> PlayAllCommand { get; set; }
        public ReactiveCommand<Unit, Unit> RemoveCommand { get; set; }
        public ReactiveCommand<Unit, Unit> RemoveAllCommand { get; set; }
        public ReactiveCommand<Unit, Unit> PauseCommand { get; set; }
        public ReactiveCommand<Unit, Unit> StopCommand { get; set; }
        public ReactiveCommand<Unit, Unit> RestartCommand { get; set; }
        public ReactiveCommand<Unit, Unit> SelectMusicCommand { get; set; }
        public ReactiveCommand<Unit, Unit> PreviousCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NextCommand { get; set; }
        public ReactiveCommand<Unit, Unit> AddMusicCommand { get; set; }
        
        public ReactiveCommand<Unit, Unit> ChangeLoopModeCommand { get; set; }
        public ReactiveCommand<Unit, Unit> ToggleShuffleCommand { get; set; }
        public ReactiveCommand<Unit, Unit> ToggleMutedCommand { get; set; }
        public ReactiveCommand<Unit, Unit> OpenEqualizerCommand { get; set; }
        public Interaction<EqualizerWindowViewModel, Unit> ShowEqualizerDialog { get; set; } 

        public Interaction<Unit, string[]> ShowOpenFileDialog { get; set; }
        public Interaction<Unit, string> ShowOpenFolderDialog { get; set; }
        public ReactiveCommand<Unit, Unit> CreateNewPlaylistCommand { get; set; }
        public Interaction<NewPlaylistWindowViewModel, PlaylistViewModel> ShowNewPlaylistWindow { get; set; }
        public ReactiveCommand<Unit, Unit> DeletePlaylistCommand { get; set; }
        public void DoubleClickMusic()
        {
            SelectMusicCommand.Execute();
        }

        private ObservableCollection<MenuItem> _contextMenuItems;

        public ObservableCollection<MenuItem> ContextMenuItems
        {
            get => _contextMenuItems;
            set => this.RaiseAndSetIfChanged(ref _contextMenuItems, value);
        }

        private ObservableCollection<MenuItem> _setMusicListContextMenuItemsFromSelectedMusic()
        {
            var items = new ObservableCollection<MenuItem>();
            if(SelectedPlayingMusic.MPlayer.IsPlaying)
                items.Add(new MenuItem(){Header = "Pause", Command = PlayCommand});
            else
                items.Add(new MenuItem(){Header = "Play", Command = PlayCommand});
            
            items.Add(new MenuItem(){Header = "Remove from list", Command = RemoveCommand});

            return items;
        }

      
        public void ShowPlaylistsContextMenu()
        {
            PlaylistsMenuItems = new ObservableCollection<MenuItem>();

            if (SelectedPlaylist == null)
            {
                PlaylistsMenuItems.Add(new MenuItem()
                {
                    Header = "New playlist",
                    Command = CreateNewPlaylistCommand
                });
            }
            else
            {
                PlaylistsMenuItems.Add(new MenuItem()
                {
                    Header = "New Playlist",
                    Command = CreateNewPlaylistCommand
                });
                PlaylistsMenuItems.Add(new MenuItem()
                {
                    Header = "Remove playlist",
                    Command = DeletePlaylistCommand
                });
            }
            
        }
    }
}
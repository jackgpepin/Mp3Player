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
using Mp3Player.Enums;
using Mp3Player.Models;
using ReactiveUI;

namespace Mp3Player.ViewModels
{
    public class MusicsDataGridViewModel : ViewModelBase
    {
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
        private LibVLC _libVlc;
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
        

        private MusicViewModel _selectedPlayingMusic;

        public MusicViewModel SelectedPlayingMusic
        {
            get => _selectedPlayingMusic;
            set
            {
                if(SelectedPlayingMusic != null && SelectedPlayingMusic.MPlayer.IsPlaying)
                    SelectedPlayingMusic.MPlayer.Stop();
                this.RaiseAndSetIfChanged(ref _selectedPlayingMusic, value);
                SelectedMusic = SelectedPlayingMusic;
                SelectedPlayingMusic.MPlayer.PositionChanged += (sender, args) =>
                {
                    //Position = ((SelectedMusic.MPlayer.Position * 100) / 1);
                    Position = SelectedPlayingMusic.MPlayer.Position;
                    var time = TimeSpan.FromMilliseconds(SelectedPlayingMusic.MPlayer.Time);
                    ActualTime.SetActual(time);
                };
                SelectedPlayingMusic.MPlayer.EndReached += (sender, args) =>
                {
                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        NextCommand.Execute();
                    });
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

        private string _text;

        public string Text
        {
            get => _text;
            set => this.RaiseAndSetIfChanged(ref _text, value);
        }

        private MainWindowViewModel _mainWindowViewModel;

        public MainWindowViewModel MainWindowViewModel
        {
            get => _mainWindowViewModel;
            set => this.RaiseAndSetIfChanged(ref _mainWindowViewModel, value);
        }

        private PlaylistViewModel _playlist;

        public PlaylistViewModel Playlist
        {
            get => _playlist;
            set => this.RaiseAndSetIfChanged(ref _playlist, value);

        } 
        
        public MusicsDataGridViewModel(PlaylistViewModel playlist, MainWindowViewModel mainWindowViewModel)
        {
            MainWindowViewModel = mainWindowViewModel;
            Playlist = playlist;
            try
            {
                _libVlc = mainWindowViewModel._libVlc;
            }
            catch (Exception e)
            {
                
            }
            Console.WriteLine("");
            
            //ActualFileUri = new Uri("/home/denny/Music/a.mp3");
            PlayCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (SelectedPlayingMusic == null)
                {
                     
                    SelectedPlayingMusic = Playlist.Musics.First();
                    //SelectedMusic?.Play();
                    SelectedPlayingMusic.MPlayer.Play();
                    Playlist.Musics.First(m => m == SelectedPlayingMusic).IsNowPlaying = true;
                }
                else
                {
                    if (!SelectedMusic.MPlayer.CanPause)
                    {
                        SelectedPlayingMusic = SelectedMusic;
                        //SelectedPlayingMusic.Play();
                        MainWindowViewModel.SelectedPlayingMusic = SelectedMusic;
                        MainWindowViewModel.SelectedPlayingMusic.Play();
                        _setActualPlayingMusicBackground();
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
                //_mediaPlayer = SelectedMusic.MPlayer;
                //_mediaPlayer.Play();
                //SelectedMusic.MPlayer.Play(SelectedMusic.MPlayer.Media);
                SelectedPlayingMusic = SelectedMusic;
                SelectedPlayingMusic.Play();
                _setActualPlayingMusicBackground();
            });
            PreviousCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                int index = Playlist.Musics.ToList().IndexOf(SelectedPlayingMusic);
                if ((index - 1) <= Playlist.Musics.Count && index - 1 >=0)
                {
                    SelectedPlayingMusic = Playlist.Musics[index-1];
                    SelectedPlayingMusic.Play();
                    _setActualPlayingMusicBackground();

                }
                
            });
            NextCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                int index = Playlist.Musics.ToList().IndexOf(SelectedPlayingMusic);
                if ((index + 2) <= Playlist.Musics.Count)
                {
                    SelectedPlayingMusic = Playlist.Musics[index+1];
                    SelectedPlayingMusic.Play();
                    _setActualPlayingMusicBackground();

                }
            });
            OpenFileCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                string[] files = (await MainWindowViewModel.ShowOpenFileDialog.Handle(Unit.Default));
                if (files == null) return;
                List<MusicViewModel> musics = new List<MusicViewModel>();
                foreach (var file in files)
                {
                    musics.Add(new MusicViewModel(new PlaylistFile(file), _libVlc));
                }

                foreach (var music in musics)
                {
                    //Musics.Add(music);
                    if(this == MainWindowViewModel.PlaylistContent)
                        Playlist.Musics.Add(music);
                    else
                    {
                        if (Playlist.Musics.Any(m => m.IsNowPlaying) && !MainWindowViewModel.Musics.Any(m => m == music)) ;
                        MainWindowViewModel.Musics.Add(music);
                    }
                }
                Playlist.Save();

                //SelectedPlayingMusic = musics.First();
                //SelectedPlayingMusic.Play();
                _setActualPlayingMusicBackground();

            });
            RemoveCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (SelectedMusic == null) return;
                if(SelectedMusic.IsNowPlaying) SelectedMusic.MPlayer.Stop();
                Playlist.RemoveFile(SelectedMusic.PlaylistFile);
                Playlist.Musics.Remove(SelectedMusic);
                
            });
            RemoveAllCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                Playlist.Musics.Clear();
            });

            PlayAllCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (Playlist.Musics.Count == 0) return;
                SelectedMusic = Playlist.Musics.First();
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
                    Playlist.Musics.Add(music);
                }
                

            });
            Text = Playlist.Musics.Count.ToString();
            ShowOpenFileDialog = new Interaction<Unit, string[]>();
            
        }

        private void _setActualPlayingMusicBackground()
        {
            if (Playlist.Musics.Any(m => m.IsNowPlaying))
            {
                Playlist.Musics.First(m => m.IsNowPlaying).IsNowPlaying = false;
                Playlist.Musics.First(m => m == SelectedPlayingMusic).IsNowPlaying = true;
            }

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
        public ReactiveCommand<Unit, Unit> OpenFileCommand { get; set; }
        public ReactiveCommand<Unit, Unit> AddMusicCommand { get; set; }
        public Interaction<Unit, string[]> ShowOpenFileDialog { get; set; }
        public void DoubleClickMusic()
        {
            SelectedPlayingMusic = SelectedMusic;
            MainWindowViewModel.SelectMusicCommand.Execute();
            //SelectMusicCommand.Execute();
        }

        private ObservableCollection<MenuItem> _contextMenuItems;

        public ObservableCollection<MenuItem> ContextMenuItems
        {
            get => _contextMenuItems;
            set => this.RaiseAndSetIfChanged(ref _contextMenuItems, value);
        }

        private  ObservableCollection<MenuItem> _setDefaultMusicListContextMenuItems()
        {
            Console.WriteLine("Generate context menu");
            var items = new ObservableCollection<MenuItem>()
            {
                new MenuItem(){Header = "Play all", Command = PlayAllCommand},
                new MenuItem(){Header = "Open file", Command = OpenFileCommand},
                new MenuItem(){Header = "Open folder", IsEnabled = false},
                new MenuItem(){Header = "Clear list", Command = RemoveAllCommand},
                new MenuItem(){Header = "Add music", Command = AddMusicCommand}
            };

            return items;
        }

        private ObservableCollection<MenuItem> _setMusicListContextMenuItemsFromSelectedMusic()
        {
            var items = new ObservableCollection<MenuItem>();
            if(SelectedMusic.IsNowPlaying && SelectedMusic.MPlayer.IsPlaying)
                items.Add(new MenuItem(){Header = "Pause", Command = PlayCommand});
            else
                items.Add(new MenuItem(){Header = "Play", Command = PlayCommand});
            
            items.Add(new MenuItem(){Header = "Remove from list", Command = RemoveCommand});

            return items;
        }

        public void ShowContextMenu()
        {
            if (SelectedMusic == null)
                ContextMenuItems = _setDefaultMusicListContextMenuItems();
            else
                ContextMenuItems = _setMusicListContextMenuItemsFromSelectedMusic();
        }
    }
}
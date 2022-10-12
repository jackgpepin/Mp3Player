using System;
using System.Reactive;
using LibVLCSharp.Shared;
using ReactiveUI;

namespace Mp3Player.ViewModels;

public class EqualizerWindowViewModel : ViewModelBase
{
    private Equalizer _equalizer = new Equalizer();
    public Equalizer Equalizer
    {
        get => _equalizer;
        set => this.RaiseAndSetIfChanged(ref _equalizer, value);
    }

    private float _preamp = 0;

    public float Preamp
    {
        get => _preamp;
        set
        {
            this.RaiseAndSetIfChanged(ref _preamp, value);
            Equalizer.SetPreamp(Preamp);
            MainWindowViewModel.Main.SelectedPlayingMusic.MPlayer.SetEqualizer(Equalizer);
        }
    }

    private double _band0 = 0;
    public double Band0
    {
        get => _band0;
        set
        {
            this.RaiseAndSetIfChanged(ref _band0, value);
            Equalizer.SetAmp((float)Band0, 0);
            Console.WriteLine(Band0);
            MainWindowViewModel.Main.SelectedPlayingMusic.MPlayer.SetEqualizer(Equalizer);

        }
    }
    
    private double _band1 = 0;
    public double Band1
    {
        get => _band1;
        set
        {
            this.RaiseAndSetIfChanged(ref _band1, value);
            Equalizer.SetAmp((float)Band1, 1);
            Console.WriteLine(Band1);
            MainWindowViewModel.Main.SelectedPlayingMusic.MPlayer.SetEqualizer(Equalizer);

        }
    }
    
    private double _band2 = 0;
    public double Band2
    {
        get => _band2;
        set
        {
            this.RaiseAndSetIfChanged(ref _band2, value);
            Equalizer.SetAmp((float)Band2, 2);
            Console.WriteLine(Band2);
            MainWindowViewModel.Main.SelectedPlayingMusic.MPlayer.SetEqualizer(Equalizer);

        }
    }
    
    private double _band3 = 0;
    public double Band3
    {
        get => _band3;
        set
        {
            this.RaiseAndSetIfChanged(ref _band3, value);
            Equalizer.SetAmp((float)Band3, 3);
            Console.WriteLine(Band3);
            MainWindowViewModel.Main.SelectedPlayingMusic.MPlayer.SetEqualizer(Equalizer);

        }
    }
    private double _band4 = 0;
    public double Band4
    {
        get => _band4;
        set
        {
            this.RaiseAndSetIfChanged(ref _band4, value);
            Equalizer.SetAmp((float)Band4, 4);
            Console.WriteLine(Band4);
            MainWindowViewModel.Main.SelectedPlayingMusic.MPlayer.SetEqualizer(Equalizer);

        }
    }
    private double _band5 = 0;
    public double Band5
    {
        get => _band5;
        set
        {
            this.RaiseAndSetIfChanged(ref _band5, value);
            Equalizer.SetAmp((float)Band5, 5);
            Console.WriteLine(Band5);
            MainWindowViewModel.Main.SelectedPlayingMusic.MPlayer.SetEqualizer(Equalizer);

        }
    }
    private double _band6 = 0;
    public double Band6
    {
        get => _band6;
        set
        {
            this.RaiseAndSetIfChanged(ref _band6, value);
            Equalizer.SetAmp((float)Band6, 6);
            Console.WriteLine(Band6);
            MainWindowViewModel.Main.SelectedPlayingMusic.MPlayer.SetEqualizer(Equalizer);

        }
    }
    private double _band7 = 0;
    public double Band7
    {
        get => _band7;
        set
        {
            this.RaiseAndSetIfChanged(ref _band7, value);
            Equalizer.SetAmp((float)Band7, 7);
            Console.WriteLine(Band7);
            MainWindowViewModel.Main.SelectedPlayingMusic.MPlayer.SetEqualizer(Equalizer);

        }
    }
    private double _band8 = 0;
    public double Band8
    {
        get => _band8;
        set
        {
            this.RaiseAndSetIfChanged(ref _band8, value);
            Equalizer.SetAmp((float)Band8, 8);
            Console.WriteLine(Band8);
            MainWindowViewModel.Main.SelectedPlayingMusic.MPlayer.SetEqualizer(Equalizer);

        }
    }
    private double _band9 = 0;
    public double Band9
    {
        get => _band9;
        set
        {
            this.RaiseAndSetIfChanged(ref _band9, value);
            Equalizer.SetAmp((float)Band9, 9);
            Console.WriteLine(Band9);
            MainWindowViewModel.Main.SelectedPlayingMusic.MPlayer.SetEqualizer(Equalizer);

        }
    }
    public EqualizerWindowViewModel()
    {
        ResetCommand = ReactiveCommand.Create(() =>
        {
            Preamp = 0;
            Band0 = 0;
            Band1 = 0;
            Band2 = 0;
            Band3 = 0;
            Band4 = 0;
            Band5 = 0;
            Band6 = 0;
            Band7 = 0;
            Band8 = 0;
            Band9 = 0;
        });
    }
    
    public ReactiveCommand<Unit, Unit> ResetCommand { get; set; }
}
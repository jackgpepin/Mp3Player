using System;
using ReactiveUI;

namespace Mp3Player.ViewModels;

public class Timer:ViewModelBase
{
    private string _formated = "00:00:00";
    public string Formated
    {
        get => _formated;
        set => this.RaiseAndSetIfChanged(ref _formated, value);
    }
    
    private long _hour;
    public long Hour
    {
        get => _hour;
        set => this.RaiseAndSetIfChanged(ref _hour, value);
    }
    private long _minutes;

    public long Minutes
    {
        get => _minutes;
        set => this.RaiseAndSetIfChanged(ref _minutes, value);
    }
    private long _seconds;

    public long Seconds
    {
        get => _seconds;
        set => this.RaiseAndSetIfChanged(ref _seconds, value);
    }
    private long _milliseconds;

    public long Milliseconds
    {
        get => _milliseconds;
        set => this.RaiseAndSetIfChanged(ref _milliseconds, value);
    }

    public void SetActual(TimeSpan time)
    {   
        Minutes = time.Minutes;
        Seconds = time.Seconds;
        Milliseconds = time.Milliseconds;

        Formated = $"{Hour.ToString("00")}:{Minutes.ToString("00")}:{Seconds.ToString("00")}";
    }

}
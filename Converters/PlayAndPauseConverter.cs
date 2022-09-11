using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using LibVLCSharp.Shared;
using Mp3Player.Enums;

namespace Mp3Player.Converters;

public class PlayAndPauseConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
        var path = string.Empty;
        Bitmap btmp = null;
        var v = (PlayerStatus) value;
        switch (v)
        {
            case PlayerStatus.Playing:
                path = "pause.png";
                break;
            case PlayerStatus.Paused:
                path = "play.png";
                break;
            default:
                path = "play.png";
                break;
        }

        btmp = new Bitmap(assets.Open(new Uri(@"avares://Mp3Player/Assets/" + path)));
        return btmp;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}
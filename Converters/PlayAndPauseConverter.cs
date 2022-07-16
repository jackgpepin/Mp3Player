using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using LibVLCSharp.Shared;
using Mp3Player.Enums;

namespace Mp3Player.Converters;

public class PlayAndPauseConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        Bitmap btmp = null;
        var v = (PlayerStatus) value;
        switch (v)
        {
            case PlayerStatus.Playing:
                btmp = new Bitmap("pause.png");
                break;
            case PlayerStatus.Paused:
                btmp = new Bitmap("play.png");
                break;
            default:
                btmp = new Bitmap("play.png");
                break;
        }

        return btmp;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}
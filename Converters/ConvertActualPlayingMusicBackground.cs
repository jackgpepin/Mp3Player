using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Mp3Player.Converters;

public class ConvertActualPlayingMusicBackground : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var val = (bool) value;
        if (val)
            return Brushes.Transparent;
        return Brushes.Transparent;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
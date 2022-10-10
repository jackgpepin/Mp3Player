using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml.Converters;
using Avalonia.Media;
using Brushes = Avalonia.Media.Brushes;
using Color = Avalonia.Media.Color;

namespace Mp3Player.Converters;

public class ConvertActualPlayingMusicBackground : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var val = (bool) value;
        if (val)
            return Brushes.White;
        Color col = Color.FromRgb(210, 215, 223);
        var brush =  new BrushConverter().ConvertFrom("#d2d7df") as Brush;
        return brush;

    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
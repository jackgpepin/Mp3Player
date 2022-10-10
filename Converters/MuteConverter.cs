using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace Mp3Player.Converters;

public class MuteConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
        var path = "volume2.png";
        if ((bool) value)
            path = "mute.png";

        return new Bitmap(assets.Open(new Uri(@"avares://Mp3Player/Assets/" + path)));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
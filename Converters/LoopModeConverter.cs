using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Shared.PlatformSupport;
using Mp3Player.Enums;

namespace Mp3Player.Converters;

public class LoopModeConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var loopMode = (LoopMode) value;
        var loopModeIconPath = "";
        if (loopMode == LoopMode.RepeatAll)
            loopModeIconPath = "repeat.png";
        else if (loopMode == LoopMode.RepeatOne)
            loopModeIconPath = "repeat-once.png";

        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
        var icon = assets.Open(new Uri(@"avares://Mp3Player/Assets/" + loopModeIconPath));
        var bitmap = new Bitmap(icon);
        return bitmap;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
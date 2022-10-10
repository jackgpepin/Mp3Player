using System;
using System.Globalization;
using System.IO;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using System.Text;
using System.Text.Encodings.Web;
using Avalonia;
using Avalonia.Platform;

namespace Mp3Player.Converters;

public class ArtConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
        var bitmap = new Bitmap(assets.Open(new Uri(@"avares://Mp3Player/Assets/mp3.jpg")));
        if (value == null)
            return bitmap;

        string path = Uri.UnescapeDataString(value as string);
        path = path.Replace("file://", "");
        if(File.Exists(path))
            return new Bitmap(path);
        return bitmap;

    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using System;
using System.Globalization;
using System.IO;

namespace Projet1.Utils
{
    public class ByteArrayToImageConverter : IValueConverter
    {
        public object? Convert(object? value, Type ?targetType, object? parameter, CultureInfo? culture)
        {
            if (value is byte[] imageData)
            {
                using (var stream = new MemoryStream(imageData))
                {
                    var bitmap = new Bitmap(stream);
                    return bitmap;
                }
            }
            return null;
        }

        public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            throw new NotImplementedException();
        }
    }
}

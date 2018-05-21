using System;
using System.Globalization;
using Xamarin.Forms;
using CRM.Data;

namespace CRM.Models.Converters
{
    public class ImageHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ImageData.GetImageHeight();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}

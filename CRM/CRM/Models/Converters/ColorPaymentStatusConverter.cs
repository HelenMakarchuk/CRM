using System;
using System.Globalization;
using Xamarin.Forms;
using CRM.Data;

namespace CRM.Models.Converters
{
    public class ColorPaymentStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //get color value by payment status
            return ColorData.PaymentStatus[(byte)value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}

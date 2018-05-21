using System;
using System.Globalization;
using Xamarin.Forms;
using CRM.Data;

namespace CRM.Models.Converters
{
    public class SumConverter : IValueConverter
    {
        /// <summary>
        /// Show to the user
        /// </summary>
        public object Convert(object value, Type targetType = null, object parameter = null, CultureInfo culture = null)
        {
            if (value != null && Double.TryParse(value.ToString(), out var d))
                return Double.Parse(value.ToString());
            else
                return 0;
        }

        /// <summary>
        /// Return from the user
        /// </summary>
        public object ConvertBack(object value, Type targetType = null, object parameter = null, CultureInfo culture = null)
        {
            return Decimal.Parse(value.ToString().Replace(",", "."),
                NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint | NumberStyles.AllowCurrencySymbol);
        }

        public bool TryConvertBack(string value)
        {
            return Decimal.TryParse(value.Replace(",", "."), 
                NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint | NumberStyles.AllowCurrencySymbol, 
                CultureInfo.CreateSpecificCulture("es-ES"), 
                out var d);
        }
    }
}

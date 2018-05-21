using System;
using System.Globalization;
using Xamarin.Forms;
using CRM.Data;

namespace CRM.Models.Converters
{
    public class PaymentMethodConverter : IValueConverter
    {
        public object Convert(object value, Type targetType = null, object parameter = null, CultureInfo culture = null)
        {
            try
            {
                //get enum value by index
                return ((PaymentPickerData.Method)((byte)value));
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public object ConvertBack(object value, Type targetType = null, object parameter = null, CultureInfo culture = null)
        {
            try
            {
                PaymentPickerData.Method paymentMethod = (PaymentPickerData.Method)Enum.Parse(typeof(PaymentPickerData.Method), (string)value);

                //get enum index by value
                return (int)paymentMethod;
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}

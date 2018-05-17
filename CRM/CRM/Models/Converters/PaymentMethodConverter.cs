using System;
using System.Globalization;
using Xamarin.Forms;
using CRM.Data;

namespace CRM.Models.Converters
{
    public class PaymentMethodConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
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

        public string Convert(byte value)
        {
            try
            {
                //get enum value by index
                return ((PaymentPickerData.Method)value).ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
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

        public int ConvertBack(string value)
        {
            try
            {
                PaymentPickerData.Method paymentMethod = (PaymentPickerData.Method)Enum.Parse(typeof(PaymentPickerData.Method), value);

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

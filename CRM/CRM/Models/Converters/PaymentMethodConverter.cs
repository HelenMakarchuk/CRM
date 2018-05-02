using System;
using System.Globalization;
using Xamarin.Forms;
using static CRM.Data.PickerData;

namespace CRM.Models.Converters
{
    public class PaymentMethodConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                //get enum value by index
                return ((PaymentMethods)((byte)value));
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
                return ((PaymentMethods)value).ToString();
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
                PaymentMethods paymentMethod = (PaymentMethods)Enum.Parse(typeof(PaymentMethods), (string)value);

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
                PaymentMethods paymentMethod = (PaymentMethods)Enum.Parse(typeof(PaymentMethods), value);

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

using System;
using System.Globalization;
using Xamarin.Forms;
using static CRM.Data.PickerData;

namespace CRM.Models.Converters
{
    public class PaymentStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                //get enum value by index
                return ((PaymentStatuses)((byte)value));
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
                return ((PaymentStatuses)value).ToString();
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
                PaymentStatuses paymentStatus = (PaymentStatuses)Enum.Parse(typeof(PaymentStatuses), (string)value);

                //get enum index by value
                return (int)paymentStatus;
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
                PaymentStatuses paymentStatus = (PaymentStatuses)Enum.Parse(typeof(PaymentStatuses), value);

                //get enum index by value
                return (int)paymentStatus;
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}

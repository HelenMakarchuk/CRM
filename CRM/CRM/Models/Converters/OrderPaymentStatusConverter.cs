using System;
using System.Globalization;
using Xamarin.Forms;
using CRM.Data;

namespace CRM.Models.Converters
{
    public class OrderPaymentStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType = null, object parameter = null, CultureInfo culture = null)
        {
            try
            {
                //get enum value by index
                return ((OrderPickerData.PaymentStatus)((byte)value));
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public object ConvertBack(object value, Type targetType = null, object parameter = null, CultureInfo culture = null)
        {
            try
            {
                OrderPickerData.PaymentStatus orderStatus = (OrderPickerData.PaymentStatus)Enum.Parse(typeof(OrderPickerData.PaymentStatus), (string)value);

                //get enum index by value
                return (int)orderStatus;
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}

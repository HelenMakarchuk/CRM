using System;
using System.Globalization;
using Xamarin.Forms;
using CRM.Data;

namespace CRM.Models.Converters
{
    public class OrderDeliveryStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                //get enum value by index
                return ((OrderPickerData.DeliveryStatus)((byte)value));
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string Convert(byte value)
        {
            try
            {
                //get enum value by index
                return ((OrderPickerData.DeliveryStatus)value).ToString();
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
                OrderPickerData.DeliveryStatus orderStatus = (OrderPickerData.DeliveryStatus)Enum.Parse(typeof(OrderPickerData.DeliveryStatus), (string)value);

                //get enum index by value
                return (int)orderStatus;
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
                OrderPickerData.DeliveryStatus orderStatus = (OrderPickerData.DeliveryStatus)Enum.Parse(typeof(OrderPickerData.DeliveryStatus), value);

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

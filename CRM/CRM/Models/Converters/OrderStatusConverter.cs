using System;
using System.Globalization;
using Xamarin.Forms;
using static CRM.Data.PickerData;

namespace CRM.Models.Converters
{
    public class OrderStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                //get enum value by index
                return ((OrderStatuses)((byte)value));
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
                return ((OrderStatuses)value).ToString();
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
                OrderStatuses orderStatus = (OrderStatuses)Enum.Parse(typeof(OrderStatuses), (string)value);

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
                OrderStatuses orderStatus = (OrderStatuses)Enum.Parse(typeof(OrderStatuses), value);

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

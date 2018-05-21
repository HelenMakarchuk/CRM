using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Xamarin.Forms;

namespace CRM.Data
{
    public class ImageData
    {
        public static string GetPaymentStatusImage(byte status)
        {
            string image = (status == 0 ? "paid.png" : "unpaid.png");

            return (Device.RuntimePlatform == Device.UWP ? $"Assets/{image}" : image);
        }

        public static int GetImageHeight()
        {
            return (Device.RuntimePlatform == Device.UWP ? 48 : 34);
        }
    }
}

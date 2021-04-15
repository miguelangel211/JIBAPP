using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CargadosTrucking.Helpers
{
    public class Photosexistinorderconverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int)
            {
                if (((int)value) > 0)
                    return (Color)Xamarin.Forms.Application.Current.Resources["amarillo"];
            }
            return (Color)Xamarin.Forms.Application.Current.Resources["blanco"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
                return !(bool)value;
            return value;
        }
    }
 
}

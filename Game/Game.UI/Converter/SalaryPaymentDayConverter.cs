using System;
using System.Globalization;
using System.Windows.Data;

namespace Game.UI.Converter
{
    class SalaryPaymentDayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString() + "th of every month";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Game.UI.Converter
{
    public class ProgressBarConverterUniform : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double progress = (double)value;
            Brush foreground = Brushes.Green;

            return foreground;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
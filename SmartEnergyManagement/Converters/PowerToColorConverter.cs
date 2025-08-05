using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SmartEnergyManagement.Converters
{
    public class PowerToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double power)
            {
                if (power > 15)
                    return Brushes.Green;
                else if (power > 8)
                    return Brushes.DarkOrange;
                else
                    return Brushes.Red;
            }
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BatteryToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double batteryLevel)
            {
                if (batteryLevel > 70)
                    return Brushes.Green;
                else if (batteryLevel > 30)
                    return Brushes.DarkOrange;
                else
                    return Brushes.Red;
            }
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StatusToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isNormal)
            {
                return isNormal ? "参数正常" : "参数异常!";
            }
            return "未知状态";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
    
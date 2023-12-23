using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace IDZ_CLIENT
{
    public class UriToBitmapImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var b = new BitmapImage
            {
                CacheOption = BitmapCacheOption.OnDemand,
                CreateOptions = BitmapCreateOptions.DelayCreation
            };
            b.BeginInit();
            b.UriSource = new Uri((string)value);
            b.EndInit();
            return b;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

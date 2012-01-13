using System;
using System.Windows.Data;
using DCT.WPF;

namespace DCT.Monitor.Client.Helpers
{
    public class TreeViewItemFormatter: MarkupBase, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var domain = values[0] as string;

            if (domain == null) return null;

            if (domain.StartsWith(" -- "))
            {
				return String.Format("{0}", domain);
            }

			return String.Format("{0} - {1}", domain, (int)values[1]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

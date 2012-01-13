using System;
using DCT.WPF;
using System.Windows.Data;

namespace DCT.Monitor.Client.Helpers
{
    public class TrimingConverter: MarkupBase, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values == null || values.Length != 2) return null;

            try
            {
                var str = values[0].ToString();
                if(String.IsNullOrEmpty(str)) return str;

                var width = System.Convert.ToDouble(values[1]);
                var length = (int)Math.Floor(width / 6.6);

                if (str.Length < length) return str;

                return str.Substring(0, length - 3) + "...";
            }
            catch
            {
                return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

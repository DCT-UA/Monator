using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Markup;
using System.Windows.Data;
using DCT.Monitor.Entities;

namespace DCT.Monitor.Client.Helpers
{
    public class RequestCounter: MarkupExtension, IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var list = value as IEnumerable<DomainStatistics>;
            var count = list == null ? 0 : list.Sum(r => r.Count);
            var format = (parameter as String) ?? "{0}";

            return string.Format(format, count);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}

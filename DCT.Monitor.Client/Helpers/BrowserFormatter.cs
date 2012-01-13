using System;
using System.Windows.Data;
using DCT.WPF;
using DCT.Monitor.Entities;
using DCT.Monitor.Client;

namespace DCT.Monitor.Client.Helpers
{
    public class BrowserFormatter : MarkupBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value == null) return "";
            var browser = (Browser)value;
            return string.Format( "/Styles/" + App.CurrentApp.ThemeName + "/Images/Browser/" + browser.GetBrowserName() + ".png" );
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

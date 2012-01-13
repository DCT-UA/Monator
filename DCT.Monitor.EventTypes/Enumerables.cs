namespace DCT.Monitor.Entities
{
    public enum Browser: byte
    {
        InternetExplorer = 1,
        Firefox = 2,
        Opera = 3,
        GoogleChrome = 4,
        Safari = 5,
        Other = 0
    }

    public static class BrowserExtensions
    {
        public static Browser GetBrowserFromString(this string str)
        {
            str = str.ToLower().Trim();

            if (str.Contains("firefox")) return Browser.Firefox;
            if (str.Contains("chrome")) return Browser.GoogleChrome;
            if (str.Contains("safari")) return Browser.Safari;
            if (str.Contains("opera")) return Browser.Opera;

            return str.Contains("ie") ? Browser.InternetExplorer : Browser.Other;
        }

        public static string GetBrowserName(this Browser browser)
        {
            switch (browser)
            {
                case Browser.Firefox: return "Firefox";
                case Browser.InternetExplorer: return "IE";
                case Browser.Safari: return "Safari";
                case Browser.GoogleChrome: return "Chrome";
                case Browser.Opera: return "Opera";
                default: return "Other";
            }
        }
    }
}

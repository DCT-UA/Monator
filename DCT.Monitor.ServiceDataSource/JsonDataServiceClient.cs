using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Script.Serialization;
using DCT.Monitor.Entities;
using DCT.Net.Browsers;

namespace DCT.Monitor.ServiceDataSource
{
    public class JsonDataServiceClient
    {
        private static string _endpoint;
        private static IBrowser _browser;
        private const string AuthorizePath = "{0}/service/logon?username={1}&password={2}";
        private const string StatsPath = "/service/count";
        private const string RequestsPath = "{0}/service/requests?id={1}";
        private static JavaScriptSerializer _jsonSerrializer;

        static JsonDataServiceClient()
        {
            _endpoint = String.Format("http://{0}", ConfigurationManager.AppSettings["domain"]);
            _browser = new WebRequestBrowser(BrowserCapabilities.Cookies);
            _jsonSerrializer = new JavaScriptSerializer();
        }

        private Uri GetUri(string path)
        {
            return new Uri(_endpoint + path);
        }

        public string Authorize(string userName, string password)
        {
            _browser.Navigate(GetLogonUri(userName, password), false);
            return (_browser.Html ?? "network").ToLower();
        }

        private Uri GetLogonUri(string userName, string password)
        {
            return new Uri(string.Format(AuthorizePath, _endpoint, userName, password));
        }

        public List<DomainStatistics> GetDomainsStats()
        {
            _browser.Navigate(GetUri(StatsPath), false);
            var data = _jsonSerrializer.Deserialize<List<DomainStatistics>>(_browser.Html);
            return data;
        }

        public List<PageRequest> GetDomainRequests(IPageSelector selector)
        {
            _browser.Navigate(GetRequestUri(selector), false);
            return _jsonSerrializer.Deserialize<List<PageRequest>>(_browser.Html);
        }

        private Uri GetRequestUri(IPageSelector selector)
        {
            return new Uri(string.Format(RequestsPath, _endpoint, selector.Id));
        }
    }
}

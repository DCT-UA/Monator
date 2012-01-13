using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCT.Monitor.Server.Models
{
    public class JsonpRequestModel
    {
        public string JsonAction { get; set; }
        public string Callback { get; set; }
        public string Url { get; set; }
        public string Referer { get; set; }
        public int Duration { get; set; }
        public Guid SiteId { get; set; }
        public Guid? RequestId { get; set; }
    }

    public class JsonpResponseModel
    {
        public int Delay { get; set; }
        public Guid? RequestId { get; set; }
        public string Callback { get; set; }

        public override string ToString()
        {
            if (RequestId != null)
            {
                return String.Format("{0}({{delay:{1}, id: '{2}', session: '{3}'}})", Callback, Delay, RequestId, SessionId);
            }
            else
            {
                return String.Format("{0}({{delay:{1}}})", Callback, Delay);
            }
        }

        public Guid SessionId { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DCT.LoggingServices;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace DCT.Monitor.Server.Providers
{
    public class EntLibLogger: ILog
    {

        public void Error(Exception e)
        {
            Logger.Write(e);
        }

        public void Info(string message)
        {
            Logger.Write(message);
        }
    }
}
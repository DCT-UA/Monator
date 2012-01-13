using System.Configuration;

namespace DCT.Monitor.Modules.Implementation.ConfigurationModule
{
    public class WebConfigurationModule: ConfigurationModule
    {
        private static int _coreDelay = int.Parse(ConfigurationManager.AppSettings["CoreDelay"]);

        public WebConfigurationModule()
        {
            this.SaveRequestsToDb = bool.Parse(ConfigurationManager.AppSettings["SaveRequestsToDb"]);
        }

        public override int CoreDelay
        {
            get { return _coreDelay; }
            set { _coreDelay = value; }
        }
    }
}

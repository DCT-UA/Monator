using DCT.Monitor.Modules;

namespace DCT.Monitor.Modules.Implementation.ConfigurationModule
{
    public class ConfigurationModule : IConfigurationModule
    {
        public bool SaveRequestsToDb { get; protected set; }
        public virtual int CoreDelay { get; set; }
    }
}

namespace DCT.Monitor.Modules
{
    public interface IConfigurationModule
    {
        /// <summary>
        /// If true - all requests will be saved to database
        /// </summary>
        bool SaveRequestsToDb { get; }

        /// <summary>
        /// Period between requests and releasing stats
        /// </summary>
        int CoreDelay { get; set; }
    }
}

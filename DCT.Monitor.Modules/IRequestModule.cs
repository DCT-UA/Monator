using DCT.Monitor.Entities;

namespace DCT.Monitor.Modules
{
    public interface IRequestModule
    {
        // sends request tick
        void SendRequest(PageRequest request);
    }
}

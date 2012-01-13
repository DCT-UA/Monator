using DCT.Unity;
using DCT.Monitor.Entities;
using DCT.Monitor.Modules;

namespace DCT.Monitor.Modules.Implementation.RequestModule
{
    public abstract class BaseRequestModule:IRequestModule 
    {
        private static IRequestModule GatheringRequestModule = ServiceLocator.Current.Resolve<IRequestModule>("requests");  
        public void SendRequest(PageRequest request)
        {
            OnSendRequest(request);
            GatheringRequestModule.SendRequest(request);
        }

        protected abstract void OnSendRequest(PageRequest request);         
    }
}

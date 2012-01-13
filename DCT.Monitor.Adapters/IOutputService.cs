using System;

namespace DCT.Monitor.Adapters
{
    // output service for streaminsight
    public interface IOutputService<TData>
    {
        // event occurs when new output event appears in streaminsight core
        event EventHandler<EventArgs<TData>> Next;
    }
}

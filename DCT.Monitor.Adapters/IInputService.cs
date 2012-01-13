namespace DCT.Monitor.Adapters
{
    // Input interface for streaminsight core
	public interface IInputService<TEvent>
	{
        // sends event to streaminsight
		void PushEvent(TEvent evt);
	}
}

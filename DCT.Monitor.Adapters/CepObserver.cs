using System;

namespace DCT.Monitor.Adapters
{
    public class CepObserver<T>: IObserver<T>, IOutputService<T>
    {
        public event EventHandler<EventArgs<T>> Next;

        public void OnCompleted()
        {
            Next(this, null);
        }

        public void OnError(Exception error)
        {
            error.ToString();
        }

        public void OnNext(T value)
        {
            if (Next != null) Next(this, new EventArgs<T>(value));
        }
    }

    public class EventArgs<TEventData>: EventArgs{
        public TEventData Data { get; set; }

        public EventArgs(TEventData value)
        {
            Data = value;
        }
    }
}

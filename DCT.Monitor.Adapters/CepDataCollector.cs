using System;
using System.Collections.Generic;
using Microsoft.ComplexEventProcessing;

namespace DCT.Monitor.Adapters
{
    public class CepDataCollector<TPayload>: IOutputService<IEnumerable<TPayload>>, IObserver<PointEvent<TPayload>>
    {
        private List<TPayload> _data;
        

        public CepDataCollector()
        {
            _data = new List<TPayload>();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(PointEvent<TPayload> evt)
        {
            lock (this)
            {
                if (evt.EventKind == EventKind.Cti)
                {
                    if(Next != null) Next(this, new EventArgs<IEnumerable<TPayload>>(_data));
                    _data = new List<TPayload>();
                    return;
                }
                _data.Add(evt.Payload);
            }
        }

        public event EventHandler<EventArgs<IEnumerable<TPayload>>> Next;
    }
}

using System;
using System.Collections.Generic;

namespace DCT.Monitor.Adapters
{
    public class InputCollection<T> : IObservable<T>, IInputService<T>
    {
        private class ObservableHandler: IDisposable
        {
            public InputCollection<T> _observableBase;
            public IObserver<T> _observer;

            public ObservableHandler(IObserver<T> observer, InputCollection<T> observableBase)
            {
                this._observableBase = observableBase;
                this._observer = observer;

                lock (_observableBase)
                {
                    _observableBase._Handlers.Add(observer, this);
                }
            }

            public void Dispose()
            {
                lock (_observableBase)
                {
                    _observableBase._Handlers.Remove(_observer);
                }
            }
        }

        private Dictionary<IObserver<T>, ObservableHandler> _Handlers;

        public InputCollection()
        {
            _Handlers = new Dictionary<IObserver<T>, ObservableHandler>();
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return new ObservableHandler(observer, this);
        }

        public void PushEvent(T element)
        {
            var keys = new List<IObserver<T>>(_Handlers.Keys);
            foreach (var k in keys)
            {
                k.OnNext(element);
            }
        }
    }
}

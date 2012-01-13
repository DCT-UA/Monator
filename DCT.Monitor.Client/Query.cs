using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;

namespace DCT.Monitor.Client
{
    public class Query<T>
    {
        protected IEnumerable<T> _data;
        private Func<IEnumerable<T>> _source;
        private DateTime _expired;
        private Dispatcher _dispatcher;
        private bool _isUpdating;
        private object _syncRoot;
        private TimeSpan _expirationDuration;

        public event EventHandler Updated;

        public IEnumerable<T> Data { get { Check();  return _data; } }

        public Query(Func<IEnumerable<T>> source, TimeSpan expiration)
        {
            _source = source;
            _syncRoot = new object();
            _dispatcher = Dispatcher.CurrentDispatcher;
            _expirationDuration = expiration;
        }

        private void Check()
        {
            lock (_syncRoot)
            {
                if (_isUpdating || DateTime.Now <= _expired) return;
                _isUpdating = true;
                _expired = DateTime.Now + _expirationDuration;
            }

            _source.BeginInvoke(UpdateComplete, null);
        }

        private void UpdateComplete(IAsyncResult result)
        {
            var newdata = _source.EndInvoke(result).ToList();

            _data = newdata;

            lock (_syncRoot)
            {
                _isUpdating = false;
            }

            if (Updated != null) Updated(this, EventArgs.Empty);
        }
    }
}

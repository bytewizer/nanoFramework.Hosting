//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace System.Threading
{
    internal class ThreadPoolThread : IDisposable
    {
        private Thread _thread;
        private ThreadPoolItem _item;
        private readonly ManualResetEvent _WaitEvent = new ManualResetEvent(false);

        public ThreadPoolThread()
        {
            _thread = new Thread(ThreadProc);
            _thread.Start();
        }
        public bool IsBusy { get; set; }

        public ThreadPoolItem Item
        {
            get { return _item; }
            set
            {
                _item = value;
                if (_item != null)
                {
                    IsBusy = true;
                    _WaitEvent.Set();
                }
            }
        }

        private void ThreadProc()
        {
            while (_thread != null)
            {
                try
                {
                    _WaitEvent.WaitOne();

                    if (_thread != null && _item != null)
                    {
                        _item.Callback(_item.State);
                    }
                }
                catch (Exception ex)
                {
                    ThreadPool.OnUnhandledThreadPoolException(Item, ex);
                }

                if (_thread != null)
                {
                    _WaitEvent.Reset();
                    _item = null;
                    IsBusy = ThreadPool.NotifyThreadIdle(this);
                }
            }
        }

        public void Dispose()
        {
            IsBusy = true;
            _thread = null;
            _WaitEvent.Set();
        }
    }
}
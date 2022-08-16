//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System.Collections;
using System.Diagnostics;

namespace System.Threading
{
    /// <summary>
    /// Provides a pool of threads that can be used to execute tasks, post work items, 
    /// process asynchronous I/O, wait on behalf of other threads, and process timers.
    /// </summary>
    /// <remarks>
    /// Only short running operations should be executed by using ThreadPool. Multiple long running operations
    /// would block the available threads. New operations will not be called at all if all threads are blocked.
    /// </remarks>
    public static class ThreadPool
    {
        private static int _minThreadCount;
        private static int _maxThreadCount = 10;
        private static readonly ArrayList _threads = new ArrayList();
        private static readonly Queue _itemsQueue = new Queue();

        /// <summary>
        /// A delegate which is executed one of the worker threads in unhandeld.
        /// </summary>
        public static event UnhandledThreadPoolExceptionDelegate UnhandledThreadPoolException;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadPool"/> class.
        /// </summary>
        static ThreadPool()
        {
            // create the initial number of threads
            SetMinThreads(5);
        }

        /// <summary>
        /// Queues a method for execution. The method executes when a thread pool thread becomes available.
        /// </summary>
        /// <param name="callback">A <see cref="WaitCallback"/> that represents the method to be executed.</param>
        public static bool QueueUserWorkItem(WaitCallback callback)
        {
            return QueueUserWorkItem(callback, null);
        }

        /// <summary>
        /// Queues a method for execution. The method executes when a thread pool thread becomes available.
        /// </summary>
        /// <param name="callback">A <see cref="WaitCallback"/> that represents the method to be executed.</param>
        /// <param name="state">An object containing data to be used by the method.</param>
        public static bool QueueUserWorkItem(WaitCallback callback, object state)
        {
            lock (_itemsQueue.SyncRoot)
            {
                var thread = GetThread();
                if (thread != null)
                {
                    thread.Item = new ThreadPoolItem(callback, state);
                }
                else
                {
                    _itemsQueue.Enqueue(new ThreadPoolItem(callback, state));
                }
                return true;
            }
        }

        /// <summary>
        /// Retrieves the minimum number of threads the thread pool creates on demand, as new
        /// requests are made, before switching to an algorithm for managing thread creation and destruction.
        /// </summary>
        public static int GetMinThreads()
        {
            return _minThreadCount;
        }

        /// <summary>
        /// Sets the minimum number of threads the thread pool creates on demand, as new requests are made,
        /// before switching to an algorithm for managing thread creation and destruction.
        /// </summary>
        /// <param name="workerThreads">The minimum number of worker threads that the thread pool creates on demand.</param>
        public static bool SetMinThreads(int workerThreads)
        {
            _minThreadCount = workerThreads;

            while (_threads.Count < _minThreadCount)
            {
                CreateNewThread();
            }
            return true;
        }

        /// <summary>
        /// Retrieves the number of requests to the thread pool that can be active concurrently. All requests 
        /// above that number remain queued until thread pool threads become available.
        /// </summary>
        public static int GetMaxThreads()
        {
            return _maxThreadCount;
        }

        /// <summary>
        /// Sets the number of requests to the thread pool that can be active concurrently. All requests above
        /// that number remain queued until thread pool threads become available.
        /// </summary>
        /// <param name="workerThreads">The maximum number of worker threads in the thread pool.</param>
        public static bool SetMaxThreads(int workerThreads)
        {
            _maxThreadCount = workerThreads;
            return true;
        }

        /// <summary>
        /// Shuts down all threads after they have finished their work.
        /// </summary>
        public static void Shutdown()
        {
            lock (_threads)
            {
                foreach (ThreadPoolThread thread in _threads)
                {
                    thread.Dispose();
                }
                _threads.Clear();
            }
        }

        private static ThreadPoolThread GetThread()
        {
            lock (_threads)
            {
                foreach (ThreadPoolThread thread in _threads)
                {
                    if (!thread.IsBusy)
                    {
                        thread.IsBusy = true;
                        return thread;
                    }
                }

                if (_threads.Count < _maxThreadCount)
                {
                    var thread = new ThreadPoolThread { IsBusy = true };
                    _threads.Add(thread);
                    return thread;
                }

                return null;
            }
        }

        private static void CreateNewThread()
        {
            lock (_threads)
            {
                _threads.Add(new ThreadPoolThread());
            }
        }

        internal static bool NotifyThreadIdle(ThreadPoolThread thread)
        {
            lock (_threads)
            {
                if (_threads.Count > _maxThreadCount)
                {
                    thread.Dispose();
                    _threads.Remove(thread);
                    Debug.WriteLine(string.Concat("ThreadPool | ", DateTime.UtcNow.ToString("MM/dd/yyyy | HH:mm:ss.fff"), " | Thread stopped: #" + _threads.Count));
                    return false;
                }
            }

            // start next enqueued item
            lock (_itemsQueue.SyncRoot)
            {
                if (_itemsQueue.Count > 0)
                {
                    thread.Item = _itemsQueue.Dequeue() as ThreadPoolItem;
                    return true;
                }
            }

            return false;
        }

        internal static void OnUnhandledThreadPoolException(ThreadPoolItem item, Exception exception)
        {
            UnhandledThreadPoolException?.Invoke(item.State, exception);
        }
    }
}
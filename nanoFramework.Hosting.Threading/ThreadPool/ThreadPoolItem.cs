//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace System.Threading
{
    internal class ThreadPoolItem
    {
        public WaitCallback Callback { get; private set; }
        public object State { get; private set; }

        public ThreadPoolItem(WaitCallback callback, object state)
        {
            Callback = callback;
            State = state;
        }
    }
}
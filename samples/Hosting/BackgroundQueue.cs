﻿using System;
using System.Collections;

namespace Hosting
{
    internal class BackgroundQueue
    {
        private readonly int _maxQueueCount = 500;
        private readonly Queue _items = new Queue();
        private readonly object _syncLock = new object();

        public int QueueCount
        {
            get 
            {
                return _items.Count;
            }
        }

        public void Enqueue(object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (_items.Count < _maxQueueCount)
            {
                lock (_syncLock)
                {
                    _items.Enqueue(item);
                }
            }
        }

        public object Dequeue()
        {
            if (_items.Count > 0)
            {
                lock (_syncLock)
                {
                    var workItem = _items.Dequeue();
                    if (workItem != null)
                    {
                        return workItem;
                    }
                }
            }

            return null;
        }
    }
}
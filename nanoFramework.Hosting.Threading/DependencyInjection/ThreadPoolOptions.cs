//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Hosting.Threading
{
    /// <summary>
    /// Represents configuration options of the thread pool specific features.
    /// </summary>
    public class ThreadPoolOptions 
    {
        /// <summary>
        /// Sets the minimum number of threads the thread pool creates on demand, as new requests are made,
        /// before switching to an algorithm for managing thread creation and destruction.
        /// </summary>
        public int MinThreads { get; set; }

        /// <summary>
        /// Retrieves the number of requests to the thread pool that can be active concurrently. All requests 
        /// above that number remain queued until thread pool threads become available.
        /// </summary>
        public int MaxThreads { get; set; }    
    }
}
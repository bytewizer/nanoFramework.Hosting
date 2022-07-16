﻿//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.DependencyInjection;
using System;
using System.Collections;

namespace nanoFramework.Hosting.Internal
{
    /// <summary>
    /// Default implementation of <see cref="IHost"/>.
    /// </summary>
    internal class Host : IHost
    {
        private bool _disposed;
        private object[] _hostedServices;

        /// <summary>
        /// Initializes a new instance of <see cref="Host"/>.
        /// </summary>
        public Host(IServiceProvider services)
        {
            if (services == null)
            {
                throw new ArgumentNullException();
            }

            Services = services;
        }

        /// <inheritdoc />
        public IServiceProvider Services { get; }

        /// <inheritdoc />
        public void Start()
        {
            _hostedServices = Services.GetServices(typeof(IHostedService));

            ArrayList exceptions = null;

            for (int index = 0; index < _hostedServices.Length; index++)
            {
                try
                {
                    ((IHostedService)_hostedServices[index]).Start();

                    if (_hostedServices[index] is BackgroundService backgroundService)
                    {
                        backgroundService.ExecuteThread().Start();
                    }
                }
                catch (Exception ex)
                {
                    exceptions ??= new ArrayList();
                    exceptions.Add(ex);
                }
            }

            if (exceptions != null)
            {
                throw new AggregateException(string.Empty, exceptions);
            }
        }

        /// <inheritdoc />
        public void Stop()
        {
            if (_hostedServices == null)
            {
                return;
            }

            ArrayList exceptions = null;

            for (int index = _hostedServices.Length - 1; index >= 0; index--)
            {
                try
                {
                    ((IHostedService)_hostedServices[index]).Stop();
                }
                catch (Exception ex)
                {
                    exceptions ??= new ArrayList();
                    exceptions.Add(ex);
                }
            }

            _hostedServices = null;

            if (exceptions != null)
            {
                throw new AggregateException(string.Empty, exceptions);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    ((IDisposable)Services).Dispose();
                }

                _disposed = true;
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

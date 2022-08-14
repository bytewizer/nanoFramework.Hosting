//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Hosting.Pipeline.Builder;
using System;

namespace nanoFramework.Hosting.Pipeline
{ 
    /// <summary>
    /// Represents a base implementation of the <see cref="Middleware"/> for creating an application request pipeline.
    /// </summary>
    public abstract class Middleware : IMiddleware
    {
        private IMiddleware next;

        /// <summary>
        /// Invokes a middleware in the application pipeline.
        /// </summary>
        /// <param name="context">The context for the request.</param>
        /// <param name="next">The next request handler to be executed.</param>
        protected abstract void Invoke(IContext context, RequestDelegate next);

        /// <inheritdoc/>
        public void Use(InlineDelegate middleware)
        {
            Use(new InlineMiddleware(middleware));
        }

        /// <inheritdoc/>
        public void Use(IMiddleware middleware)
        {
            if (middleware == null)
            {
                throw new ArgumentNullException();
            }

            if (next == null)
            {
                next = middleware;
            }
            else
            {
                next.Use(middleware);
            }
        }

        /// <inheritdoc/>
        void IApplication.Invoke(IContext context)
        {
            Invoke(context, ctx =>
                {
                    if (next != null)
                        next.Invoke(ctx);
                });
        }
    }
}
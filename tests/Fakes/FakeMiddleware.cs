//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.TestFramework;
using nanoFramework.Hosting.Pipeline;

namespace nanoFramework.Hosting.UnitTests.Fakes
{
    public class FakeMiddleware : Middleware
    {
        public bool BeforeNext { get; private set; }
        public bool AfterNext { get; private set; }

        public FakeMiddleware()
        {
           BeforeNext = false;
           AfterNext = false;
        }

        protected override void Invoke(IContext context, RequestDelegate next)
        {
            var ctx = context as Context;
            
            ctx.StepCount++;
            BeforeNext = true;
            
            Assert.Equal(1, ctx.StepCount);

            next(context);    
            
            ctx.StepCount++;
            AfterNext = true;

            Assert.Equal(2, ctx.StepCount);
        }
    }
}

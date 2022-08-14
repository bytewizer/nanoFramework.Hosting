//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.TestFramework;
using nanoFramework.DependencyInjection;
using nanoFramework.Hosting.Pipeline;
using nanoFramework.Hosting.UnitTests.Fakes;
using nanoFramework.Hosting.Pipeline.Builder;

namespace nanoFramework.Hosting.UnitTests
{
    [TestClass]
    public class PipelineTests
    {
        [TestMethod]
        public void CreateAndReleaseContextPool()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton(typeof(Context))
                .AddSingleton(typeof(ContextPool))
                .AddSingleton(typeof(FakeMiddleware))
                .AddSingleton(typeof(ApplicationBuilder))
                .BuildServiceProvider();

            var contextPool = (ContextPool)serviceProvider.GetRequiredService(typeof(ContextPool));
            var builder = (ApplicationBuilder)serviceProvider.GetRequiredService(typeof(ApplicationBuilder));
            
            // add middleware to pipeline (order matters)
            builder.Use(typeof(FakeMiddleware));

            // get context from pool or create context if one is not available
            var context = (Context)contextPool.GetContext(typeof(Context));

            // build application pipeline
            var app = builder.Build();
            
            // invoke pipeline
            app.Invoke(context);
            Assert.Equal(2, context.StepCount);

            // release context back to pool and clear context
            contextPool.Release(context);
            Assert.Equal(0, context.StepCount);
            Assert.Equal(string.Empty, context.Message);
        }

        [TestMethod]
        public void CreateAndInvokeMultistepPipeline()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton(typeof(FakeMiddleware3))
                .AddSingleton(typeof(FakeMiddleware2))
                .AddSingleton(typeof(FakeMiddleware1))
                .BuildServiceProvider();

            var builder = new ApplicationBuilder(serviceProvider);

            // Order items are added to pipeline determines the order invoked
            builder.Use(typeof(FakeMiddleware1));
            builder.Use(typeof(FakeMiddleware2));
            builder.Use(typeof(FakeMiddleware3));

            var app = builder.Build();

            app.Use((context, next) =>
            {
                var ctx = context as Context;
                ctx.Message += "...";

                ctx.StepCount++;

                next(context);

                ctx.StepCount++;
            });

            var ctx = new Context() { Message = "Context: Finished" };
            app.Invoke(ctx);

            Assert.Equal(8, ctx.StepCount);
            Assert.Equal("Context: Finished...", ctx.Message);

            var middleware1 = (FakeMiddleware1)serviceProvider.GetRequiredService(typeof(FakeMiddleware1));
            Assert.True(middleware1.BeforeNext);
            Assert.True(middleware1.AfterNext);

            var middleware2 = (FakeMiddleware2)serviceProvider.GetRequiredService(typeof(FakeMiddleware2));
            Assert.True(middleware2.BeforeNext);
            Assert.True(middleware2.AfterNext);

            var middleware3 = (FakeMiddleware3)serviceProvider.GetRequiredService(typeof(FakeMiddleware3));
            Assert.True(middleware3.BeforeNext);
            Assert.True(middleware3.AfterNext);
        }
    }

    public class Context : IContext
    {
        public string Message { get; set; }
        public int StepCount { get; set; }

        public void Clear()
        {
            Message = string.Empty;
            StepCount = 0;
        }
    }
}

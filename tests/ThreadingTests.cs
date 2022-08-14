////
//// Copyright (c) .NET Foundation and Contributors
//// See LICENSE file in the project root for full license information.
////

//using nanoFramework.TestFramework;
//using nanoFramework.DependencyInjection;
//using nanoFramework.Hosting.Threading;
//using nanoFramework.Threading;

//namespace nanoFramework.Hosting.UnitTests
//{
//    [TestClass]
//    public class ThreadingTests
//    {
//        [TestMethod]
//        public void CreateServiceCollectionAndMofifyThreads()
//        {
//            var serviceProvider = new ServiceCollection()
//                .AddThreadPool(options => 
//                { 
//                    options.MinThreads = 3;
//                    options.MaxThreads = 30;
//                })
//                .BuildServiceProvider();

//            Assert.Equal(30, ThreadPool.GetMaxThreads());
//            Assert.Equal(5, ThreadPool.GetMinThreads());
//        }
//    }
//}

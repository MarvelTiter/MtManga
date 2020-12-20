
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MT.MVVM.Core;
using MT.MVVM.Core.Ioc;

namespace Core.Test {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void TestMethod1() {
            ServiceLocator.SetLocatorProvider(() => MIoC.Default);
            MIoC.Default.RegisterScope<ITest1, Test1>();

            var ins = ServiceLocator.Current.GetScope<ITest1>();
            Assert.IsTrue(ins.Count == 1);
        }
    }

    public interface ITest1 {
        int Count { get; }
    }

    public class Test1 : ITest1 {
        public int Count { get; set; } = 1;
    }
}

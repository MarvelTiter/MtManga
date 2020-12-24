
using System;
using CommonServiceLocator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MT.MVVM.Core;
using MT.MVVM.Core.Ioc;

namespace Core.Test {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void TestMethod1() {
            ServiceLocator.SetLocatorProvider(() => MIoC.Default);
            MIoC.Default.RegisterSingle<ITest1, Test1>(true);

            var ins1 = ServiceLocator.Current.GetInstance<ITest1>();
            var ins2 = ServiceLocator.Current.GetInstance<ITest1>();
            Assert.AreSame(ins1, ins2);
        }
        [TestMethod]
        public void TestRegisterKey() {
            ServiceLocator.SetLocatorProvider(() => MIoC.Default);
            MIoC.Default.RegisterSingle<ITest1, Test1>(false, "No1");
            MIoC.Default.RegisterSingle<ITest1, Test1>(false, "No2");

            var ins1 = ServiceLocator.Current.GetInstance<ITest1>("No1");
            var ins2 = ServiceLocator.Current.GetInstance<ITest1>("No1");
            var ins3 = ServiceLocator.Current.GetInstance<ITest1>("No2");

            Assert.AreSame(ins1, ins2);
            Assert.AreNotSame(ins1, ins3);
        }
        [TestMethod]
        public void TestUnRegister() {
            ServiceLocator.SetLocatorProvider(() => MIoC.Default);
            MIoC.Default.RegisterSingle<ITest1, Test1>(false, "No1");
            MIoC.Default.RegisterSingle<ITest1, Test1>(false, "No2");
            MIoC.Default.UnRegister<ITest1>("No1");
            //var ins1 = ServiceLocator.Current.GetInstance<ITest1>("No1");
            var ins3 = ServiceLocator.Current.GetInstance<ITest1>("No2");

            Assert.IsTrue(ins3.Count == 1);
        }
    }

    public interface ITest1 {
        int Count { get; }
    }

    public class Test1 : ITest1 {
        public int Count { get; set; } = 1;
    }
}

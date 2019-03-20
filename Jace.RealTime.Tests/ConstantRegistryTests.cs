using System;
using Jace.RealTime.Compilation;
using Jace.RealTime.Execution;


using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jace.RealTime.Tests
{
    [TestClass]
    public class ConstantRegistryTests
    {
        [TestMethod]
        public void TestAddConstant()
        {
            ConstantRegistry registry = new ConstantRegistry();
            
            registry.RegisterConstant("test", 42.0f);

            ConstantInfo functionInfo = registry.GetConstantInfo("test");
            
            Assert.IsNotNull(functionInfo);
            Assert.AreEqual("test", functionInfo.ConstantName);
            Assert.AreEqual(42.0f, functionInfo.Value);
        }

        [TestMethod]
        public void TestOverwritable()
        {
            ConstantRegistry registry = new ConstantRegistry();

            registry.RegisterConstant("test", 42.0f);
            registry.RegisterConstant("test", 26.3f);
        }

        [TestMethod]
        public void TestNotOverwritable()
        {
            ConstantRegistry registry = new ConstantRegistry();

            registry.RegisterConstant("test", 42.0f, false);

            Assert.ThrowsException<Exception>(() =>
                {
                    registry.RegisterConstant("test", 26.3f, false);
                });
        }
    }
}

using System;
using Jace.RealTime.Compilation;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jace.RealTime.Tests
{
    [TestClass]
    public class FunctionRegistryTests
    {
        [TestMethod]
        public void TestAddFunc2()
        {
            FunctionRegistry registry = new FunctionRegistry();
            
            Func<float, float, float> testFunction = (a, b) => a * b;
            registry.RegisterFunction("test", testFunction);

            FunctionInfo functionInfo = registry.GetFunctionInfo("test");
            
            Assert.IsNotNull(functionInfo);
            Assert.AreEqual("test", functionInfo.FunctionName);
            Assert.AreEqual(2, functionInfo.NumberOfParameters);
            Assert.AreEqual(testFunction, functionInfo.Function);

            
        }
    }
}

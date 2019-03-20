using System.Collections.Generic;
using Jace.RealTime.Operations;
using Jace.RealTime.Execution;
using Jace.RealTime.Tests.Mocks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jace.RealTime.Compilation;

namespace Jace.RealTime.Tests
{
    [TestClass]
    public class BasicInterpreterTests
    {
        [TestMethod]
        public void TestBasicInterpreterSubstraction()
        {
            IFunctionRegistry functionRegistry = new MockFunctionRegistry();

            IExecutor executor = new Interpreter();
            float result = executor.Execute(new Substraction(
                new FloatingPointConstant(6),
                new FloatingPointConstant(9)), functionRegistry);

            Assert.AreEqual(-3.0f, result);
        }

        [TestMethod]
        public void TestBasicInterpreter1()
        {
            IFunctionRegistry functionRegistry = new MockFunctionRegistry();

            IExecutor executor = new Interpreter();
            // 6 + (2 * 4)
            float result = executor.Execute(
                new Addition(
                    new FloatingPointConstant(6),
                    new Multiplication(
                        new FloatingPointConstant(2), 
                        new FloatingPointConstant(4))), functionRegistry);

            Assert.AreEqual(14.0f, result);
        }

        [TestMethod]
        public void TestBasicInterpreterWithVariables()
        {
            IFunctionRegistry functionRegistry = new MockFunctionRegistry();

            Dictionary<string, float> variables = new Dictionary<string, float>();
            variables.Add("var1", 2);
            variables.Add("age", 4);

            IExecutor interpreter = new Interpreter();
            // var1 + 2 * (3 * age)
            float result = interpreter.Execute(
                new Addition(
                    new Variable("var1"),
                    new Multiplication(
                        new FloatingPointConstant(2),
                        new Multiplication(
                            new FloatingPointConstant(3),
                            new Variable("age")))), functionRegistry, variables);

            Assert.AreEqual(26.0f, result);
        }
    }
}
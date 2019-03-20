using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Jace.RealTime.Execution;
using Jace.RealTime.Maths;
using Jace.RealTime.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jace.RealTime.Tests
{
    [TestClass]
    public class CalculationEngineTests
    {
        [TestMethod]
        public void TestCalculationFormula1FloatingPointCompiled()
        {
            CalculationEngine engine = new CalculationEngine();

            Func<float> f = engine
                .Formula("2.0+3.0")
                .Build();

            float result = f();
            Assert.AreEqual(5.0f, result);
        }

        [TestMethod]
        public void TestCalculateFormula1()
        {
            CalculationEngine engine = new CalculationEngine();

            Func<float> f = engine
                .Formula("2+3")
                .Build();

            float result = f();
            Assert.AreEqual(5.0f, result);
        }

        [TestMethod]
        public void TestCalculateModuloCompiled()
        {
            CalculationEngine engine = new CalculationEngine();

            Func<float> f = engine
                .Formula("5 % 3.0")
                .Build();

            float result = f();
            Assert.AreEqual(2.0f, result);
        }

        [TestMethod]
        public void TestCalculatePowCompiled()
        {
            CalculationEngine engine = new CalculationEngine();

            Func<float> f = engine
                .Formula("2^3.0")
                .Build();

            float result = f();
            Assert.AreEqual(8.0f, result);
        }

        [TestMethod]
        public void TestCalculateFormulaWithVariables()
        {
            CalculationEngine engine = new CalculationEngine();

            Func<float, float, float> f = engine
                .Formula("var1*var2")
                .Parameter("var1")
                .Parameter("var2")
                .Build();

            float result = f(2.5f, 3.4f);
            Assert.AreEqual(8.5f, result);
        }

        [TestMethod]
        public void TestCalculateFormulaVariableNotDefinedCompiled()
        {
            CalculationEngine engine = new CalculationEngine();

            Dictionary<string, float> variables = new Dictionary<string, float>();
            variables.Add("var1", 2.5f);

            Assert.ThrowsException<VariableNotDefinedException>(() =>
            {
                Func<float, float> f = engine
                    .Formula("var1*var2")
                    .Parameter("var1")
                    .Build();
            });
        }

        [TestMethod]
        public void TestCalculateSineFunctionCompiled()
        {
            CalculationEngine engine = new CalculationEngine();

            Func<float> f = engine
                .Formula("sin(14)")
                .Build();

            float result = f();
            Assert.AreEqual(Mathf.Sin(14.0f), result);
        }

        [TestMethod]
        public void TestCalculateCosineFunctionCompiled()
        {
            CalculationEngine engine = new CalculationEngine();

            Func<float> f = engine
                .Formula("cos(41)")
                .Build();

            float result = f();
            Assert.AreEqual(Mathf.Cos(41.0f), result);
        }

        [TestMethod]
        public void TestCalculateLognFunctionCompiled()
        {
            CalculationEngine engine = new CalculationEngine();

            Func<float> f = engine
                .Formula("logn(14, 3)")
                .Build();

            float result = f();
            Assert.AreEqual(Mathf.Logn(14.0f, 3.0f), result);
        }

        [TestMethod]
        public void TestNegativeConstant()
        {
            CalculationEngine engine = new CalculationEngine();

            Func<float> f = engine
                .Formula("-100")
                .Build();

            float result = f();
            Assert.AreEqual(-100.0f, result);
        }

        [TestMethod]
        public void TestMultiplicationWithNegativeConstant()
        {
            CalculationEngine engine = new CalculationEngine();

            Func<float> f = engine
                .Formula("5*-100")
                .Build();

            float result = f();
            Assert.AreEqual(-500.0f, result);
        }

        [TestMethod]
        public void TestUnaryMinus1Compiled()
        {
            CalculationEngine engine = new CalculationEngine();

            Func<float> f = engine
                .Formula("-(1+2+(3+4))")
                .Build();

            float result = f();
            Assert.AreEqual(-10.0f, result);
        }

        [TestMethod]
        public void TestUnaryMinus2Compiled()
        {
            CalculationEngine engine = new CalculationEngine();

            Func<float> f = engine
                .Formula("5+(-(1*2))")
                .Build();

            float result = f();
            Assert.AreEqual(3.0f, result);
        }

        [TestMethod]
        public void TestUnaryMinus3Compiled()
        {
            CalculationEngine engine = new CalculationEngine();

            Func<float> f = engine
                .Formula("5*(-(1*2)*3)")
                .Build();

            float result = f();
            Assert.AreEqual(-30.0f, result);
        }

        [TestMethod]
        public void TestUnaryMinus4Compiled()
        {
            CalculationEngine engine = new CalculationEngine();

            Func<float> f = engine
                .Formula("5* -(1*2)")
                .Build();

            float result = f();
            Assert.AreEqual(-10.0f, result);
        }

        [TestMethod]
        public void TestUnaryMinus5Compiled()
        {
            CalculationEngine engine = new CalculationEngine();

            Func<float> f = engine
                .Formula("-(1*2)^3")
                .Build();

            float result = f();
            Assert.AreEqual(-8.0f, result);
        }

        [TestMethod]
        public void TestPiMultiplication()
        {
            CalculationEngine engine = new CalculationEngine();

            Func<float> f = engine
                .Formula("2 * pi")
                .Build();

            float result = f();
            Assert.AreEqual(2 * Mathf.PI, result);
        }

        [TestMethod]
        public void TestReservedVariableName()
        {
            CalculationEngine engine = new CalculationEngine();

            Assert.ThrowsException<ArgumentException>(() =>
            {
                Func<float, float> f = engine
                    .Formula("2 * pI")
                    .Parameter("pi")
                    .Build();
                
                float result = f(18.0f);
            });
        }

        [TestMethod]
        public void TestVariableNameCaseSensitivityNoToLowerCompiled()
        {
            CalculationEngine engine = new CalculationEngine();

            Func<float, float> f = engine
                    .Formula("2 * BlAbLa")
                    .Parameter("BlAbLa")
                    .Build();

            float result = f(42.5f);
            Assert.AreEqual(85.0f, result);
        }

        [TestMethod]
        public void TestCustomFunctionCompiled()
        {
            CalculationEngine engine = new CalculationEngine();

            engine.AddFunction("test", (a, b) => a + b);

            Func<float> f = engine
                .Formula("test(2, 3)")
                .Build();

            float result = f();
            Assert.AreEqual(5.0f, result);
        }

        [TestMethod]
        public void TestComplicatedPrecedence1()
        {
            CalculationEngine engine = new CalculationEngine();

            Func<float> f = engine
                .Formula("1+2-3*4/5+6-7*8/9+0")
                .Build();

            float result = f();
            Assert.AreEqual(0.378f, Mathf.Round(result, 3));
        }

        [TestMethod]
        public void TestComplicatedPrecedence2()
        {
            CalculationEngine engine = new CalculationEngine();

            Func<float> f = engine
                .Formula("1+2-3*4/sqrt(25)+6-7*8/9+0")
                .Build();

            float result = f();
            Assert.AreEqual(0.378f, Mathf.Round(result, 3));
        }

        [TestMethod]
        public void TestExpressionArguments1()
        {
            CalculationEngine engine = new CalculationEngine();

            Func<float> f = engine
                .Formula("ifless(0.57, (3000-500)/(1500-500), 10, 20)")
                .Build();

            float result = f();
            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void TestExpressionArguments2()
        {
            CalculationEngine engine = new CalculationEngine();

            Func<float> f = engine
                .Formula("if(0.57 < (3000-500)/(1500-500), 10, 20)")
                .Build();

            float result = f();
            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void TestLessThanCompiled()
        {
            CalculationEngine engine = new CalculationEngine();

            Func<float, float, float> f = engine
                .Formula("var1 < var2")
                .Parameter("var1")
                .Parameter("var2")
                .Build();

            float result = f(2, 4);
            Assert.AreEqual(1.0f, result);
        }

        [TestMethod]
        public void TestLessOrEqualThan1Compiled()
        {
            CalculationEngine engine = new CalculationEngine();

            Func<float, float, float> f = engine
                .Formula("var1 <= var2")
                .Parameter("var1")
                .Parameter("var2")
                .Build();

            float result = f(2, 2);
            Assert.AreEqual(1.0f, result);
        }

        [TestMethod]
        public void TestLessOrEqualThan2Compiled()
        {
            CalculationEngine engine = new CalculationEngine();

            Func<float, float, float> f = engine
                .Formula("var1 ≤ var2")
                .Parameter("var1")
                .Parameter("var2")
                .Build();

            float result = f(2, 2);
            Assert.AreEqual(1.0f, result);
        }

        [TestMethod]
        public void TestGreaterThan1Compiled()
        {
            CalculationEngine engine = new CalculationEngine();

            Func<float, float, float> f = engine
                .Formula("var1 > var2")
                .Parameter("var1")
                .Parameter("var2")
                .Build();

            float result = f(2, 3);
            Assert.AreEqual(0.0f, result);
        }

        [TestMethod]
        public void TestGreaterOrEqualThan1Compiled()
        {
            CalculationEngine engine = new CalculationEngine();

            Func<float, float, float> f = engine
                .Formula("var1 >= var2")
                .Parameter("var1")
                .Parameter("var2")
                .Build();

            float result = f(2, 2);
            Assert.AreEqual(1.0f, result);
        }

        [TestMethod]
        public void TestNotEqual2Compiled()
        {
            CalculationEngine engine = new CalculationEngine();

            Func<float, float, float> f = engine
                .Formula("var1 ≠ 2")
                .Parameter("var1")
                .Parameter("var2")
                .Build();

            float result = f(2, 2);
            Assert.AreEqual(0.0f, result);
        }

        [TestMethod]
        public void TestEqualCompiled()
        {
            CalculationEngine engine = new CalculationEngine();

            Func<float, float, float> f = engine
                .Formula("var1 == 2")
                .Parameter("var1")
                .Parameter("var2")
                .Build();

            float result = f(2, 2);
            Assert.AreEqual(1.0f, result);
        }

        [TestMethod]
        public void TestVariableUnderscoreCompiled()
        {
            CalculationEngine engine = new CalculationEngine();

            Func<float, float, float> f = engine
                .Formula("var_var_1 + var_var_2")
                .Parameter("var_var_1")
                .Parameter("var_var_2")
                .Build();

            float result = f(1, 2);
            Assert.AreEqual(3.0f, result);
        }

        [TestMethod]
        public void TestCustomFunctionFunc11Compiled()
        {
            CalculationEngine engine = new CalculationEngine();
            engine.AddFunction("test", (a, b, c, d, e, f, g, h, i, j, k) => a + b + c + d + e + f + g + h + i + j + k);

            Func<float> func = engine
                .Formula("test(1,2,3,4,5,6,7,8,9,10,11)")
                .Build();

            float result = func();
            float expected = (11 * (11 + 1)) / 2.0f;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestAndCompiled()
        {
            CalculationEngine engine = new CalculationEngine();

            Func<float> f = engine
                .Formula("(1 && 0)")
                .Build();

            float result = f();
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void TestOr1Compiled()
        {
            CalculationEngine engine = new CalculationEngine();

            Func<float> f = engine
                .Formula("(1 || 0)")
                .Build();

            float result = f();
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void TestOr2Compiled()
        {
            CalculationEngine engine = new CalculationEngine();

            Func<float> f = engine
                .Formula("(0 || 0)")
                .Build();

            float result = f();
            Assert.AreEqual(0, result);
        }
    }
}
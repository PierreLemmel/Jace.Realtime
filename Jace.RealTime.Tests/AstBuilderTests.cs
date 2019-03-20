using System.Collections.Generic;
using System.Linq;
using Jace.RealTime.Operations;
using Jace.RealTime.Execution;
using Jace.RealTime.Tests.Mocks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jace.RealTime.Compilation;
using Jace.RealTime.Parsing;

namespace Jace.RealTime.Tests
{
    [TestClass]
    public class AstBuilderTests
    {
        [TestMethod]
        public void TestBuildFormula1()
        {
            IFunctionRegistry registry = new MockFunctionRegistry();

            AstBuilder builder = new AstBuilder(registry);
            Operation operation = builder.Build(new List<Token>() { 
                new Token() { Value = '(', TokenType = TokenType.LeftBracket }, 
                new Token() { Value = 42.0f, TokenType = TokenType.FloatingPoint }, 
                new Token() { Value = '+', TokenType = TokenType.Operation }, 
                new Token() { Value = 8.0f, TokenType = TokenType.FloatingPoint }, 
                new Token() { Value = ')', TokenType = TokenType.RightBracket }, 
                new Token() { Value = '*', TokenType = TokenType.Operation }, 
                new Token() { Value = 2.0f, TokenType = TokenType.FloatingPoint } 
            });

            Multiplication multiplication = (Multiplication)operation;
            Addition addition = (Addition)multiplication.Argument1;

            Assert.AreEqual(42, ((FloatingPointConstant)addition.Argument1).Value);
            Assert.AreEqual(8, ((FloatingPointConstant)addition.Argument2).Value);
            Assert.AreEqual(2, ((FloatingPointConstant)multiplication.Argument2).Value);
        }

        [TestMethod]
        public void TestBuildFormula2()
        {
            IFunctionRegistry registry = new MockFunctionRegistry();

            AstBuilder builder = new AstBuilder(registry);
            Operation operation = builder.Build(new List<Token>() {
                new Token() { Value = 2.0f, TokenType = TokenType.FloatingPoint }, 
                new Token() { Value = '+', TokenType = TokenType.Operation }, 
                new Token() { Value = 8.0f, TokenType = TokenType.FloatingPoint }, 
                new Token() { Value = '*', TokenType = TokenType.Operation }, 
                new Token() { Value = 3.0f, TokenType = TokenType.FloatingPoint } 
            });

            Addition addition = (Addition)operation;
            Multiplication multiplication = (Multiplication)addition.Argument2;

            Assert.AreEqual(2, ((FloatingPointConstant)addition.Argument1).Value);
            Assert.AreEqual(8, ((FloatingPointConstant)multiplication.Argument1).Value);
            Assert.AreEqual(3, ((FloatingPointConstant)multiplication.Argument2).Value);
        }

        [TestMethod]
        public void TestBuildFormula3()
        {
            IFunctionRegistry registry = new MockFunctionRegistry();

            AstBuilder builder = new AstBuilder(registry);
            Operation operation = builder.Build(new List<Token>() {
                new Token() { Value = 2.0f, TokenType = TokenType.FloatingPoint }, 
                new Token() { Value = '*', TokenType = TokenType.Operation }, 
                new Token() { Value = 8.0f, TokenType = TokenType.FloatingPoint }, 
                new Token() { Value = '-', TokenType = TokenType.Operation }, 
                new Token() { Value = 3.0f, TokenType = TokenType.FloatingPoint }
            });

            Substraction substraction = (Substraction)operation;
            Multiplication multiplication = (Multiplication)substraction.Argument1;

            Assert.AreEqual(3, ((FloatingPointConstant)substraction.Argument2).Value);
            Assert.AreEqual(2, ((FloatingPointConstant)multiplication.Argument1).Value);
            Assert.AreEqual(8, ((FloatingPointConstant)multiplication.Argument2).Value);
        }

        [TestMethod]
        public void TestDivision()
        {
            IFunctionRegistry registry = new MockFunctionRegistry();

            AstBuilder builder = new AstBuilder(registry);
            Operation operation = builder.Build(new List<Token>() { 
                new Token() { Value = 10.0f, TokenType = TokenType.FloatingPoint }, 
                new Token() { Value = '/', TokenType = TokenType.Operation }, 
                new Token() { Value = 2.0f, TokenType = TokenType.FloatingPoint }
            });

            Assert.AreEqual(typeof(Division), operation.GetType());

            Division division = (Division)operation;

            Assert.AreEqual(new FloatingPointConstant(10), division.Dividend);
            Assert.AreEqual(new FloatingPointConstant(2), division.Divisor);
        }

        [TestMethod]
        public void TestMultiplication()
        {
            IFunctionRegistry registry = new MockFunctionRegistry();

            AstBuilder builder = new AstBuilder(registry);
            Operation operation = builder.Build(new List<Token>() { 
                new Token() { Value = 10.0f, TokenType = TokenType.FloatingPoint }, 
                new Token() { Value = '*', TokenType = TokenType.Operation }, 
                new Token() { Value = 2.0f, TokenType = TokenType.FloatingPoint }
            });

            Multiplication multiplication = (Multiplication)operation;

            Assert.AreEqual(new FloatingPointConstant(10), multiplication.Argument1);
            Assert.AreEqual(new FloatingPointConstant(2.0f), multiplication.Argument2);
        }

        [TestMethod]
        public void TestExponentiation()
        {
            IFunctionRegistry registry = new MockFunctionRegistry();

            AstBuilder builder = new AstBuilder(registry);
            Operation operation = builder.Build(new List<Token>() { 
                new Token() { Value = 2.0f, TokenType = TokenType.FloatingPoint }, 
                new Token() { Value = '^', TokenType = TokenType.Operation }, 
                new Token() { Value = 3.0f, TokenType = TokenType.FloatingPoint }
            });

            Exponentiation exponentiation = (Exponentiation)operation;

            Assert.AreEqual(new FloatingPointConstant(2), exponentiation.Base);
            Assert.AreEqual(new FloatingPointConstant(3), exponentiation.Exponent);
        }

        [TestMethod]
        public void TestModulo()
        {
            IFunctionRegistry registry = new MockFunctionRegistry();

            AstBuilder builder = new AstBuilder(registry);
            Operation operation = builder.Build(new List<Token>() { 
                new Token() { Value = 2.7f, TokenType = TokenType.FloatingPoint }, 
                new Token() { Value = '%', TokenType = TokenType.Operation }, 
                new Token() { Value = 3.0f, TokenType = TokenType.FloatingPoint }
            });

            Modulo modulo = (Modulo)operation;

            Assert.AreEqual(new FloatingPointConstant(2.7f), modulo.Dividend);
            Assert.AreEqual(new FloatingPointConstant(3), modulo.Divisor);
        }

        [TestMethod]
        public void TestVariable()
        {
            IFunctionRegistry registry = new MockFunctionRegistry();

            AstBuilder builder = new AstBuilder(registry);
            Operation operation = builder.Build(new List<Token>() { 
                new Token() { Value = 10.0f, TokenType = TokenType.FloatingPoint }, 
                new Token() { Value = '*', TokenType = TokenType.Operation }, 
                new Token() { Value = "var1", TokenType = TokenType.Text }
            });

            Multiplication multiplication = (Multiplication)operation;

            Assert.AreEqual(new FloatingPointConstant(10), multiplication.Argument1);
            Assert.AreEqual(new Variable("var1"), multiplication.Argument2);
        }

        [TestMethod]
        public void TestMultipleVariable()
        {
            IFunctionRegistry registry = new MockFunctionRegistry();

            AstBuilder builder = new AstBuilder(registry);
            Operation operation = builder.Build(new List<Token>() { 
                new Token() { Value = "var1", TokenType = TokenType.Text }, 
                new Token() { Value = '+', TokenType = TokenType.Operation }, 
                new Token() { Value = 2.0f, TokenType = TokenType.FloatingPoint }, 
                new Token() { Value = '*', TokenType = TokenType.Operation }, 
                new Token() { Value = '(', TokenType = TokenType.LeftBracket }, 
                new Token() { Value = 3.0f, TokenType = TokenType.FloatingPoint }, 
                new Token() { Value = '*', TokenType = TokenType.Operation }, 
                new Token() { Value = "age", TokenType = TokenType.Text }, 
                new Token() { Value = ')', TokenType = TokenType.RightBracket }
            });

            Addition addition = (Addition)operation;
            Multiplication multiplication1 = (Multiplication)addition.Argument2;
            Multiplication multiplication2 = (Multiplication)multiplication1.Argument2;

            Assert.AreEqual(new Variable("var1"), addition.Argument1);
            Assert.AreEqual(new FloatingPointConstant(2), multiplication1.Argument1);
            Assert.AreEqual(new FloatingPointConstant(3), multiplication2.Argument1);
            Assert.AreEqual(new Variable("age"), multiplication2.Argument2);
        }

        [TestMethod]
        public void TestSinFunction1()
        {
            IFunctionRegistry registry = new MockFunctionRegistry();

            AstBuilder builder = new AstBuilder(registry);
            Operation operation = builder.Build(new List<Token>() { 
                new Token() { Value = "sin", TokenType = TokenType.Text }, 
                new Token() { Value = '(', TokenType = TokenType.LeftBracket }, 
                new Token() { Value = 2.0f, TokenType = TokenType.FloatingPoint }, 
                new Token() { Value = ')', TokenType = TokenType.RightBracket }
            });

            Function sineFunction = (Function)operation;
            Assert.AreEqual(new FloatingPointConstant(2), sineFunction.Arguments.Single());
        }

        [TestMethod]
        public void TestSinFunction2()
        {
            IFunctionRegistry registry = new MockFunctionRegistry();

            AstBuilder builder = new AstBuilder(registry);
            Operation operation = builder.Build(new List<Token>() { 
                new Token() { Value = "sin", TokenType = TokenType.Text }, 
                new Token() { Value = '(', TokenType = TokenType.LeftBracket }, 
                new Token() { Value = 2.0f, TokenType = TokenType.FloatingPoint }, 
                new Token() { Value = '+', TokenType = TokenType.Operation }, 
                new Token() { Value = 3.0f, TokenType = TokenType.FloatingPoint }, 
                new Token() { Value = ')', TokenType = TokenType.RightBracket }
            });

            Function sineFunction = (Function)operation;

            Addition addition = (Addition)sineFunction.Arguments.Single();
            Assert.AreEqual(new FloatingPointConstant(2), addition.Argument1);
            Assert.AreEqual(new FloatingPointConstant(3), addition.Argument2);
        }

        [TestMethod]
        public void TestSinFunction3()
        {
            IFunctionRegistry registry = new MockFunctionRegistry();

            AstBuilder builder = new AstBuilder(registry);
            Operation operation = builder.Build(new List<Token>() { 
                new Token() { Value = "sin", TokenType = TokenType.Text }, 
                new Token() { Value = '(', TokenType = TokenType.LeftBracket }, 
                new Token() { Value = 2.0f, TokenType = TokenType.FloatingPoint }, 
                new Token() { Value = '+', TokenType = TokenType.Operation }, 
                new Token() { Value = 3.0f, TokenType = TokenType.FloatingPoint }, 
                new Token() { Value = ')', TokenType = TokenType.RightBracket },
                new Token() { Value = '*', TokenType = TokenType.Operation },
                new Token() { Value = 4.9f, TokenType = TokenType.FloatingPoint }
            });

            Multiplication multiplication = (Multiplication)operation;

            Function sineFunction = (Function)multiplication.Argument1;

            Addition addition = (Addition)sineFunction.Arguments.Single();
            Assert.AreEqual(new FloatingPointConstant(2), addition.Argument1);
            Assert.AreEqual(new FloatingPointConstant(3), addition.Argument2);

            Assert.AreEqual(new FloatingPointConstant(4.9f), multiplication.Argument2);
        }

        [TestMethod]
        public void TestUnaryMinus()
        {
            IFunctionRegistry registry = new MockFunctionRegistry();

            AstBuilder builder = new AstBuilder(registry);
            Operation operation = builder.Build(new List<Token>() { 
                new Token() { Value = 5.3f, TokenType = TokenType.FloatingPoint }, 
                new Token() { Value = '*', TokenType = TokenType.Operation}, 
                new Token() { Value = '_', TokenType = TokenType.Operation }, 
                new Token() { Value = '(', TokenType = TokenType.LeftBracket }, 
                new Token() { Value = 5.0f, TokenType = TokenType.FloatingPoint }, 
                new Token() { Value = '+', TokenType = TokenType.Operation }, 
                new Token() { Value = 42.0f, TokenType = TokenType.FloatingPoint }, 
                new Token() { Value = ')', TokenType = TokenType.RightBracket }, 
            });

            Multiplication multiplication = (Multiplication)operation;
            Assert.AreEqual(new FloatingPointConstant(5.3f), multiplication.Argument1);

            UnaryMinus unaryMinus = (UnaryMinus)multiplication.Argument2;

            Addition addition = (Addition)unaryMinus.Argument;
            Assert.AreEqual(new FloatingPointConstant(5), addition.Argument1);
            Assert.AreEqual(new FloatingPointConstant(42), addition.Argument2);
        }

        [TestMethod]
        public void TestBuildInvalidFormula1()
        {
            IFunctionRegistry registry = new MockFunctionRegistry();

            AstBuilder builder = new AstBuilder(registry);

            Assert.ThrowsException<ParseException>(() =>
                {
                    Operation operation = builder.Build(new List<Token>() { 
                        new Token() { Value = '(', TokenType = TokenType.LeftBracket, StartPosition = 0 }, 
                        new Token() { Value = 42.0f, TokenType = TokenType.FloatingPoint, StartPosition = 1 }, 
                        new Token() { Value = '+', TokenType = TokenType.Operation, StartPosition = 3 }, 
                        new Token() { Value = 8.0f, TokenType = TokenType.FloatingPoint, StartPosition = 4 }, 
                        new Token() { Value = ')', TokenType = TokenType.RightBracket, StartPosition = 5 }, 
                        new Token() { Value = '*', TokenType = TokenType.Operation, StartPosition = 6 }, 
                    });
                });
        }

        [TestMethod]
        public void TestBuildInvalidFormula2()
        {
            IFunctionRegistry registry = new MockFunctionRegistry();

            AstBuilder builder = new AstBuilder(registry);

            Assert.ThrowsException<ParseException>(() =>
                {
                    Operation operation = builder.Build(new List<Token>() { 
                        new Token() { Value = 42.0f, TokenType = TokenType.FloatingPoint, StartPosition = 0 }, 
                        new Token() { Value = '+', TokenType = TokenType.Operation, StartPosition = 2 }, 
                        new Token() { Value = 8.0f, TokenType = TokenType.FloatingPoint, StartPosition = 3 }, 
                        new Token() { Value = ')', TokenType = TokenType.RightBracket, StartPosition = 4 }, 
                        new Token() { Value = '*', TokenType = TokenType.Operation, StartPosition = 5 }, 
                        new Token() { Value = 2.0f, TokenType = TokenType.FloatingPoint, StartPosition = 6 }, 
                    });
                });
        }

        [TestMethod]
        public void TestBuildInvalidFormula3()
        {
            IFunctionRegistry registry = new MockFunctionRegistry();

            AstBuilder builder = new AstBuilder(registry);

            Assert.ThrowsException<ParseException>(() =>
                {
                    Operation operation = builder.Build(new List<Token>() { 
                        new Token() { Value = '(', TokenType = TokenType.LeftBracket, StartPosition = 0 }, 
                        new Token() { Value = 42.0f, TokenType = TokenType.FloatingPoint, StartPosition = 1 }, 
                        new Token() { Value = '+', TokenType = TokenType.Operation, StartPosition = 3 }, 
                        new Token() { Value = 8.0f, TokenType = TokenType.FloatingPoint, StartPosition = 4 }
                    });
                });
        }

        [TestMethod]
        public void TestBuildInvalidFormula4()
        {
            IFunctionRegistry registry = new MockFunctionRegistry();

            AstBuilder builder = new AstBuilder(registry);

            Assert.ThrowsException<ParseException>(() =>
                {
                    Operation operation = builder.Build(new List<Token>() { 
                        new Token() { Value = 5.0f, TokenType = TokenType.FloatingPoint, StartPosition = 0 }, 
                        new Token() { Value = 42.0f, TokenType = TokenType.FloatingPoint, StartPosition = 1 }, 
                        new Token() { Value = '+', TokenType = TokenType.Operation, StartPosition = 3 }, 
                        new Token() { Value = 8.0f, TokenType = TokenType.FloatingPoint, StartPosition = 4 }
                    });
                });
        }

        [TestMethod]
        public void TestBuildInvalidFormula5()
        {
            IFunctionRegistry registry = new MockFunctionRegistry();

            AstBuilder builder = new AstBuilder(registry);

            Assert.ThrowsException<ParseException>(() =>
                {
                    Operation operation = builder.Build(new List<Token>() { 
                        new Token() { Value = 42.0f, TokenType = TokenType.FloatingPoint, StartPosition = 0 }, 
                        new Token() { Value = '+', TokenType = TokenType.Operation, StartPosition = 2 }, 
                        new Token() { Value = 8.0f, TokenType = TokenType.FloatingPoint, StartPosition = 3 },
                        new Token() { Value = 5.0f, TokenType = TokenType.FloatingPoint, StartPosition = 4 } 
                    });
                });
        }
    }
}

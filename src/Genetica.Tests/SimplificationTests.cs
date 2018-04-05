// ------------------------------------------
// <copyright file="SimplificationTests.cs" company="Pedro Sequeira">
// 
//     Copyright (c) 2018 Pedro Sequeira
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to the following conditions:
//  
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS
// OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// </copyright>
// <summary>
//    Project: Genetica.Tests
//    Last updated: 04/02/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using Genetica.Elements;
using Genetica.Elements.Functions;
using Genetica.Elements.Terminals;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genetica.Tests
{
    [TestClass]
    public class SimplificationTests
    {
        #region Static Fields & Constants

        private static readonly KeyValuePair<string, string>[] EquivalentExpressions =
        {
            new KeyValuePair<string, string>("(((1.1+1)*x)+x)", "(x*3.1)"),
            new KeyValuePair<string, string>("(2+(x*x))", "((x^2)+2)"),
            new KeyValuePair<string, string>("(0?1:log(3,(1+0)):max(3,(cos(0)-(3/1))))", "1"),
            new KeyValuePair<string, string>("(x?1:log(3,(1+0)):max(3,(cos(0)-(3/1))))", "(x?1:NaN:3)"),
            new KeyValuePair<string, string>("(x?1:log(3,(1+0)):max(x,(cos(0)-(3/1))))", "(x?1:NaN:max(x,-2))"),
            new KeyValuePair<string, string>("((x*0)?(x-0):log(3,0):max(3,1))", "x"),
            new KeyValuePair<string, string>("(min((x*5),(((x^y)-0)^(5-y)))+22)", "(min((x*5),((x^y)^(5-y)))+22)")
        };

        #endregion

        #region Public Methods

        [TestMethod]
        public void AdditionByZeroTest()
        {
            GetConverter(out var varX, out _);
            var prog1 = new AdditionFunction(varX, Constant.Zero);
            var prog2 = varX;
            var simp = prog1.Simplify();
            Console.WriteLine($"{prog1}->{simp}, {prog2}");
            Assert.AreEqual(simp, prog2, $"{prog1} simplification ({simp}) should be equal to {prog2}");
        }

        [TestMethod]
        public void ChainedAdditionsTest()
        {
            var converter = GetConverter(out var varX, out _);
            var expression = "x";
            for (var i = 2; i <= 10; i++)
            {
                expression = $"({expression}+x)";
                var prog1 = converter.FromNormalNotation(expression);
                var prog2 = new MultiplicationFunction(new Constant(i), varX);
                var simp = prog1.Simplify();
                Console.WriteLine($"{prog1}->{simp}, {prog2}");
                Assert.AreEqual(simp, prog2, $"{prog1} simplification ({simp}) should be equal to {prog2}");
            }
        }

        [TestMethod]
        public void ChainedMultiplicationsTest()
        {
            var converter = GetConverter(out var varX, out _);
            var expression = "x";
            for (var i = 2; i <= 10; i++)
            {
                expression = $"({expression}*x)";
                var prog1 = converter.FromNormalNotation(expression);
                var prog2 = new PowerFunction(varX, new Constant(i));
                var simp = prog1.Simplify();
                Console.WriteLine($"{prog1}->{simp}, {prog2}");
                Assert.AreEqual(simp, prog2, $"{prog1} simplification ({simp}) should be equal to {prog2}");
            }
        }

        [TestMethod]
        public void ConstantTest()
        {
            var prog1 = new SubtractionFunction(new Constant(5), new Constant(2));
            var prog2 = new Constant(3);
            var simp = prog1.Simplify();
            Console.WriteLine($"{prog1}->{simp}, {prog2}");
            Assert.AreEqual(simp, prog2, $"{prog1} simplification ({simp}) should be equal to {prog2}");
        }

        [TestMethod]
        public void DivisionByOneTest()
        {
            GetConverter(out var varX, out _);
            var prog1 = new DivisionFunction(varX, Constant.One);
            var prog2 = varX;
            var simp = prog1.Simplify();
            Console.WriteLine($"{prog1}->{simp}, {prog2}");
            Assert.AreEqual(simp, prog2, $"{prog1} simplification ({simp}) should be equal to {prog2}");
        }

        [TestMethod]
        public void DivisionBySelfTest()
        {
            GetConverter(out var varX, out _);
            var prog1 = new DivisionFunction(varX, varX);
            var prog2 = Constant.One;
            var simp = prog1.Simplify();
            Console.WriteLine($"{prog1}->{simp}, {prog2}");
            Assert.AreEqual(simp, prog2, $"{prog1} simplification ({simp}) should be equal to {prog2}");
        }

        [TestMethod]
        public void IfConstantTest()
        {
            var converter = GetConverter(out var varX, out _);
            var prog1 = new IfFunction(converter.FromNormalNotation("(2-2)"), varX, Constant.One, Constant.One);
            var prog2 = varX;
            var simp = prog1.Simplify();
            Console.WriteLine($"{prog1}->{simp}, {prog2}");
            Assert.AreEqual(simp, prog2, $"{prog1} simplification ({simp}) should be equal to {prog2}");

            prog1 = new IfFunction(converter.FromNormalNotation("(2-1)"), Constant.One, varX, Constant.One);
            simp = prog1.Simplify();
            Console.WriteLine($"{prog1}->{simp}, {prog2}");
            Assert.AreEqual(simp, prog2, $"{prog1} simplification ({simp}) should be equal to {prog2}");

            prog1 = new IfFunction(converter.FromNormalNotation("(1-2)"), Constant.One, Constant.One, varX);
            simp = prog1.Simplify();
            Console.WriteLine($"{prog1}->{simp}, {prog2}");
            Assert.AreEqual(simp, prog2, $"{prog1} simplification ({simp}) should be equal to {prog2}");
        }

        [TestMethod]
        public void IfEqualChildrenTest()
        {
            GetConverter(out var varX, out var varY);
            var prog1 = new IfFunction(varY, varX, varX, varX);
            var prog2 = varX;
            var simp = prog1.Simplify();
            Console.WriteLine($"{prog1}->{simp}, {prog2}");
            Assert.AreEqual(simp, prog2, $"{prog1} simplification ({simp}) should be equal to {prog2}");
        }

        [TestMethod]
        public void IfRangeTest()
        {
            var varX = new Variable("x", 2, new Range(1, 4));
            var varY = new Variable("y", new Range(-3, -1));
            var varZ = new Variable("z", new Range(0));
            var prog1 = new IfFunction(varZ, varX, Constant.One, Constant.One);
            var prog2 = varX;
            var simp = prog1.Simplify();
            Console.WriteLine($"{prog1}->{simp}, {prog2}");
            Assert.AreEqual(simp, prog2, $"{prog1} simplification ({simp}) should be equal to {prog2}");

            prog1 = new IfFunction(varX, Constant.One, varY, Constant.One);
            simp = prog1.Simplify();
            prog2 = varY;
            Console.WriteLine($"{prog1}->{simp}, {prog2}");
            Assert.AreEqual(simp, prog2, $"{prog1} simplification ({simp}) should be equal to {prog2}");

            prog1 = new IfFunction(varY, Constant.One, Constant.One, varX);
            simp = prog1.Simplify();
            prog2 = varX;
            Console.WriteLine($"{prog1}->{simp}, {prog2}");
            Assert.AreEqual(simp, prog2, $"{prog1} simplification ({simp}) should be equal to {prog2}");
        }

        [TestMethod]
        public void MaxTest()
        {
            GetConverter(out var varX, out _);
            MathProgram prog1 = new MaxFunction(varX, new Constant(double.MinValue));
            var prog2 = varX;
            var simp = prog1.Simplify();
            Console.WriteLine($"{prog1}->{simp}, {prog2}");
            Assert.AreEqual(simp, prog2, $"{prog1} simplification ({simp}) should be equal to {prog2}");

            prog1 = new MaxFunction(varX, new Constant(double.NegativeInfinity));
            simp = prog1.Simplify();
            Console.WriteLine($"{prog1}->{simp}, {prog2}");
            Assert.AreEqual(simp, prog2, $"{prog1} simplification ({simp}) should be equal to {prog2}");
        }

        [TestMethod]
        public void MinMaxWithSelfTest()
        {
            GetConverter(out var varX, out _);
            MathProgram prog1 = new MaxFunction(varX, varX);
            var prog2 = varX;
            var simp = prog1.Simplify();
            Console.WriteLine($"{prog1}->{simp}, {prog2}");
            Assert.AreEqual(simp, prog2, $"{prog1} simplification ({simp}) should be equal to {prog2}");

            prog1 = new MinFunction(varX, varX);
            simp = prog1.Simplify();
            Console.WriteLine($"{prog1}->{simp}, {prog2}");
            Assert.AreEqual(simp, prog2, $"{prog1} simplification ({simp}) should be equal to {prog2}");
        }

        [TestMethod]
        public void MinTest()
        {
            GetConverter(out var varX, out _);
            MathProgram prog1 = new MinFunction(varX, new Constant(double.MaxValue));
            var prog2 = varX;
            var simp = prog1.Simplify();
            Console.WriteLine($"{prog1}->{simp}, {prog2}");
            Assert.AreEqual(simp, prog2, $"{prog1} simplification ({simp}) should be equal to {prog2}");

            prog1 = new MinFunction(varX, new Constant(double.PositiveInfinity));
            simp = prog1.Simplify();
            Console.WriteLine($"{prog1}->{simp}, {prog2}");
            Assert.AreEqual(simp, prog2, $"{prog1} simplification ({simp}) should be equal to {prog2}");
        }

        [TestMethod]
        public void MultiplicationByOneTest()
        {
            GetConverter(out var varX, out _);
            var prog1 = new MultiplicationFunction(Constant.One, varX);
            var prog2 = varX;
            var simp = prog1.Simplify();
            Console.WriteLine($"{prog1}->{simp}, {prog2}");
            Assert.AreEqual(simp, prog2, $"{prog1} simplification ({simp}) should be equal to {prog2}");
        }

        [TestMethod]
        public void MultiplicationByZeroTest()
        {
            GetConverter(out var varX, out _);
            var prog1 = new MultiplicationFunction(varX, Constant.Zero);
            var prog2 = Constant.Zero;
            var simp = prog1.Simplify();
            Console.WriteLine($"{prog1}->{simp}, {prog2}");
            Assert.AreEqual(simp, prog2, $"{prog1} simplification ({simp}) should be equal to {prog2}");
        }

        [TestMethod]
        public void PowerByOneTest()
        {
            GetConverter(out var varX, out _);
            var prog1 = new PowerFunction(varX, Constant.One);
            var prog2 = varX;
            var simp = prog1.Simplify();
            Console.WriteLine($"{prog1}->{simp}, {prog2}");
            Assert.AreEqual(simp, prog2, $"{prog1} simplification ({simp}) should be equal to {prog2}");
        }

        [TestMethod]
        public void SelfSubtractionTest()
        {
            GetConverter(out var varX, out _);
            var prog1 = new SubtractionFunction(varX, varX);
            var prog2 = Constant.Zero;
            var simp = prog1.Simplify();
            Console.WriteLine($"{prog1}->{simp}, {prog2}");
            Assert.AreEqual(simp, prog2, $"{prog1} simplification ({simp}) should be equal to {prog2}");
        }

        [TestMethod]
        public void SimplicationsEqualTest()
        {
            var converter = GetConverter(out _, out _);
            foreach (var kvp in EquivalentExpressions)
            {
                var prog1 = converter.FromNormalNotation(kvp.Key);
                var prog2 = converter.FromNormalNotation(kvp.Value);
                var simp = prog1.Simplify();
                Console.WriteLine($"{prog1}->{simp}, {prog2}");
                Assert.AreEqual(simp, prog2, $"{prog1} simplification ({simp}) should be equal to {prog2}");
            }
        }

        [TestMethod]
        public void SimplicationsValueEqualTest()
        {
            var converter = GetConverter(out _, out _);
            foreach (var kvp in EquivalentExpressions)
            {
                var prog1 = converter.FromNormalNotation(kvp.Key);
                var prog2 = converter.FromNormalNotation(kvp.Value);
                var simp = prog1.Simplify();
                Console.WriteLine($"{prog1}->{simp}, {prog2}");
                Assert.IsTrue(prog2.IsValueEquivalent(simp),
                    $"{prog1} simplification ({simp}) should be value-equivalent to {prog2}");
            }
        }

        [TestMethod]
        public void SubtractionByZeroTest()
        {
            GetConverter(out var varX, out _);
            var prog1 = new SubtractionFunction(varX, Constant.Zero);
            var prog2 = varX;
            var simp = prog1.Simplify();
            Console.WriteLine($"{prog1}->{simp}, {prog2}");
            Assert.AreEqual(simp, prog2, $"{prog1} simplification ({simp}) should be equal to {prog2}");
        }

        [TestMethod]
        public void ZeroDivisionTest()
        {
            GetConverter(out var varX, out _);
            var prog1 = new DivisionFunction(Constant.Zero, varX);
            var prog2 = Constant.Zero;
            var simp = prog1.Simplify();
            Console.WriteLine($"{prog1}->{simp}, {prog2}");
            Assert.AreEqual(simp, prog2, $"{prog1} simplification ({simp}) should be equal to {prog2}");
        }

        #endregion

        #region Private & Protected Methods

        private static MathExpressionConverter GetConverter(out Variable varX, out Variable varY)
        {
            varX = new Variable("x", 2, new Range(-1, 1));
            varY = new Variable("y", new Range(1, 4));
            var primitiveSet = new PrimitiveSet<MathProgram>(new List<Terminal> {varX, varY}, new MathProgram[0]);
            primitiveSet.Add(MathPrimitiveSets.Default);
            return new MathExpressionConverter(primitiveSet);
        }

        #endregion
    }
}
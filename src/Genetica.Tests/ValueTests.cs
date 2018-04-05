// ------------------------------------------
// <copyright file="ValueTests.cs" company="Pedro Sequeira">
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
//    Last updated: 03/27/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using Genetica.Elements;
using Genetica.Elements.Terminals;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genetica.Tests
{
    [TestClass]
    public class ValueTests
    {
        #region Static Fields & Constants

        private static readonly KeyValuePair<string, string>[] EquivalentExpressions =
        {
            new KeyValuePair<string, string>("(cos(max(3,1))+3)", "(3+cos(max(3,1)))"),
            new KeyValuePair<string, string>("(x + x)", "(2 * x)"),
            new KeyValuePair<string, string>("(sin(0) + x)", $"(x + cos(({Math.PI} * 0.5)))"),
            new KeyValuePair<string, string>("(sin(x) ^ 2)", "(1 - (cos(x) ^ 2))"),
            new KeyValuePair<string, string>("sin((2*x))", "(sin(x)*(2*cos(x)))"),
            new KeyValuePair<string, string>("((cos(x) + sin(x))^2)", $"(2 * (cos((({Math.PI} / 4) - x)) ^ 2))"),
            new KeyValuePair<string, string>("sin((x - y))", "((sin(x) * cos(y)) - (cos(x) * sin(y)))")
        };

        #endregion

        #region Public Methods

        [TestMethod]
        public void ConstantEqualValueTest()
        {
            var converter = GetConverter(out _, out _);
            var prog1 = converter.FromNormalNotation("4");
            var prog2 = converter.FromNormalNotation("(1*(2+(1*2)))");
            var val1 = prog1.Compute();
            var val2 = prog2.Compute();
            Console.WriteLine($"{prog1}:{val1}, {prog2}:{val2}");
            Assert.AreEqual(val1, val2, double.Epsilon,
                $"Output of {prog1}({val1}) should be equivalent to the one of {prog2}({val2})");
        }

        [TestMethod]
        public void CorrectValueTest()
        {
            var converter = GetConverter(out _, out _);
            var prog1 = converter.FromNormalNotation("(cos(max(3,1))+3)");
            var val1 = prog1.Compute();
            var val2 = Math.Cos(3) + 3;
            Console.WriteLine($"{prog1}:{val1}, {val2}");
            Assert.AreEqual(val1, val2, double.Epsilon, $"Output of {prog1}({val1}) should be equivalent to {val2}");
        }

        [TestMethod]
        public void IsValueEquivalentTest()
        {
            var converter = GetConverter(out _, out _);
            foreach (var kvp in EquivalentExpressions)
            {
                var prog1 = converter.FromNormalNotation(kvp.Key);
                var prog2 = converter.FromNormalNotation(kvp.Value);
                Console.WriteLine($"{prog1}, {prog2}");
                Assert.IsTrue(prog1.IsValueEquivalent(prog2), $"{prog1} should be value-equivalent to {prog2}");
            }
        }

        [TestMethod]
        public void VariableValueTest()
        {
            var converter = GetConverter(out var varX, out _);
            for (var i = 0; i < 10; i++)
            {
                var prog1 = converter.FromNormalNotation("cos((max(3,1)+x))");
                varX.Value = i;
                var val1 = prog1.Compute();
                var val2 = Math.Cos(3 + i);
                Console.WriteLine($"{prog1}, x={varX.Value}:{val1}, {val2}");
                Assert.AreEqual(val1, val2, double.Epsilon,
                    $"Output of {prog1}({val1}) should be equivalent to {val2}");
            }
        }

        #endregion

        #region Private & Protected Methods

        private static MathExpressionConverter GetConverter(out Variable varX, out Variable varY)
        {
            varX = new Variable("x", 2, Range.Default);
            varY = new Variable("y", Range.Default);
            var primitiveSet = new PrimitiveSet<MathProgram>(new List<Terminal> {varX, varY}, new MathProgram[0]);
            primitiveSet.Add(MathPrimitiveSets.Default);
            return new MathExpressionConverter(primitiveSet);
        }

        #endregion
    }
}
// ------------------------------------------
// <copyright file="RangeTests.cs" company="Pedro Sequeira">
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
//    Project: Genesis.Tests
//    Last updated: 03/27/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using Genesis.Elements;
using Genesis.Elements.Functions;
using Genesis.Elements.Terminals;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genesis.Tests
{
    [TestClass]
    public class RangeTests
    {
        #region Public Methods

        [TestMethod]
        public void ConstantRangeTest()
        {
            const double val = 4;
            var prog = new Constant(val);
            var range = new Range(val);
            Console.WriteLine($"{prog}:{prog.GetRange()}\n{range}");
            Assert.AreEqual(prog.GetRange(), range, $"{prog.GetRange()} should be equal to {range}");
            Assert.AreEqual(range.Interval, 0, double.Epsilon, $"Range interval {range.Interval} should be 0.");
        }

        [TestMethod]
        public void HashcodeTest()
        {
            const double val = 4;
            var range = new Range(val);
            Console.WriteLine(range);
            Assert.AreNotEqual(range.GetHashCode(), 0, double.Epsilon, "Range hashcode should not be 0.");
        }

        [TestMethod]
        public void MinMaxRangeTest()
        {
            var converter = GetConverter(out var xVar, out var yVar);
            var prog1 = new MaxFunction(xVar, yVar);
            var prog2 = new MinFunction(xVar, yVar);
            Console.WriteLine($"{prog1}:{prog1.GetRange()}\n{prog2}:{prog2.GetRange()}");
            Assert.AreEqual(prog1.GetRange(), yVar.Range, $"{prog1.GetRange()} should be equal to {yVar.Range}");
            Assert.AreEqual(prog2.GetRange(), xVar.Range, $"{prog2.GetRange()} should be equal to {xVar.Range}");
        }

        [TestMethod]
        public void NoIntervalTest()
        {
            const double val = 4;
            var range = new Range(val);
            Console.WriteLine(range);
            Assert.AreEqual(range.Min, range.Max, double.Epsilon,
                $"Range min {range.Min} does not equal range max {range.Max}.");
            Assert.AreEqual(range.Max, val, double.Epsilon, $"Range max {range.Max} does not equal {val}.");
            Assert.AreEqual(range.Interval, 0, double.Epsilon, $"Range interval {range.Interval} should be 0.");
        }

        [TestMethod]
        public void ProgramRangeTest()
        {
            var converter = GetConverter(out var xVar, out var yVar);
            var prog = converter.FromPrefixNotation("(+ x (cos (max 1 x)))");
            var range = prog.GetRange();
            Console.WriteLine($"{prog}:{range}");
            xVar.Value = xVar.Range.Min;
            var progVal = prog.Compute();
            Assert.AreEqual(range.Min, progVal, $"Prog range min {range.Min} should be equal to {progVal}");
            xVar.Value = xVar.Range.Max;
            progVal = prog.Compute();
            Assert.AreEqual(range.Max, progVal, $"Prog range max {range.Max} should be equal to {progVal}");
        }

        [TestMethod]
        public void RangeElementsTest()
        {
            const double min = -3;
            const double max = 5;
            var range = new Range(min, max);
            Console.WriteLine(range);
            Assert.AreEqual(range.Min, min, double.Epsilon, $"Range min {range.Min} does not equal {min}");
            Assert.AreEqual(range.Max, max, double.Epsilon, $"Range max {range.Max} does not equal {max}");
            Assert.AreEqual(range.Interval, max - min, double.Epsilon,
                $"Range interval {range.Interval} does not equal {max - min}");
        }

        #endregion

        #region Private & Protected Methods

        private static MathExpressionConverter GetConverter(out Variable xVar, out Variable yVar)
        {
            xVar = new Variable("x", new Range(-1, 1));
            yVar = new Variable("y", new Range(2, 4));
            var primitiveSet = new PrimitiveSet<MathProgram>(
                new List<Terminal> {xVar, yVar}, new MathProgram[0]);
            primitiveSet.Add(MathPrimitiveSets.Default);
            var converter = new MathExpressionConverter(primitiveSet);
            return converter;
        }

        #endregion
    }
}
// ------------------------------------------
// <copyright file="ElementsTests.cs" company="Pedro Sequeira">
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
//    Last updated: 03/28/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Genetica.Elements;
using Genetica.Elements.Functions;
using Genetica.Elements.Terminals;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genetica.Tests
{
    [TestClass]
    public class ElementsTests
    {
        #region Static Fields & Constants

        private static readonly MathExpressionConverter Converter =
            new MathExpressionConverter(MathPrimitiveSets.Default);

        #endregion

        #region Public Methods

        [TestMethod]
        public void CommutativeFunctionTest()
        {
            ITreeProgram<double> a = new Variable("a");
            ITreeProgram<double> b = new Variable("b");
            var commFunctions = new List<CommutativeBinaryFunction>
                                {
                                    new AdditionFunction(a, a),
                                    new MultiplicationFunction(a, a),
                                    new MinFunction(a, a),
                                    new MaxFunction(a, a)
                                };
            var children1 = new[] {a, b};
            var children2 = new[] {b, a};
            foreach (var commFunction in commFunctions)
            {
                var addition1 = commFunction.CreateNew(children1);
                var addition2 = commFunction.CreateNew(children2);
                Console.WriteLine($"{addition1}, {addition2}");
                Assert.AreEqual(addition1, addition2, $"Functions should be equal: {addition1}, {addition2}");
            }
        }

        [TestMethod]
        public void ConstantEqualsTest()
        {
            var prog = Converter.FromNormalNotation("(cos(((1+2)/2))?10:2:(2+3))");
            Console.WriteLine(prog);
            var val = prog.Compute();
            Assert.IsTrue(prog.EqualsConstant(val), $"{prog} should have a constant value of {val}.");
        }

        [TestMethod]
        public void ConstantIsConstantTest()
        {
            var prog = new Constant(3);
            Console.WriteLine(prog);
            Assert.IsTrue(prog.IsConstant(), $"{prog} should be constant.");
        }

        [TestMethod]
        public void CreateNewTest()
        {
            var prog1 = Converter.FromNormalNotation("(cos(((1+2)/2))?10:2:(2+3))");
            var prog2 = prog1.CreateNew(prog1.Input.ToList());
            Console.WriteLine($"{prog1}, {prog2}");
            Assert.AreEqual(prog1, prog2, $"{prog1} should be equal to {prog2}.");
        }

        [TestMethod]
        public void ProgramAt0Test()
        {
            const string expression = "(cos (- 3 2))";
            var prog = Converter.FromPrefixNotation(expression);
            var prog0 = prog.ProgramAt(0);
            Console.WriteLine($"{prog}:{prog0}");
            Assert.AreEqual(prog, prog0, $"Program at 0 of {prog} should be {prog}.");
        }

        [TestMethod]
        public void ProgramAt2Test()
        {
            const string expression = "(cos (- 3 2))";
            var prog = Converter.FromPrefixNotation(expression);
            var subProg = prog.ProgramAt(2);
            Console.WriteLine($"{prog}:{subProg}");
            Assert.AreEqual(new Constant(3), subProg, $"Program at 2 of {prog} should be 3.");
        }

        [TestMethod]
        public void ProgramAtLengthTest()
        {
            const string expression = "(cos (- 3 2))";
            var prog = Converter.FromPrefixNotation(expression);
            var subProg = prog.ProgramAt(prog.Length);
            Console.WriteLine($"{prog}:{subProg}");
            Assert.IsNull(subProg, $"Program at {prog.Length} of {prog} should be null.");
        }

        [TestMethod]
        public void ProgramBreadthTest()
        {
            const string expression = "(cos (- 3 2))";
            var prog = Converter.FromPrefixNotation(expression);
            Console.WriteLine(prog);
            const int breadth = 2;
            Assert.AreEqual(prog.GetMaxBreadth(), breadth, double.Epsilon, $"Breadth of {prog} should be {breadth}.");
        }

        [TestMethod]
        public void ProgramDepthTest()
        {
            const string expression = "(cos (- 3 2))";
            var prog = Converter.FromPrefixNotation(expression);
            Console.WriteLine(prog);
            const int depth = 2;
            Assert.AreEqual(prog.GetMaxDepth(), depth, double.Epsilon, $"Depth of {prog} should be {depth}.");
        }

        [TestMethod]
        public void ProgramIsConstantTest()
        {
            var prog = Converter.FromNormalNotation("(cos(((1+2)/2))?10:2:(2+3))");
            Console.WriteLine(prog);
            Assert.IsTrue(prog.IsConstant(), $"{prog} should be constant.");
        }

        [TestMethod]
        public void ProgramIsNotConstantTest()
        {
            var prog = new MultiplicationFunction(new Constant(3), new Variable("a"));
            Console.WriteLine(prog);
            Assert.IsFalse(prog.IsConstant(), $"{prog} should not be constant.");
        }

        [TestMethod]
        public void ProgramLengthTest()
        {
            const string expression = "(cos (- 3 2))";
            var prog = Converter.FromPrefixNotation(expression);
            Console.WriteLine(prog);
            const int size = 4;
            Assert.AreEqual(prog.Length, size, double.Epsilon, $"Length of {prog} should be {size}.");
        }

        #endregion
    }
}
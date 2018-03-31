// ------------------------------------------
// <copyright file="SubProgramsTests.cs" company="Pedro Sequeira">
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
//    Last updated: 03/28/2018
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
    public class SubProgramsTests
    {
        #region Static Fields & Constants

        private static readonly MathExpressionConverter Converter =
            new MathExpressionConverter(MathPrimitiveSets.Default);

        #endregion

        #region Public Methods

        [TestMethod]
        public void IsSubCombinationTest()
        {
            var prog1 = Converter.FromNormalNotation("(2/4)");
            var prog2 = Converter.FromNormalNotation("((2+1)/(4*3))");
            Console.WriteLine($"{prog1}, {prog2}");
            Assert.IsTrue(prog2.GetSubCombinations().Contains(prog1));
        }

        [TestMethod]
        public void IsSubProgramTest()
        {
            var prog1 = Converter.FromNormalNotation("cos(max(3, 1))");
            var prog2 = Converter.FromNormalNotation("(cos(max(3,1))+3)");
            Console.WriteLine($"{prog1}, {prog2}");
            Assert.IsTrue(prog1.IsSubProgramOf(prog2));
        }

        [TestMethod]
        public void ProgramIsNotSubCombinationTest()
        {
            var program = CreateProgram();
            Assert.IsFalse(program.GetSubCombinations().Contains(program),
                $"{program} should not be sub-combination of {program}");
        }

        [TestMethod]
        public void ProgramIsNotSubProgramTest()
        {
            var program = CreateProgram();
            Assert.IsFalse(program.IsSubProgramOf(program), $"{program} should not be sub-program of {program}");
        }

        [TestMethod]
        public void SubCombinationsSizeTest()
        {
            var program = CreateProgram();
            var subCombs = program.GetSubCombinations();
            const double size = 30;
            Assert.AreEqual(subCombs.Count, size, double.Epsilon,
                $"Wrong number of sub-combinations, should be {size}");
        }

        [TestMethod]
        public void SubProgramsIsSubCombinationTest()
        {
            var program = CreateProgram();
            var subPrograms = program.GetSubPrograms();
            var subCombs = program.GetSubCombinations();
            foreach (var subProgram in subPrograms)
            {
                Console.WriteLine(subProgram);
                Assert.IsTrue(subCombs.Contains(subProgram), $"{subProgram} should be sub-combination of {program}");
            }
        }

        [TestMethod]
        public void SubProgramsIsSubProgramTest()
        {
            var program = CreateProgram();
            var subPrograms = program.GetSubPrograms();
            foreach (var subProgram in subPrograms)
            {
                Console.WriteLine(subProgram);
                Assert.IsTrue(subProgram.IsSubProgramOf(program), $"{subProgram} should be sub-program of {program}");
            }
        }

        [TestMethod]
        public void SubProgramsSizeEqualsProgramLengthTest()
        {
            var program = CreateProgram();
            var subPrograms = program.GetSubPrograms();
            Assert.AreEqual(subPrograms.Length, program.Length - 1, double.Epsilon,
                $"Sub-programs count {subPrograms.Length} should be equal to program.Length-1: {program.Length - 1}");
        }

        [TestMethod]
        public void SubProgramsSizeTest()
        {
            var program = CreateProgram();
            var subPrograms = program.GetSubPrograms();
            const double size = 8;
            Assert.AreEqual(subPrograms.Length, size, double.Epsilon,
                $"Wrong number of sub-programs, should be {size}");
            Assert.AreEqual(subPrograms.Length, program.Length - 1, double.Epsilon,
                $"Wrong number of sub-programs, should be {size}");
        }

        #endregion

        #region Private & Protected Methods

        private static MathProgram CreateProgram()
        {
            var a = new Variable("a");
            var b = new Variable("b");
            var c = new Variable("c");
            var d = new Variable("d");
            var e = new Variable("e");
            var addition = new AdditionFunction(a, a);
            var subtr = new SubtractionFunction(a, a);

            var primitives = new PrimitiveSet<MathProgram>(
                new HashSet<Terminal> {a, b, c, d, e},
                new HashSet<MathProgram> {addition, subtr});

            var converter = new MathExpressionConverter(primitives);
            return converter.FromPrefixNotation("(+ (- a b) (- c (+ d e)))");
        }

        #endregion
    }
}
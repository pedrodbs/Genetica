// ------------------------------------------
// <copyright file="CrossoverTests.cs" company="Pedro Sequeira">
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
//    Last updated: 03/29/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using Genesis.Elements;
using Genesis.Operators.Crossover;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genesis.Tests
{
    [TestClass]
    public class CrossoverTests
    {
        #region Static Fields & Constants

        private static readonly List<ICrossoverOperator<MathProgram>> Operators =
            new List<ICrossoverOperator<MathProgram>>
            {
                new SubtreeCrossover<MathProgram, double>(),
                new OnePointCrossover<MathProgram, double>(),
                new UniformCrossover<MathProgram, double>(),
                new ContextPreservingCrossover<MathProgram, double>()
            };

        #endregion

        #region Public Methods

        [TestMethod]
        public void AllOnePointInAllUniformCrossoverTest()
        {
            var converter = new MathExpressionConverter(MathPrimitiveSets.Default);
            var prog1 = converter.FromPrefixNotation("(- (+ 2 1) 3)");
            var prog2 = converter.FromPrefixNotation("(/ (+ (cos 3) 1) 2)");
            Console.WriteLine($"{prog1}, {prog2}");
            var allOnePointProgs =
                new HashSet<MathProgram>(new OnePointCrossover<MathProgram, double>().GetAllOffspring(prog1, prog2));
            var allUniformProgs =
                new HashSet<MathProgram>(new UniformCrossover<MathProgram, double>().GetAllOffspring(prog1, prog2));
            Assert.IsTrue(allOnePointProgs.IsSubsetOf(allUniformProgs),
                $"All one-point crossovers between {prog1} and {prog2} should be included in all uniform crossovers.");
        }

        [TestMethod]
        public void CrossoverInAllCrossoversTest()
        {
            var converter = new MathExpressionConverter(MathPrimitiveSets.Default);
            var prog1 = converter.FromPrefixNotation("(- (+ 2 1) 3)");
            var prog2 = converter.FromPrefixNotation("(/ (+ (cos 3) 1) 2)");
            Console.WriteLine($"{prog1}, {prog2}");
            foreach (var op in Operators)
            {
                var allProgs = new HashSet<MathProgram>(op.GetAllOffspring(prog1, prog2));
                var prog = op.Crossover(prog1, prog2);
                Assert.IsTrue(allProgs.Contains(prog),
                    $"{prog} should be a valid crossover between {prog1} and {prog2}.");
            }
        }

        [TestMethod]
        public void CrossoversInAllStochasticCrossoverTest()
        {
            var converter = new MathExpressionConverter(MathPrimitiveSets.Default);
            var cross = new StochasticCrossover<MathProgram>(Operators);
            var prog1 = converter.FromPrefixNotation("(- (+ 2 1) 3)");
            var prog2 = converter.FromPrefixNotation("(/ (+ (cos 3) 1) 2)");
            var all = new HashSet<MathProgram>(cross.GetAllOffspring(prog1, prog2));
            Console.WriteLine($"{prog1}, {prog2}");
            foreach (var op in Operators)
            {
                var prog = op.Crossover(prog1, prog2);
                Console.WriteLine(prog);
                Assert.IsTrue(all.Contains(prog), $"{prog} should be a valid crossover between {prog1} and {prog2}.");
            }
        }

        [TestMethod]
        public void EqualCrossoversTest()
        {
            var converter = new MathExpressionConverter(MathPrimitiveSets.Default);
            var prog1 = converter.FromNormalNotation("((2+3)-1)");
            var prog2 = converter.FromNormalNotation("((3+2)-1)");
            Console.WriteLine($"{prog1}, {prog2}");
            foreach (var op in Operators)
            {
                var allProgs = new HashSet<MathProgram>(op.GetAllOffspring(prog1, prog2));
                Assert.IsTrue(allProgs.Contains(prog1),
                    $"{prog1} should be a valid crossover between {prog1} and {prog2}.");
                Assert.IsTrue(allProgs.Contains(prog2),
                    $"{prog1} should be a valid crossover between {prog1} and {prog2}.");
                Assert.AreEqual(allProgs.Count, 1, double.Epsilon,
                    $"Crossover between {prog1} and {prog2} should only have 1 element.");
            }
        }

        [TestMethod]
        public void LeafSubTreeCrossoverTest()
        {
            var converter = new MathExpressionConverter(MathPrimitiveSets.Default);
            var prog1 = converter.FromNormalNotation("0");
            var prog2 = converter.FromNormalNotation("((2+3)-1)");
            Console.WriteLine($"{prog1}, {prog2}");
            var op = new SubtreeCrossover<MathProgram, double>();
            var allProgs = new HashSet<MathProgram>(op.GetAllOffspring(prog1, prog2));
            var prog = op.Crossover(prog1, prog2);
            Assert.IsTrue(allProgs.Contains(prog2),
                $"{prog} should be a valid crossover between {prog1} and {prog2}.");
            Assert.IsTrue(prog.Equals(prog2),
                $"{prog} should be the only valid crossover between {prog1} and {prog2}.");
            Assert.AreEqual(allProgs.Count, 1, double.Epsilon,
                $"Crossover between {prog1} and {prog2} should only have 1 element.");
        }

        [TestMethod]
        public void NoCommonRegionCrossoverTest()
        {
            var converter = new MathExpressionConverter(MathPrimitiveSets.Default);
            var prog1 = converter.FromNormalNotation("sin((1-2))");
            var prog2 = converter.FromNormalNotation("((2+3)-1)");
            Console.WriteLine($"{prog1}, {prog2}");
            var op = new OnePointCrossover<MathProgram, double>();
            var allProgs = new HashSet<MathProgram>(op.GetAllOffspring(prog1, prog2));
            var prog = op.Crossover(prog1, prog2);
            Assert.IsTrue(allProgs.Contains(prog2),
                $"{prog} should be a valid crossover between {prog1} and {prog2}.");
            Assert.IsTrue(prog.Equals(prog2),
                $"{prog} should be the only valid crossover between {prog1} and {prog2}.");
            Assert.AreEqual(allProgs.Count, 1, double.Epsilon,
                $"Crossover between {prog1} and {prog2} should only have 1 element.");
        }

        [TestMethod]
        public void NoContextCrossoverTest()
        {
            var converter = new MathExpressionConverter(MathPrimitiveSets.Default);
            var prog1 = converter.FromNormalNotation("0");
            var prog2 = converter.FromNormalNotation("((2+3)-1)");
            Console.WriteLine($"{prog1}, {prog2}");
            var op = new ContextPreservingCrossover<MathProgram, double>();
            var allProgs = new HashSet<MathProgram>(op.GetAllOffspring(prog1, prog2));
            var prog = op.Crossover(prog1, prog2);
            Assert.IsTrue(allProgs.Contains(prog2),
                $"{prog} should be a valid crossover between {prog1} and {prog2}.");
            Assert.IsTrue(prog.Equals(prog2),
                $"{prog} should be the only valid crossover between {prog1} and {prog2}.");
            Assert.AreEqual(allProgs.Count, 1, double.Epsilon,
                $"Crossover between {prog1} and {prog2} should only have 1 element.");
        }

        [TestMethod]
        public void NullParent1CrossoversTest()
        {
            var converter = new MathExpressionConverter(MathPrimitiveSets.Default);
            foreach (var op in Operators)
            {
                var prog1 = converter.FromNormalNotation("log(2,4)");
                var allProgs = new HashSet<MathProgram>(op.GetAllOffspring(prog1, null));
                var prog = op.Crossover(prog1, null);
                Assert.IsNotNull(allProgs, "Crossover list of null should not be null.");
                Assert.AreEqual(allProgs.Count, 0, double.Epsilon, "Crossover list of null should be empty.");
                Assert.IsNull(prog, "Crossover of null should be null.");
            }
        }

        [TestMethod]
        public void NullParent2CrossoversTest()
        {
            var converter = new MathExpressionConverter(MathPrimitiveSets.Default);
            foreach (var op in Operators)
            {
                var prog2 = converter.FromNormalNotation("log(2,4)");
                var allProgs = new HashSet<MathProgram>(op.GetAllOffspring(null, prog2));
                var prog = op.Crossover(null, prog2);
                Assert.IsNotNull(allProgs, "Crossover list of null should not be null.");
                Assert.AreEqual(allProgs.Count, 0, double.Epsilon, "Crossover list of null should be empty.");
                Assert.IsNull(prog, "Crossover of null should be null.");
            }
        }

        [TestMethod]
        public void Parent2InAllCrossoversTest()
        {
            var converter = new MathExpressionConverter(MathPrimitiveSets.Default);
            var prog1 = converter.FromPrefixNotation("(- (+ 2 1) 3)");
            var prog2 = converter.FromPrefixNotation("(/ (+ (cos 3) 1) 2)");
            Console.WriteLine($"{prog1}, {prog2}");
            foreach (var op in Operators)
            {
                var allProgs = new HashSet<MathProgram>(op.GetAllOffspring(prog1, prog2));
                Assert.IsTrue(allProgs.Contains(prog2),
                    $"{prog2} should be a valid crossover between {prog1} and {prog2}.");
            }
        }

        #endregion
    }
}
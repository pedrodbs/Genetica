// ------------------------------------------
// <copyright file="MutationTests.cs" company="Pedro Sequeira">
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
using Genesis.Elements.Terminals;
using Genesis.Operators.Mutation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genesis.Tests
{
    [TestClass]
    public class MutationTests
    {
        #region Static Fields & Constants

        private static readonly PrimitiveSet<MathProgram> ExtraPrimitiveSet =
            new PrimitiveSet<MathProgram>(new List<Terminal> {new Constant(4)}, MathPrimitiveSets.Default.Functions);

        private static readonly List<IMutationOperator<MathProgram>> Operators =
            new List<IMutationOperator<MathProgram>>
            {
                new HoistMutation<MathProgram, double>(),
                new SimplifyMutation(),
                new SwapMutation<MathProgram, double>(),
                new PointMutation<MathProgram, double>(ExtraPrimitiveSet)

                //new SubtreeMutation<MathProgram, double>(null, ExtraPrimitiveSet, 0),
            };

        #endregion

        #region Public Methods

        [TestMethod]
        public void MutationInAllMutationsTest()
        {
            var converter = new MathExpressionConverter(MathPrimitiveSets.Default);
            var prog1 = converter.FromPrefixNotation("(- (/ 2 1) 3)");
            Console.WriteLine(prog1);
            foreach (var op in Operators)
            {
                var allProgs = new HashSet<MathProgram>(op.GetAllMutations(prog1));
                Console.WriteLine("====================");
                foreach (var mut in allProgs) Console.WriteLine($"\t{mut}");
                var prog = op.Mutate(prog1);
                Assert.IsTrue(allProgs.Contains(prog), $"{prog} should be a valid mutation of {prog1}.");
            }
        }

        [TestMethod]
        public void MutationsInAllStochasticMutationTest()
        {
            var converter = new MathExpressionConverter(MathPrimitiveSets.Default);
            var mut = new StochasticMutation<MathProgram>(Operators);
            var prog1 = converter.FromPrefixNotation("(* 2 (^ 1 4))");
            var allMuts = new HashSet<MathProgram>(mut.GetAllMutations(prog1));
            Console.WriteLine(prog1);
            foreach (var op in Operators)
            {
                var prog = op.Mutate(prog1);
                Console.WriteLine(prog);
                Assert.IsTrue(allMuts.Contains(prog), $"{prog} should be a valid mutation of {prog1}.");
            }
        }

        [TestMethod]
        public void NullMutationsTest()
        {
            foreach (var op in Operators)
            {
                var allProgs = new HashSet<MathProgram>(op.GetAllMutations(null));
                var prog = op.Mutate(null);
                Assert.IsNotNull(allProgs, "Mutation list of null should not be null.");
                Assert.AreEqual(allProgs.Count, 0, double.Epsilon, $"Mutation list of null should be empty.");
                Assert.IsNull(prog, "Mutation of null should be null.");
            }
        }

        [TestMethod]
        public void PointMutationTest()
        {
            var converter = new MathExpressionConverter(MathPrimitiveSets.Default);
            var prog1 = converter.FromNormalNotation("1");
            var prog2 = converter.FromNormalNotation("4");
            Console.WriteLine($"{prog1}, {prog2}");
            var op = new PointMutation<MathProgram, double>(ExtraPrimitiveSet);
            var allProgs = new HashSet<MathProgram>(op.GetAllMutations(prog1));
            Assert.IsTrue(allProgs.Contains(prog2), $"{prog2} should be a valid mutation of {prog1}.");
            Assert.AreEqual(allProgs.Count, 2, double.Epsilon, $"Mutation of {prog1} should only have 2 elements.");
        }

        [TestMethod]
        public void ProgInAllMutationsTest()
        {
            var converter = new MathExpressionConverter(MathPrimitiveSets.Default);
            var prog1 = converter.FromPrefixNotation("(+ 2 1)");
            Console.WriteLine(prog1);
            foreach (var op in Operators)
            {
                var allProgs = new HashSet<MathProgram>(op.GetAllMutations(prog1));
                Assert.IsTrue(allProgs.Contains(prog1), $"{prog1} should be a valid mutation of {prog1}.");
            }
        }

        [TestMethod]
        public void ShrinkMutationTest()
        {
            var converter = new MathExpressionConverter(MathPrimitiveSets.Default);
            var prog1 = converter.FromNormalNotation("((3*2)/2)");
            var prog2 = converter.FromNormalNotation("(4/2)");
            Console.WriteLine($"{prog1}, {prog2}");
            var op = new ShrinkMutation<MathProgram, double>(ExtraPrimitiveSet);
            var allProgs = new HashSet<MathProgram>(op.GetAllMutations(prog1));
            Assert.IsTrue(allProgs.Contains(prog2), $"{prog2} should be a valid mutation of {prog1}.");
        }

        [TestMethod]
        public void SimplifyMutationTest()
        {
            var converter = new MathExpressionConverter(MathPrimitiveSets.Default);
            var prog1 = converter.FromNormalNotation("((2/1)-3)");
            var prog2 = converter.FromNormalNotation("-1");
            Console.WriteLine($"{prog1}, {prog2}");
            var op = new SimplifyMutation();
            var allProgs = new HashSet<MathProgram>(op.GetAllMutations(prog1));
            foreach (var mut in allProgs) Console.WriteLine($"\t{mut}");
            Assert.IsTrue(allProgs.Contains(prog1), $"{prog2} should be a valid mutation of {prog1}.");
            Assert.IsTrue(allProgs.Contains(prog2), $"{prog2} should be a valid mutation of {prog1}.");
            Assert.AreEqual(allProgs.Count, 3, double.Epsilon, $"Mutation of {prog1} should only have 3 elements.");
        }

        #endregion
    }
}
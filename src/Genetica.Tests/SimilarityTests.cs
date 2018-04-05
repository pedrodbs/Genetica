// ------------------------------------------
// <copyright file="SimilarityTests.cs" company="Pedro Sequeira">
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
using Genetica.Elements;
using Genetica.Elements.Functions;
using Genetica.Elements.Terminals;
using Genetica.Similarity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genetica.Tests
{
    [TestClass]
    public class SimilarityTests
    {
        #region Static Fields & Constants

        private static readonly List<ISimilarityMeasure<MathProgram>> SimilarityMeasures =
            new List<ISimilarityMeasure<MathProgram>>
            {
                new LeafSimilarity<MathProgram, double>(),
                new CommonRegionSimilarity<MathProgram, double>(),
                new SubCombinationSimilarity<MathProgram, double>(),
                new SubProgramSimilarity<MathProgram, double>(),
                new PrefixNotationEditSimilarity(),
                new NormalNotationEditSimilarity(),
                new TreeEditSimilarity<MathProgram, double>(),
                new ValueSimilarity(),
                new PrimitiveSimilarity<MathProgram, double>()
            };

        #endregion

        #region Public Methods

        [TestMethod]
        public void CommonRegionSimilarityTest()
        {
            var converter = GetConverter();
            var prog1 = converter.FromNormalNotation("sin(((z+4)*(1+x)))");
            var prog2 = converter.FromNormalNotation("cos(((3^x)-(y/2)))");
            var similarity = new CommonRegionSimilarity<MathProgram, double>().Calculate(prog1, prog2);
            Console.WriteLine($"{prog1}, {prog2}: {similarity}");
            Assert.AreEqual(similarity, 1d, double.Epsilon, $"{prog1} similarity with {prog2} should be 1.");
        }

        [TestMethod]
        public void EqualsSimilarTest()
        {
            var converter = GetConverter();
            var prog1 = converter.FromNormalNotation("(((2+x)/y)^cos(5))");
            var prog2 = converter.FromNormalNotation(converter.ToNormalNotation(prog1));
            foreach (var measure in SimilarityMeasures)
            {
                var similarity = measure.Calculate(prog1, prog2);
                Console.WriteLine($"{prog1}, {prog2}: {similarity}");
                Assert.AreEqual(similarity, 1d, double.Epsilon, $"{prog1} similarity with {prog2} should be 1.");
            }
        }

        [TestMethod]
        public void LeafSimilarityTest()
        {
            var converter = GetConverter();
            var prog1 = converter.FromNormalNotation("(max((3/x),((1+(4/y))/y))-z)");
            var prog2 = converter.FromNormalNotation("((x^y)*((1*(4/z))^sin(3)))");
            var similarity = new LeafSimilarity<MathProgram, double>().Calculate(prog1, prog2);
            Console.WriteLine($"{prog1}, {prog2}: {similarity}");
            Assert.AreEqual(similarity, 1d, double.Epsilon, $"{prog1} similarity with {prog2} should be 1.");
        }

        [TestMethod]
        public void NotEqualsSimilarTest()
        {
            var converter = GetConverter();
            var prog1 = converter.FromNormalNotation("(((2+x)/y)^cos(5))");
            var prog2 = new AdditionFunction(prog1, Constant.One);
            foreach (var measure in SimilarityMeasures)
            {
                var similarity = measure.Calculate(prog1, prog2);
                Console.WriteLine($"{prog1}, {prog2}: {similarity}");
                Assert.AreNotEqual(similarity, 1d, 1E-10, $"{prog1} similarity with {prog2} should not be 1.");
                Assert.AreNotEqual(similarity, 0d, 1E-10, $"{prog1} similarity with {prog2} should not be 0.");
            }
        }

        [TestMethod]
        public void PrimitiveSimilarityTest()
        {
            var converter = GetConverter();
            var prog1 = converter.FromNormalNotation("(max((3/x),((1+(4/y))/y))-z)");
            var prog2 = converter.FromNormalNotation("((x+y)/((1-(4/z))/max(3,x)))");
            var similarity = new PrimitiveSimilarity<MathProgram, double>().Calculate(prog1, prog2);
            Console.WriteLine($"{prog1}, {prog2}: {similarity}");
            Assert.AreEqual(similarity, 1d, double.Epsilon, $"{prog1} similarity with {prog2} should be 1.");
        }

        [TestMethod]
        public void SubCombinationSimilarityTest()
        {
            var converter = GetConverter();
            var prog1 = converter.FromNormalNotation("((2+x)+y)");
            var prog2 = converter.FromNormalNotation("(x+(2+y))");
            var similarity = new SubCombinationSimilarity<MathProgram, double>().Calculate(prog1, prog2);
            Console.WriteLine(prog1);
            foreach (var subComb in prog1.GetSubCombinations()) Console.WriteLine($"\t{subComb}");
            Console.WriteLine(prog2);
            foreach (var subComb in prog2.GetSubCombinations()) Console.WriteLine($"\t{subComb}");
            Console.WriteLine($"Similar: {similarity}");
            Assert.AreEqual(similarity, 1d, double.Epsilon, $"{prog1} similarity with {prog2} should be 1.");
        }

        [TestMethod]
        public void SubProgramSimilarityTest()
        {
            var converter = GetConverter();
            var prog1 = converter.FromNormalNotation("((y/2)*(1+x))");
            var prog2 = converter.FromNormalNotation("log((1+x),(y/2))");
            var similarity = new SubProgramSimilarity<MathProgram, double>().Calculate(prog1, prog2);
            Console.WriteLine($"{prog1}, {prog2}: {similarity}");
            Assert.AreEqual(similarity, 1d, double.Epsilon, $"{prog1} similarity with {prog2} should be 1.");
        }

        [TestMethod]
        public void SymbolTreeSimilarityTest()
        {
            var converter = GetConverter();
            var prog1 = converter.FromNormalNotation("sin(((z+4)*(1+x)))");
            var prog2 = converter.FromNormalNotation("sin(((x+1)*(4+z)))");
            var similarity = new SymbolTreeSimilarity().Calculate(prog1, prog2);
            Console.WriteLine($"{prog1}, {prog2}: {similarity}");
            Assert.AreEqual(similarity, 1d, double.Epsilon, $"{prog1} similarity with {prog2} should be 1.");
        }

        [TestMethod]
        public void ValueSimilarityTest()
        {
            var converter = GetConverter();
            var prog1 = converter.FromNormalNotation("(sin(x) ^ 2)");
            var prog2 = converter.FromNormalNotation("(1 - (cos(x) ^ 2))");
            var similarity = new ValueSimilarity().Calculate(prog1, prog2);
            Console.WriteLine($"{prog1}, {prog2}: {similarity}");
            Assert.AreEqual(similarity, 1d, 1E-10, $"{prog1} similarity with {prog2} should be 1.");
        }

        #endregion

        #region Private & Protected Methods

        private static MathExpressionConverter GetConverter()
        {
            var varX = new Variable("x");
            var varY = new Variable("y");
            var varZ = new Variable("z");
            var primitiveSet = new PrimitiveSet<MathProgram>(
                new List<Terminal> {varX, varY, varZ}, MathPrimitiveSets.Default.Functions);
            return new MathExpressionConverter(primitiveSet);
        }

        #endregion
    }
}
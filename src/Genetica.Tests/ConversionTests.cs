// ------------------------------------------
// <copyright file="ConversionTests.cs" company="Pedro Sequeira">
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
using Genetica.Elements;
using Genetica.Elements.Terminals;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genetica.Tests
{
    [TestClass]
    public class ConversionTests
    {
        #region Static Fields & Constants

        private static readonly string[] PrefixExpressions =
        {
            "3",
            "(+ 1 0)",
            "(* 2 -1)",
            "(cos (- 3 2))",
            "(min 3 (- 2 1))",
            "(if 0 1 (log 3 (+ 1 0)) (max 3 (- (cos 0) (/ 3 1))))",
            "(+ (+ 4 3) (+ 1 0))"
        };

        private static readonly string[] NormalExpressions =
        {
            "3",
            "(2*-1)",
            "(3?1:(1+0):3)",
            "min(3,(2-1))",
            "(log((1/3),cos((3-1)))+(2?1:max(3,1):5))",
            "((4+3)+(1+0))",
            "(5^sin(6))"
        };

        private static readonly string[] IllegalPrefixExpressions =
        {
            "(+ 1 0 0)",
            "(cos 12 2)",
            "(1 / 2",
            "3 ^ 2",
            "a",
            "max(3, (2 - 1))",
            "(log 2)",
            "(0 + 1) + ((3 + 4))",
            "(0 + 1) + (3 + 4)",
            "(0 + 1 + (3 + 4))"
        };

        private static readonly MathExpressionConverter Converter =
            new MathExpressionConverter(MathPrimitiveSets.Default);

        #endregion

        #region Public Methods

        [TestMethod]
        public void ArgumentOrderTest()
        {
            const string expr1 = "(2+1)";
            const string expr2 = "(1+2)";
            var prog1 = Converter.FromNormalNotation(expr1);
            var prog2 = Converter.FromNormalNotation(expr2);
            Console.WriteLine($"{prog1}, {prog2}");
            Assert.AreEqual(prog1, prog2, $"{prog1} should be equal to {prog2}.");
        }

        [TestMethod]
        public void EmptyExpressionTest()
        {
            Assert.IsNull(Converter.FromPrefixNotation(""), "Empty program should result in null.");
        }

        [TestMethod]
        public void FromNormalEqualsPrefixTest()
        {
            foreach (var expression in PrefixExpressions)
            {
                var prog1 = Converter.FromPrefixNotation(expression);
                var normalExpr = Converter.ToNormalNotation(prog1);
                var prog2 = Converter.FromNormalNotation(normalExpr);
                Console.WriteLine($"{expression} -> {prog1} -> {normalExpr} -> {prog2}");
                Assert.AreEqual(prog1, prog2,
                    $"Program from expression {expression} should result in equal program from expression: {normalExpr}.");
            }
        }

        [TestMethod]
        public void FromPrefixEqualsNormalTest()
        {
            foreach (var expression in NormalExpressions)
            {
                var prog1 = Converter.FromNormalNotation(expression);
                var prefixExpr = Converter.ToPrefixNotation(prog1);
                var prog2 = Converter.FromPrefixNotation(prefixExpr);
                Console.WriteLine($"{expression} -> {prog1} -> {prefixExpr} -> {prog2}");
                Assert.AreEqual(prog1, prog2,
                    $"Program from expression {expression} should result in equal program from expression: {prefixExpr}.");
            }
        }

        [TestMethod]
        public void IllegalTest()
        {
            foreach (var expression in IllegalPrefixExpressions)
            {
                var prog = Converter.FromPrefixNotation(expression);
                Console.WriteLine(prog);
                Assert.IsNull(prog, $"Illegal expression {expression} should result in null.");
            }
        }

        [TestMethod]
        public void IsConstantTest()
        {
            const string expression = "3";
            Assert.IsInstanceOfType(Converter.FromPrefixNotation(expression), typeof(Constant),
                $"{expression} should result in a Constant.");
        }

        [TestMethod]
        public void NormalNotationEqualsTest()
        {
            foreach (var expression in NormalExpressions)
            {
                var prog = Converter.FromNormalNotation(expression);
                var normalExpr = Converter.ToNormalNotation(prog);
                Console.WriteLine($"{expression} -> {prog} -> {normalExpr}");
                Assert.AreEqual(normalExpr, expression,
                    $"Normal notation expression {expression} should result in equal expression: {normalExpr}.");
            }
        }

        [TestMethod]
        public void NormalNotationTest()
        {
            foreach (var expression in NormalExpressions)
            {
                var prog = Converter.FromNormalNotation(expression);
                Console.WriteLine(prog);
                Assert.IsNotNull(prog, $"Normal notation expression {expression} should not result in null.");
            }
        }

        [TestMethod]
        public void PrefixNotationEqualsTest()
        {
            foreach (var expression in PrefixExpressions)
            {
                var prog = Converter.FromPrefixNotation(expression);
                var prefixExpr = Converter.ToPrefixNotation(prog);
                Console.WriteLine($"{expression} -> {prog} -> {prefixExpr}");
                Assert.AreEqual(prefixExpr, expression,
                    $"Prefix notation expression {expression} should result in equal expression: {prefixExpr}.");
            }
        }

        [TestMethod]
        public void PrefixNotationTest()
        {
            foreach (var expression in PrefixExpressions)
            {
                var prog = Converter.FromPrefixNotation(expression);
                Console.WriteLine(prog);
                Assert.IsNotNull(prog, $"Prefix notation expression {expression} should not result in null.");
            }
        }

        [TestMethod]
        public void SpacesTest()
        {
            const string expression = "(2     +           1)";
            const string compressedExpr = "(2+1)";
            var prog = Converter.FromNormalNotation(expression);
            Console.WriteLine(prog);
            var normalNotation = Converter.ToNormalNotation(prog);
            Assert.AreEqual(normalNotation, compressedExpr,
                $"{normalNotation} should be equal to {compressedExpr}.");
        }

        #endregion
    }
}
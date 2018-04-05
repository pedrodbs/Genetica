// ------------------------------------------
// <copyright file="EqualityTests.cs" company="Pedro Sequeira">
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
using Genetica.Elements;
using Genetica.Elements.Functions;
using Genetica.Elements.Terminals;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genetica.Tests
{
    [TestClass]
    public class EqualityTests
    {
        #region Static Fields & Constants

        private static readonly MathExpressionConverter Converter =
            new MathExpressionConverter(MathPrimitiveSets.Default);

        #endregion

        #region Public Methods

        [TestMethod]
        public void EqualsButDiffRefsTestProgram()
        {
            var const1 = new Constant(0);
            var const2 = new Constant(1);
            var const3 = new Constant(3);
            var addition = new AdditionFunction(const1, const3);
            var subtr = new SubtractionFunction(const2, addition);
            var log1 = new LogarithmFunction(subtr, const1);
            var log2 = new LogarithmFunction(subtr, const1);

            Console.WriteLine($"{log1}, {log2}");
            Assert.AreEqual(log1, log2, $"{log1} should be equal to {log2}.");
            Assert.AreNotSame(log1, log2, $"{log1} should not be the same as {log2}.");
        }

        [TestMethod]
        public void HashDiffTestProgram()
        {
            var prog1 = Converter.FromNormalNotation("(1+2)");
            var prog2 = Converter.FromNormalNotation("(1*2)");
            var prog3 = Converter.FromNormalNotation("(2*2)");
            var hashCode1 = prog1.GetHashCode();
            var hashCode2 = prog2.GetHashCode();
            var hashCode3 = prog3.GetHashCode();

            Console.WriteLine($"{prog1}:{hashCode1}\n{prog2}:{hashCode2}\n{prog3}:{hashCode3}");
            Assert.AreNotEqual(hashCode1, hashCode2, double.Epsilon,
                $"Hash code of {prog1} ({hashCode1}) should not be equal to the one of {prog2} ({hashCode2}).");
            Assert.AreNotEqual(hashCode2, hashCode3, double.Epsilon,
                $"Hash code of {prog2} ({hashCode2}) should not be equal to the one of {prog3} ({hashCode3}).");
        }

        [TestMethod]
        public void HashEqualsTestProgram()
        {
            var min1 = Converter.FromNormalNotation("min(0,(1-(3+0)))");
            var min2 = Converter.FromNormalNotation("min(0,(1-(3+0)))");
            var min3 = Converter.FromNormalNotation("min((1-(3+0)),0)");
            var hashCode1 = min1.GetHashCode();
            var hashCode2 = min2.GetHashCode();
            var hashCode3 = min3.GetHashCode();

            Console.WriteLine($"{min1}:{hashCode1}\n{min2}:{hashCode2}\n{min3}:{hashCode3}");
            Assert.AreEqual(hashCode1, hashCode2, double.Epsilon,
                $"Hash code of {min1} ({hashCode1}) should be equal to the one of {min2} ({hashCode2}).");
            Assert.AreEqual(hashCode2, hashCode3, double.Epsilon,
                $"Hash code of {min2} ({hashCode2}) should be equal to the one of {min3} ({hashCode3}).");
        }

        [TestMethod]
        public void HashTestProgram()
        {
            var const1 = new Constant(0);
            var const2 = new Constant(1);
            var variable = new Variable("a");
            var sine = new SineFunction(const2);
            var pow = new PowerFunction(const1, variable);
            var subtr = new SubtractionFunction(const2, pow);
            var min = new MinFunction(subtr, const1);
            var div = new DivisionFunction(pow, variable);
            var prog = new IfFunction(div, min, subtr, sine);
            var hashCode = prog.GetHashCode();

            Console.WriteLine($"{prog}:{hashCode}");
            Assert.AreNotEqual(hashCode, 0, double.Epsilon, $"Hash code of {prog} should not be 0.");
        }

        #endregion
    }
}
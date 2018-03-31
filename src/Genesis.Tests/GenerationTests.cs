// ------------------------------------------
// <copyright file="GenerationTests.cs" company="Pedro Sequeira">
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
using System.Linq;
using Genesis.Elements;
using Genesis.Elements.Functions;
using Genesis.Elements.Terminals;
using Genesis.Operators.Generation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genesis.Tests
{
    [TestClass]
    public class GenerationTests
    {
        #region Public Methods

        [TestMethod]
        public void FullGeneratorDepthTest()
        {
            for (var maxDepth = 0u; maxDepth <= 4; maxDepth++)
            {
                var generator = new FullProgramGenerator<MathProgram, double>();
                for (var i = 0; i < 10; i++)
                {
                    var prog = generator.Generate(MathPrimitiveSets.Default, maxDepth);
                    var depth = prog.GetMaxDepth();
                    Console.WriteLine($"{prog}:{depth}");
                    Assert.AreEqual(depth, maxDepth, double.Epsilon, $"Depth of {prog} should be {maxDepth}.");
                }
            }
        }

        [TestMethod]
        public void GrowGeneratorDepthTest()
        {
            for (var maxDepth = 0u; maxDepth <= 4; maxDepth++)
            {
                var generator = new GrowProgramGenerator<MathProgram, double>();
                for (var i = 0; i < 20; i++)
                {
                    var prog = generator.Generate(MathPrimitiveSets.Default, maxDepth);
                    var depth = prog.GetMaxDepth();
                    Console.WriteLine($"{prog}:{depth}");
                    Assert.IsTrue(depth <= maxDepth, $"Depth of {prog} should be <= {maxDepth}.");
                }
            }
        }

        [TestMethod]
        public void RestrictedGenerationTest()
        {
            var generator = new FullProgramGenerator<MathProgram, double>();
            var primitiveSet =
                new PrimitiveSet<MathProgram>(new[] {Constant.Zero}, new[] {new CosineFunction(Constant.Zero)});
            var prog = generator.Generate(primitiveSet, 1);
            Console.WriteLine(prog);
            Assert.AreEqual(prog, primitiveSet.Functions.First(),
                $"The only program to be generated should be {primitiveSet.Functions.First()}.");
        }

        [TestMethod]
        public void StochasticGeneratorDepthTest()
        {
            for (var maxDepth = 0u; maxDepth <= 4; maxDepth++)
            {
                var generator = new StochasticProgramGenerator<MathProgram, double>(
                    new List<IProgramGenerator<MathProgram, double>>
                    {
                        new FullProgramGenerator<MathProgram, double>(),
                        new GrowProgramGenerator<MathProgram, double>()
                    });
                for (var i = 0; i < 20; i++)
                {
                    var prog = generator.Generate(MathPrimitiveSets.Default, maxDepth);
                    var depth = prog.GetMaxDepth();
                    Console.WriteLine($"{prog}:{depth}");
                    Assert.IsTrue(depth <= maxDepth, $"Depth of {prog} should be <= {maxDepth}.");
                }
            }
        }

        #endregion
    }
}
// ------------------------------------------
// <copyright file="PopulationTests.cs" company="Pedro Sequeira">
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
//    Last updated: 03/29/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using Genetica.Elements;
using Genetica.Elements.Terminals;
using Genetica.Evaluation;
using Genetica.Operators.Crossover;
using Genetica.Operators.Generation;
using Genetica.Operators.Mutation;
using Genetica.Operators.Selection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genetica.Tests
{
    [TestClass]
    public class PopulationTests
    {
        #region Static Fields & Constants

        private const int MAX_SIZE = 100;

        #endregion

        #region Public Methods

        [TestMethod]
        public void InitialBestProgramTest()
        {
            var pop = CreatePopulation();
            var prog = new Constant(MAX_SIZE - 1);
            Console.WriteLine(pop.BestProgram);
            Assert.AreEqual(pop.BestProgram, prog, $"Initial best program should be {prog}.");
        }

        [TestMethod]
        public void InitialPopulationSizeTest()
        {
            var pop = CreatePopulation();
            Console.WriteLine(pop.Count);
            Assert.AreEqual(pop.Count, MAX_SIZE, double.Epsilon, $"Initial population size should be {MAX_SIZE}.");
        }

        [TestMethod]
        public void SteadyStateTest()
        {
            for (var i = 0; i < 10; i++)
            {
                var pop = CreatePopulation();
                pop.Step();
                var prog = new Constant(MAX_SIZE - 1);
                Console.WriteLine(pop.BestProgram);
                Assert.AreEqual(pop.BestProgram, prog, $"Best program should be {prog}.");
            }
        }

        #endregion

        #region Private & Protected Methods

        private static Population<MathProgram, double> CreatePopulation()
        {
            var consts = new HashSet<Terminal>();
            for (var i = 0; i < MAX_SIZE; i++)
                consts.Add(new Constant(i));

            var primitives = new PrimitiveSet<MathProgram>(consts, new HashSet<MathProgram>());
            var fullGen = new FullProgramGenerator<MathProgram, double>();
            var fitnessFunction = new FitnessFunction();
            var pop = new Population<MathProgram, double>(
                MAX_SIZE, primitives, fullGen, fitnessFunction,
                new TournamentSelection<MathProgram>(fitnessFunction, 5),
                new OnePointCrossover<MathProgram, double>(),
                new SimplifyMutation(), 0, 20, 0.65, 0.2, 0.1);
            pop.Init(new HashSet<MathProgram>());
            return pop;
        }

        #endregion

        #region Nested type: FitnessFunction

        private sealed class FitnessFunction : IFitnessFunction<MathProgram>
        {
            #region Public Methods

            public int Compare(MathProgram x, MathProgram y) => this.Evaluate(x).CompareTo(this.Evaluate(y));

            public double Evaluate(MathProgram program) => program.Compute();

            #endregion
        }

        #endregion
    }
}
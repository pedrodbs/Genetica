// ------------------------------------------
// <copyright file="SelectionTests.cs" company="Pedro Sequeira">
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
using System.Linq;
using Genetica.Elements;
using Genetica.Elements.Terminals;
using Genetica.Evaluation;
using Genetica.Operators.Generation;
using Genetica.Operators.Selection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genetica.Tests
{
    [TestClass]
    public class SelectionTests
    {
        #region Static Fields & Constants

        private const int MAX_SIZE = 30;
        private static readonly Population<MathProgram, double> DefaultPop = CreatePopulation();

        private static readonly List<ISelectionOperator<MathProgram>> Operators =
            new List<ISelectionOperator<MathProgram>>
            {
                new RouletteWheelSelection<MathProgram>(PopulationSelectors.RandomSelector, new FitnessFunction()),
                new RouletteWheelSelection<MathProgram>(PopulationSelectors.UniformSelector, new FitnessFunction()),
                new TournamentSelection<MathProgram>(new FitnessFunction(), 5),
                new RouletteWheelSelection<MathProgram>(PopulationSelectors.RandomSelector,
                    new LinearRankFitnessFunction<MathProgram>(new FitnessFunction(), DefaultPop))

                //new RouletteWheelSelection<MathProgram>(PopulationSelectors.RandomSelector,
                //    new NonLinearRankFitnessFunction<MathProgram>(new FitnessFunction(), DefaultPop, 2))
            };

        #endregion

        #region Public Methods

        [TestMethod]
        public void SelectionsSizeTest()
        {
            foreach (var op in Operators)
            {
                var selection = op.Select(DefaultPop);
                var count = selection.Count();
                Console.WriteLine(count);
                Assert.AreEqual(count, MAX_SIZE, double.Epsilon,
                    $"Selection size should be the same as population size: {MAX_SIZE}.");
            }
        }

        [TestMethod]
        public void StochasticSelectionSizeTest()
        {
            var op = new StochasticSelection<MathProgram>(Operators);
            var selection = op.Select(DefaultPop);
            var count = selection.Count();
            Console.WriteLine(count);
            Assert.AreEqual(count, MAX_SIZE, double.Epsilon,
                $"Selection size should be the same as population size: {MAX_SIZE}.");
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
                MAX_SIZE, primitives, fullGen, fitnessFunction, null, null, null, 0);
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
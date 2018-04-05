// ------------------------------------------
// <copyright file="Program.cs" company="Pedro Sequeira">
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
//    Project: FunctionRegression
//    Last updated: 03/30/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using Genetica;
using Genetica.Elements;
using Genetica.Elements.Functions;
using Genetica.Elements.Terminals;
using Genetica.Evaluation;
using Genetica.Graphviz;
using Genetica.Operators.Crossover;
using Genetica.Operators.Generation;
using Genetica.Operators.Mutation;
using Genetica.Operators.Selection;
using QuickGraph.Graphviz.Dot;

namespace FunctionRegression
{
    internal class MainClass
    {
        #region Public Methods

        public static void Main(string[] args)
        {
            const uint popSize = 200;
            const uint maxDepth = 4;
            const uint maxGenerations = 2000;
            const uint maxElementLength = 20;
            const uint maxNoImproveGen = (uint) (maxGenerations * 0.5);
            const string solutionExp = "(+ (+ x x) (+ (* 3 (* x x)) 1))";

            var variable = new Variable("x");

            var primitives = new PrimitiveSet<MathProgram>(
                new HashSet<Terminal> {variable, new Constant(0), new Constant(1), new Constant(3)},
                new HashSet<MathProgram>());
            primitives.Add(MathPrimitiveSets.Default);

            var fitnessFunction = new FitnessFunction(x => 2 * x + 3 * x * x + 1, variable, 100, -50, 50);

            var solution = new MathExpressionConverter(primitives).FromPrefixNotation(solutionExp);
            Console.WriteLine("===================================");
            Console.WriteLine("Fitness: {0} | {1}", fitnessFunction.Evaluate(solution), solution);
            solution.ToGraphvizFile(Path.GetFullPath("."), "solution", GraphvizImageType.Png);
            Console.WriteLine("===================================");

            var seed = new AdditionFunction(new Constant(1), variable);

            var elementGenerator =
                new StochasticProgramGenerator<MathProgram, double>(
                    new List<IProgramGenerator<MathProgram, double>>
                    {
                        new GrowProgramGenerator<MathProgram, double>(),
                        new FullProgramGenerator<MathProgram, double>()
                    });
            var selection = new TournamentSelection<MathProgram>(fitnessFunction, (uint) (popSize * 0.05));
            var crossover = new StochasticCrossover<MathProgram>(
                new List<ICrossoverOperator<MathProgram>>
                {
                    new SubtreeCrossover<MathProgram, double>(),
                    new OnePointCrossover<MathProgram, double>(),
                    new ContextPreservingCrossover<MathProgram, double>(),
                    new UniformCrossover<MathProgram, double>()
                });
            var mutation = new StochasticMutation<MathProgram>(
                new List<IMutationOperator<MathProgram>>
                {
                    new SubtreeMutation<MathProgram, double>(elementGenerator, primitives, 1),
                    new PointMutation<MathProgram, double>(primitives),

                    //new ShrinkMutation(primitives),
                    new SimplifyMutation(),
                    new HoistMutation<MathProgram, double>()
                });

            var pop = new Population<MathProgram, double>(
                popSize, primitives, elementGenerator,
                fitnessFunction, selection, crossover, mutation, maxDepth, maxElementLength);
            pop.Init(new HashSet<MathProgram> {seed});

            MathProgram best = null;
            var numNoImproveGens = -1;
            for (var i = 0u; i < maxGenerations && numNoImproveGens < maxNoImproveGen; i++)
            {
                pop.Step();

                var newBest = pop.BestProgram;
                if (best == null) best = newBest;
                var diff = fitnessFunction.Evaluate(newBest) - fitnessFunction.Evaluate(best);
                numNoImproveGens = diff.Equals(0) && best.Equals(newBest) ? numNoImproveGens + 1 : 0;
                best = newBest;

                Print(pop, i, fitnessFunction, diff);
            }

            best.ToGraphvizFile(Path.GetFullPath("."), "best", GraphvizImageType.Png);
            Console.WriteLine("===================================");
            Console.WriteLine($"Best: {pop.BestProgram}, fitness: {fitnessFunction.Evaluate(pop.BestProgram):0.000}");
            Console.ReadKey();
        }

        #endregion

        #region Private & Protected Methods

        private static void Print(IPopulation<MathProgram> pop, uint generation,
            IFitnessFunction<MathProgram> fitnessFunction, double diff)
        {
            var elem = pop.BestProgram;
            Console.WriteLine(
                $"Gen {generation:000}, pop: {pop.Count:000}, diff: {diff:0.00}, " +
                $"fitness: {fitnessFunction.Evaluate(elem):0.00} | {elem}");
        }

        #endregion
    }
}
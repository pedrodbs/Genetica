// ------------------------------------------
// <copyright file="Program.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis.Examples.LogisticRegression
//    Last updated: 2017/09/08
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using Genesis.Crossover;
using Genesis.Elements;
using Genesis.Elements.Functions;
using Genesis.Elements.Terminals;
using Genesis.Evaluation;
using Genesis.Generation;
using Genesis.Graphviz;
using Genesis.Graphviz.Patch;
using Genesis.Mutation;
using Genesis.Selection;

namespace Genesis.Examples.LogisticRegression
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

            var primitives = new PrimitiveSet(
                new HashSet<Terminal> {variable, new Constant(0), new Constant(1), new Constant(3)},
                new HashSet<IFunction>());
            primitives.Add(PrimitiveSet.Default);

            var fitnessFunction = new FitnessFunction(x => 2 * x + 3 * x * x + 1, variable, 100, -50, 50);

            var solution = new ExpressionConverter(primitives).FromPrefixNotation(solutionExp);
            Console.WriteLine("===================================");
            Console.WriteLine("Fitness: {0} | {1}", fitnessFunction.Evaluate(solution), solution);
            solution.ToGraphvizFile(Path.GetFullPath("."), "solution", MyGraphvizImageType.Png);
            Console.WriteLine("===================================");

            var seed = new AdditionFunction(new Constant(1), variable);

            var elementGenerator =
                new StochasticElementGenerator(new List<IElementGenerator>
                                               {
                                                   new GrowElementGenerator(),
                                                   new FullElementGenerator()
                                               });
            var selection = new TournamentSelection(fitnessFunction, (uint) (popSize * 0.05));
            var crossover = new StochasticCrossover(new List<ICrossoverOperator>
                                                    {
                                                        new SubtreeCrossover(),
                                                        new OnePointCrossover(),
                                                        new ContextPreservingCrossover(),
                                                        new UniformCrossover()
                                                    });
            var mutation = new StochasticMutation(new List<IMutationOperator>
                                                  {
                                                      new SubtreeMutation(elementGenerator, primitives, 1),
                                                      new PointMutation(primitives),

                                                      //new ShrinkMutation(primitives),
                                                      new SimplifyMutation(),
                                                      new HoistMutation()
                                                  });

            var pop = new Population(
                popSize, primitives, elementGenerator,
                fitnessFunction, selection, crossover, mutation, maxDepth, maxElementLength);
            pop.Init(new HashSet<IElement> {seed});

            IElement best = null;
            var numNoImproveGens = -1;
            for (var i = 0u; i < maxGenerations && numNoImproveGens < maxNoImproveGen; i++)
            {
                pop.Step();

                var newBest = pop.BestElement;
                if (best == null) best = newBest;
                var diff = fitnessFunction.Evaluate(newBest) - fitnessFunction.Evaluate(best);
                numNoImproveGens = diff.Equals(0) && best.Equals(newBest) ? numNoImproveGens + 1 : 0;
                best = newBest;

                Print(pop, i, fitnessFunction, diff);
            }

            best.ToGraphvizFile(Path.GetFullPath("."), "best", MyGraphvizImageType.Png);
            Console.WriteLine("===================================");
            Console.WriteLine($"Best: {pop.BestElement}, fitness: {fitnessFunction.Evaluate(pop.BestElement):0.000}");
            Console.ReadKey();
        }

        #endregion

        #region Private & Protected Methods

        private static void Print(IPopulation pop, uint generation, IFitnessFunction fitnessFunction, double diff)
        {
            var elem = pop.BestElement;
            Console.WriteLine(
                $"Gen {generation:000}, pop: {pop.Count:000}, diff: {diff:0.00}, " +
                $"fitness: {fitnessFunction.Evaluate(elem):0.00} | {elem}");
        }

        #endregion
    }
}
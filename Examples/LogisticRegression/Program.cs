// ------------------------------------------
// <copyright file="Program.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis.Examples.LogisticRegression
//    Last updated: 2017/08/13
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
using Genesis.Mutation;
using Genesis.Selection;
using QuickGraph.Graphviz.Dot;

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

            var const0 = new Constant(0);
            var const1 = new Constant(1);
            var const3 = new Constant(3);
            var valued = new ValuedObject();
            var variable = new Variable("x", valued);
            var addition = new AdditionFunction(const0, const0);
            var subtr = new SubtractionFunction(const0, const0);
            var div = new DivisionFunction(const0, const0);
            var mult = new MultiplicationFunction(const0, const0);
            var max = new MaxFunction(const0, const0);
            var min = new MinFunction(const0, const0);
            var log = new LogarithmFunction(const0, const0);
            var pow = new PowerFunction(const0, const0);
            var ifop = new IfFunction(const0, const0, const0, const0);
            var cos = new CosineFunction(const0);
            var sin = new SineFunction(const0);

            var primitives = new PrimitiveSet(
                new HashSet<Terminal> {variable, const0, const1, const3},
                new HashSet<IFunction> {addition, subtr, div, mult, max, min, log, pow, ifop, cos, sin});

            var fitnessFunction = new FitnessFunction(x => 2 * x + 3 * x * x + 1, valued, 100, -50, 50);

            var solutionExp = "(+ (+ x x) (+ (* 3 (* x x)) 1))";
            var solution = new ExpressionConverter(primitives).FromPrefixNotation(solutionExp);
            Console.WriteLine("===================================");
            Console.WriteLine("Fitness: {0} | {1}", fitnessFunction.Evaluate(solution), solution);
            solution.ToGraphvizFile(Path.GetFullPath("."), "solution", GraphvizImageType.Png);
            Console.WriteLine("===================================");

            var seed = new AdditionFunction(const1, variable);

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

            best.ToGraphvizFile(Path.GetFullPath("."), "best", GraphvizImageType.Png);
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
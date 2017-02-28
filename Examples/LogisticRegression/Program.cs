using System;
using System.Collections.Generic;
using System.IO;
using Genesis;
using Genesis.Crossover;
using Genesis.Elements;
using Genesis.Elements.Functions;
using Genesis.Elements.Terminals;
using Genesis.Evaluation;
using Genesis.Generation;
using Genesis.Mutation;
using Genesis.QuickGraph;
using Genesis.Selection;
using QuickGraph.Graphviz.Dot;

namespace LogisticRegression
{
    internal class MainClass
	{
		public static void Main(string[] args)
		{
			const uint popSize = 200;
			const uint maxDepth = 4;
			const uint maxGenerations = 1000;
			const uint maxNoImproveGen = (uint)(maxGenerations * 0.5);

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
				new HashSet<Terminal> { variable, const0, const1, const3 },
				new HashSet<IFunction> { addition, subtr, div, mult, max, min, log, pow, ifop, cos, sin });

			var fitnessFunction = new FitnessFunction((x) => 2 * x + 3 * x * x + 1, valued, 100, -50, 50);

			var solutionExp = "(+ (+ x x) (+ (* 3 (* x x)) 1))";
			var solution = new ExpressionConverter(primitives).FromPrefixNotation(solutionExp);
			Console.WriteLine("Fitness: {0} | {1}", fitnessFunction.Evaluate(solution), solution);
			solution.ToGraphvizFile(GraphvizImageType.Png, Path.GetFullPath("."), "solution");

			var seed = new AdditionFunction(const1, variable);

			var elementGenerator =
				new StochasticElementGenerator(new List<IElementGenerator>
				{
					new GrowElemenetGenerator(),
					new FullElementGenerator()
				});
			var selection = new TournamentSelection(fitnessFunction, (uint)(popSize * 0.03));
			var crossover = new StochasticCrossover(new List<ICrossoverOperator>
				{
					new SubtreeCrossover(),
					//new OnePointCrossover(),
					//new ContextPreservingCrossover(),
					new UniformCrossover()
				});
			var mutation = new StochasticMutation(new List<IMutationOperator>
				{
					//new SubtreeMutation(elementGenerator, primitives, 1),
					new PointMutation(primitives),
					new ShrinkMutation(primitives),
					//new HoistMutation(),
				});

			var pop = new Population(
				popSize, primitives, elementGenerator, maxDepth,
				fitnessFunction, selection, crossover, mutation);
			pop.Init(new HashSet<IElement> { seed });

			IElement best = null;
			var numNoImproveGens = -1;
			for (var i = 0u; (i < maxGenerations) && (numNoImproveGens < maxNoImproveGen); i++)
			{
				pop.Step();

				var newBest = pop.BestElement;
				if (best == null) best = newBest;
				var diff = fitnessFunction.Evaluate(newBest) - fitnessFunction.Evaluate(best);
				numNoImproveGens = (diff.Equals(0) && best.Equals(newBest)) ? numNoImproveGens + 1 : 0;
				best = newBest;

				Print(pop, i, fitnessFunction, diff);
			}

			pop.BestElement.ToGraphvizFile(GraphvizImageType.Png, Path.GetFullPath("."), "best");
			Console.ReadKey();
		}

		private static void Print(IPopulation pop, uint generation, IFitnessFunction fitnessFunction, double diff)
		{
			var elem = pop.BestElement;
			Console.WriteLine(
				"Gen {0:000}, pop: {1:000}, diff: {2:0.00}, fitness: {3:0.00} | {4}",
				generation, pop.Count, diff, fitnessFunction.Evaluate(elem), elem);

		}
	}
}

using System.Collections.Generic;
using Genesis.Elements;
using Genesis.Crossover;
using Genesis.Generation;
using System;
using Genesis.Selection;
using System.Linq;
using Genesis.Mutation;
using MathNet.Numerics.Random;

namespace Genesis
{
	[Serializable]
	public class Population : HashSet<IElement>, IPopulation
	{
		private readonly Random _random = new WH2006(RandomSeed.Robust());
		private readonly ICrossoverOperator _crossoverOperator;
		private readonly IMutationOperator _mutationOperator;
		private readonly ISelectionOperator _selectionOperator;
		private readonly uint _maxSize;
		private readonly IElementGenerator _elementGenerator;
		private readonly PrimitiveSet _primitives;
		private readonly uint _maxDepth;
		private readonly IComparer<IElement> _elementComparer;

		public double CrossoverPercent { get; set; }
		public double MutationPercent { get; set; }
		public double ElitismPercent { get; set; }

		public IElement BestElement { get; private set; }

		public Population(
			uint maxSize,
			PrimitiveSet primitives,
			IElementGenerator elementGenerator,
			uint maxDepth,
			IComparer<IElement> elementComparer,
			ISelectionOperator selectionOperator,
			ICrossoverOperator crossoverOperator,
			IMutationOperator mutationOperator,
			double crossoverPercent = 0.65d,
			double mutationPercent = 0.2d,
			double elitismPercent = 0.1d)
		{
			this._maxSize = maxSize;
			this._primitives = primitives;
			this._elementGenerator = elementGenerator;
			this._maxDepth = maxDepth;
			this._elementComparer = elementComparer;
			this._selectionOperator = selectionOperator;
			this._mutationOperator = mutationOperator;
			this._crossoverOperator = crossoverOperator;
			this.ElitismPercent = elitismPercent;
			this.MutationPercent = mutationPercent;
			this.CrossoverPercent = crossoverPercent;
		}

		public void Init(ISet<IElement> seeds)
		{
			// clear pop
			this.Clear();

			// add seeds directly to the initial pop
			if (seeds != null)
			{
				var i = 0;
				foreach (var elem in seeds)
					if (i++ < this._maxSize) this.Add(elem);
			}

			// creates new elements
			for (var i = this.Count; i < this._maxSize; i++)
			{
				IElement element;
				do
				{
					element = this._elementGenerator.Generate(this._primitives, this._maxDepth);
				} while (this.Contains(element));
				this.Add(element);
			}
		}

		public virtual void Step()
		{
			// checks initialization
			if (this.Count == 0) this.Init(null);

			var newGeneration = new List<IElement>((int)this._maxSize);

			// 1 - performs selection to get pool of parents for crossover
			var selection = new List<IElement>(this._selectionOperator.Select(this));

			// 2 - performs crossover to get some offspring
			var numOffspring = (int)(this.CrossoverPercent * this._maxSize);
			for (var i = 0; i < numOffspring; i++)
			{
				//randomly selects 2 parents (may be equal)
				var parent1 = selection[this._random.Next(selection.Count)];
				var parent2 = selection[this._random.Next(selection.Count)];
				newGeneration.Add(this._crossoverOperator.Crossover(parent1, parent2));
			}

			// 3 - performs mutation from selection
			var numMutations = (int)(this.MutationPercent * this._maxSize);
			for (var i = 0; i < numMutations; i++)
			{
				//randomly selects element
				var element = selection[this._random.Next(selection.Count)];
				newGeneration.Add(this._mutationOperator.Mutate(element));
			}

			// 4 - performs elite selection (keeps some best elements)
			var numElite = (int)(this.ElitismPercent * this._maxSize);
			var j = 0;
			foreach (var element in this.Reverse())
			{
				if (j++ >= numElite) break;
				newGeneration.Add(element);
			}

			// 5 - creates random elements
			for (var i = newGeneration.Count; i < this._maxSize; i++)
				newGeneration.Add(this._elementGenerator.Generate(this._primitives, this._maxDepth));

			// 6 - replace population with new generation
			this.Clear();
			foreach (var element in newGeneration)
				this.Add(element);

			newGeneration.Clear();
			selection.Clear();

			// gets best element
			foreach (var element in this)
				if ((this.BestElement == null) || this._elementComparer.Compare(element, this.BestElement) > 0)
					this.BestElement = element;
		}

		#region IDisposable Support

		private bool _disposed;

		protected virtual void Dispose(bool disposing)
		{
			if (this._disposed) return;
			if (disposing) this.Clear();

			this._disposed = true;
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion
	}
}

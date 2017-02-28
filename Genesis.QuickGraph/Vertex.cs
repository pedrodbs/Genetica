using System;
using Genesis.Elements;
using MathNet.Numerics.Random;

namespace Genesis.QuickGraph
{
	public class Vertex
	{
		private readonly Random _random = new WH2006(RandomSeed.Robust());

		private readonly int _hashCode;

		public IElement Element { get; }

		public Vertex(IElement element)
		{
			// every vertex will have its own hash key 
			// to ensure repetition in the graph display of leaf elements (terminals)
			this._hashCode = this._random.Next();
			this.Element = element;
		}

		public override string ToString()
		{
			return this.Element.Label;
		}

		public override int GetHashCode()
		{
			return this._hashCode;
		}
	}
}

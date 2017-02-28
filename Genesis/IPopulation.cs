using System;
using System.Collections.Generic;
using Genesis.Elements;

namespace Genesis
{
	public interface IPopulation : ISet<IElement>, IDisposable
    {
		double CrossoverPercent { get; set; }

		double MutationPercent { get; set; }

		double ElitismPercent { get; set; }

		IElement BestElement { get; }

		void Init(ISet<IElement> seeds);
        
		void Step();
	}
}

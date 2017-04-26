// ------------------------------------------
// <copyright file="SymbolTreeSimilarity.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis
//    Last updated: 2017/03/06
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using Genesis.Elements;

namespace Genesis.Similarity
{
	/// <summary>
	///     Measures the similarity of elements based on the similarity of their symbol-trees.
	/// </summary>
	public class SymbolTreeSimilarity : ISimilarityMeasure
	{
		#region Public Methods

		public double Calculate(IElement elem1, IElement elem2)
		{
			if (elem1 == null || elem2 == null) return 0;
			if (elem1.Equals(elem2)) return 1;

			var elem1SymbTree = new SymbolTree();
			elem1SymbTree.AddElement(elem1);
			var elem2SymbTree = new SymbolTree();
			elem2SymbTree.AddElement(elem2);
			return elem1SymbTree.GetSimilarity(elem2SymbTree);
		}

		#endregion
	}
}
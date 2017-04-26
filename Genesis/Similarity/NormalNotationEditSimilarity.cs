// ------------------------------------------
// <copyright file="NormalNotationEditSimilarity.cs" company="Pedro Sequeira">
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

using System;
using Genesis.Elements;
using Genesis.Util;

namespace Genesis.Similarity
{
	/// <summary>
	///     Measures the similarity of elements based on the edit or Levenshtein string distance between the expressions of the
	///     elements in normal notation, i.e., based on the number of edits needed to transform the expression of one element
	///     into the one of the other element.
	/// </summary>
	public class NormalNotationEditSimilarity : ISimilarityMeasure
	{
		#region Public Methods

		public double Calculate(IElement elem1, IElement elem2)
		{
			if (elem1 == null || elem2 == null) return 0;
			if (elem1.Equals(elem2)) return 1;

			var expr1 = elem1.Expression;
			var expr2 = elem2.Expression;
			return 1d - (double)expr1.LevenshteinDistance(expr2) / Math.Max(expr1.Length, expr2.Length);
		}

		#endregion
	}
}
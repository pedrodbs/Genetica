// ------------------------------------------
// <copyright file="AverageSimilarity.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis
//    Last updated: 2017/03/09
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Genesis.Elements;

namespace Genesis.Similarity
{
    public class AverageSimilarity : ISimilarityMeasure
    {
        #region Fields

        private readonly IEnumerable<ISimilarityMeasure> _measures;

        #endregion

        #region Constructors

        public AverageSimilarity(IEnumerable<ISimilarityMeasure> measures)
        {
            this._measures = measures;
        }

        #endregion

        #region Public Methods

        public double Calculate(IElement elem1, IElement elem2)
        {
            var sum = this._measures.Sum(measure => measure.Calculate(elem1, elem2));
            return sum / this._measures.Count();
        }

        #endregion
    }
}
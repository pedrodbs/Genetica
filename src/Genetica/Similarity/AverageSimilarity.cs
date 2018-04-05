// ------------------------------------------
// <copyright file="AverageSimilarity.cs" company="Pedro Sequeira">
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
//    Project: Genetica
//    Last updated: 03/31/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Genetica.Elements;

namespace Genetica.Similarity
{
    /// <summary>
    ///     Represents a <see cref="ISimilarityMeasure{TProgram}" /> that computes the average similarity computed by other
    ///     several similarity measures.
    /// </summary>
    /// <typeparam name="TProgram">The type of program.</typeparam>
    public class AverageSimilarity<TProgram> : ISimilarityMeasure<TProgram> where TProgram : IProgram
    {
        #region Fields

        private readonly IEnumerable<ISimilarityMeasure<TProgram>> _measures;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="AverageSimilarity{TProgram}" /> with the given measures.
        /// </summary>
        /// <param name="measures">The similarity measures used to compute the average similarity.</param>
        public AverageSimilarity(IEnumerable<ISimilarityMeasure<TProgram>> measures)
        {
            this._measures = measures;
        }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public double Calculate(TProgram prog1, TProgram prog2) =>
            this._measures.Sum(measure => measure.Calculate(prog1, prog2)) / this._measures.Count();

        #endregion
    }
}
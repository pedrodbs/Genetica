// ------------------------------------------
// <copyright file="ValueSimilarity.cs" company="Pedro Sequeira">
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
//    Project: Genesis
//    Last updated: 03/23/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using Genesis.Elements;
using Genesis.Elements.Terminals;

namespace Genesis.Similarity
{
    /// <summary>
    ///     Measures the similarity of two <see cref="MathProgram" /> in terms of the values that they can compute when we
    ///     replace their <see cref="Constant" /> programs by random variables.
    /// </summary>
    public class ValueSimilarity : ISimilarityMeasure<MathProgram>
    {
        #region Public Methods

        public double Calculate(MathProgram prog1, MathProgram prog2)
        {
            if (prog1 == null || prog2 == null) return 0;
            if (prog1.Equals(prog2)) return 1;

            var valueRmsd = prog1.GetValueRmsd(prog2);
            return valueRmsd.Equals(double.NaN) ? 0 : 1d - Math.Min(1, valueRmsd);
        }

        #endregion
    }
}
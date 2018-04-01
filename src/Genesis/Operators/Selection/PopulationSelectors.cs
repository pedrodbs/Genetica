// ------------------------------------------
// <copyright file="PopulationSelectors.cs" company="Pedro Sequeira">
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
//    Last updated: 03/31/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using MathNet.Numerics.Random;

namespace Genesis.Operators.Selection
{
    /// <summary>
    ///     Represents a delegate for methods of assigning proportions of selection to some
    ///     <see cref="IPopulation{TProgram}" /> to be used in conjunction with some
    ///     <see cref="ISelectionOperator{TProgram}" />.
    /// </summary>
    /// <param name="numPointers">The number of pointers for the selection.</param>
    /// <returns>The array of selection pointers</returns>
    public delegate double[] PopulationSelector(uint numPointers);

    /// <summary>
    ///     Provides methods that can be used as <see cref="PopulationSelector" /> delegates.
    /// </summary>
    public static class PopulationSelectors
    {
        #region Static Fields & Constants

        private static readonly Random Random = new WH2006(RandomSeed.Robust());

        #endregion

        #region Public Methods

        /// <summary>
        ///     Gets an array of pointers that are unevenly (randomly) spread.
        /// </summary>
        /// <param name="numPointers">The number of pointers for the selection.</param>
        /// <returns>The array of selection pointers</returns>
        public static double[] RandomSelector(uint numPointers)
        {
            var pointers = new double[numPointers];
            for (var i = 0; i < numPointers; i++)
                pointers[i] = Random.NextDouble();
            return pointers;
        }

        //// <param name="numPointers">The number of pointers for the selection.</param>
        /// <summary>
        ///     Gets an array of pointers that are equally spread.
        /// </summary>
        /// <returns>The array of selection pointers</returns>
        public static double[] UniformSelector(uint numPointers)
        {
            var delta = 1d / numPointers;
            var initDelta = Random.NextDouble() * delta;
            var pointers = new double[numPointers];
            for (var i = 0; i < numPointers; i++)
                pointers[i] = initDelta + i * delta;
            return pointers;
        }

        #endregion
    }
}
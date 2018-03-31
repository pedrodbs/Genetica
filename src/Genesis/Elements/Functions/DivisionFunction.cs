// ------------------------------------------
// <copyright file="DivisionFunction.cs" company="Pedro Sequeira">
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
//    Last updated: 03/26/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System.Collections.Generic;

namespace Genesis.Elements.Functions
{
    /// <summary>
    ///     Represents a <see cref="BinaryFunction" /> performing the division between two sub-programs.
    /// </summary>
    public class DivisionFunction : BinaryFunction
    {
        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="DivisionFunction" />.
        /// </summary>
        /// <param name="numeratorProgram">The sub-program computing the numerator value.</param>
        /// <param name="denominatorProgram">The sub-program computing the denominator value.</param>
        public DivisionFunction(ITreeProgram<double> numeratorProgram, ITreeProgram<double> denominatorProgram) :
            base(numeratorProgram, denominatorProgram)
        {
            this.Expression = $"({this.FirstParameter.Expression}{this.Label}{this.SecondParameter.Expression})";
        }

        #endregion

        #region Properties & Indexers

        public override string Expression { get; }

        public override string Label => "/";

        #endregion

        #region Public Methods

        public override double Compute() => this.FirstParameter.Compute() / this.SecondParameter.Compute();

        public override ITreeProgram<double> CreateNew(IList<ITreeProgram<double>> children) =>
            children == null || children.Count != 2
                ? null
                : new DivisionFunction(children[0], children[1]);

        #endregion
    }
}
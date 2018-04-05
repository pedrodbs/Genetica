// ------------------------------------------
// <copyright file="CosineFunction.cs" company="Pedro Sequeira">
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

using System;
using System.Collections.Generic;

namespace Genetica.Elements.Functions
{
    /// <summary>
    ///     Represents a <see cref="UnaryFunction" /> performing the cosine operation over some sub-program.
    /// </summary>
    public class CosineFunction : UnaryFunction
    {
        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="CosineFunction" /> with the given parameter.
        /// </summary>
        /// <param name="parameter">The  parameter of the program.</param>
        public CosineFunction(ITreeProgram<double> parameter) : base(parameter)
        {
        }

        #endregion

        #region Properties & Indexers

        /// <inheritdoc />
        public override string Label => "cos";

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public override double Compute() => Math.Cos(this.Operand.Compute());

        /// <inheritdoc />
        public override ITreeProgram<double> CreateNew(IList<ITreeProgram<double>> children) =>
            children == null || children.Count != 1
                ? null
                : new CosineFunction(children[0]);

        #endregion
    }
}
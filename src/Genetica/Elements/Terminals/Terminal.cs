// ------------------------------------------
// <copyright file="Terminal.cs" company="Pedro Sequeira">
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
//    Last updated: 04/04/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System.Collections.Generic;

namespace Genetica.Elements.Terminals
{
    /// <summary>
    ///     A terminal represents a <see cref="MathProgram" /> whose output value depends only on itself. It can be used as a
    ///     leaf node in a program's tree, i.e., it has no children/input.
    /// </summary>
    public abstract class Terminal : MathProgram
    {
        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="Terminal" />.
        /// </summary>
        protected Terminal() : base(new MathProgram[0])
        {
        }

        #endregion

        #region Properties & Indexers

        /// <inheritdoc />
        public override string Expression => this.Label;

        #endregion

        #region Public Methods

        /// <summary>
        ///     Because terminals have no children, this returns the object itself.
        /// </summary>
        /// <param name="children">This parameter will not be used, so it may be null.</param>
        /// <returns>The same <see cref="Terminal" /> object.</returns>
        public override ITreeProgram<double> CreateNew(IList<ITreeProgram<double>> children) => this;

        /// <inheritdoc />
        public override bool Equals(object obj) =>
            !(obj is null) && (ReferenceEquals(this, obj) || obj is Terminal term && this.Label.Equals(term.Label));

        /// <inheritdoc />
        public override int GetHashCode() => this.Expression.GetHashCode();

        /// <summary>
        ///     Because terminals have no children, the program cannot be simplified so this method returns the object itself.
        /// </summary>
        /// <returns>The same <see cref="Terminal" /> object.</returns>
        public override ITreeProgram<double> Simplify() => this;

        #endregion
    }
}
// ------------------------------------------
// <copyright file="Variable.cs" company="Pedro Sequeira">
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

namespace Genetica.Elements.Terminals
{
    /// <summary>
    ///     Represents a mathematical variable represented by some label and whose value is in some given <see cref="Range" />.
    /// </summary>
    public class Variable : Terminal
    {
        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="Variable" /> with the given label and default range.
        /// </summary>
        /// <param name="label">The label associated with the variable.</param>
        public Variable(string label) : this(label, Range.Default)
        {
        }

        /// <summary>
        ///     Creates a new <see cref="Variable" />with the given label, range and initial value.
        /// </summary>
        /// <param name="label">The label associated with the variable.</param>
        /// <param name="value">The initial value of the variable.</param>
        /// <param name="range">The range of possible values for the variable.</param>
        public Variable(string label, double value, Range range)
        {
            this.Value = value;
            this.Label = label;
            this.Range = range;
        }

        /// <summary>
        ///     Creates a new <see cref="Variable" />with the given label and range.
        /// </summary>
        /// <param name="label">The label associated with the variable.</param>
        /// <param name="range">The range of possible values for the variable.</param>
        public Variable(string label, Range range)
        {
            this.Label = label;
            this.Range = range;
        }

        #endregion

        #region Properties & Indexers

        /// <inheritdoc />
        public override string Label { get; }

        /// <summary>
        ///     Gets the minimum and maximum value expected for this variable.
        /// </summary>
        public Range Range { get; }

        /// <summary>
        ///     Gets or sets the value associated with this variable.
        /// </summary>
        public double Value { get; set; }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public override double Compute() => this.Value;

        #endregion
    }
}
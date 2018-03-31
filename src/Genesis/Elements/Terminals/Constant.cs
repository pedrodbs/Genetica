// ------------------------------------------
// <copyright file="Constant.cs" company="Pedro Sequeira">
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
//    Last updated: 03/22/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System.Globalization;

namespace Genesis.Elements.Terminals
{
    /// <summary>
    ///     Represents a <see cref="Terminal" /> program whose output value does not change.
    /// </summary>
    public class Constant : Terminal
    {
        #region Static Fields & Constants

        public static readonly Constant Zero = new Constant(0);
        public static readonly Constant One = new Constant(1);

        #endregion

        #region Fields

        private readonly double _value;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="Constant" /> with the given value.
        /// </summary>
        /// <param name="val">The value of this constant.</param>
        public Constant(double val)
        {
            this._value = val;
        }

        #endregion

        #region Properties & Indexers

        public override string Label => this._value.ToString("0.##", CultureInfo.InvariantCulture);

        #endregion

        #region Public Methods

        public override double Compute() => this._value;

        #endregion

        #region Public Methods

        public override int GetHashCode() => this._value.GetHashCode();

        #endregion
    }
}
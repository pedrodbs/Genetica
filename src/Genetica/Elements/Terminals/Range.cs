// ------------------------------------------
// <copyright file="Range.cs" company="Pedro Sequeira">
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

namespace Genetica.Elements.Terminals
{
    /// <summary>
    ///     Allows the definition of a range of <see cref="double" /> values by setting the minimum and maximum values allowed.
    /// </summary>
    public struct Range : IEquatable<Range>
    {
        #region Static Fields & Constants

        private const int DEFAULT_VALUE = 100;

        /// <summary>
        ///     Returns the default value for the full <see cref="double" /> range.
        /// </summary>
        public static Range Default = new Range(-DEFAULT_VALUE, DEFAULT_VALUE);

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="Range" /> corresponding to some constant value.
        /// </summary>
        /// <param name="constValue">The minimum and maximum of this range.</param>
        public Range(double constValue) : this(constValue, constValue)
        {
        }

        /// <summary>
        ///     Creates a new <see cref="Range" /> with the given minimum and maximum values.
        /// </summary>
        /// <param name="min">The minimum value allowed in this range.</param>
        /// <param name="max">The maximum value allowed in this range.</param>
        public Range(double min, double max)
        {
            this.Min = min;
            this.Max = max;
        }

        #endregion

        #region Properties & Indexers

        /// <summary>
        ///     Gets the difference between the minimum and maximum values.
        /// </summary>
        public double Interval => this.Max - this.Min;

        /// <summary>
        ///     Gets or sets the maximum value allowed in this range.
        /// </summary>
        public double Max { get; }

        /// <summary>
        ///     Gets or sets the minimum value allowed in this range.
        /// </summary>
        public double Min { get; }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            return obj is Range range && Equals(range);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (this.Max.GetHashCode() * 397) ^ this.Min.GetHashCode();
            }
        }

        /// <inheritdoc />
        public override string ToString() => $"[{this.Min:0.###},{this.Max:0.###}]";

        #endregion

        #region Public Methods

        /// <summary>
        ///     Checks whether two <see cref="Range" /> objects are equal, i.e., whether their <see cref="Max" /> and
        ///     <see cref="Min" /> properties are equal.
        /// </summary>
        /// <param name="left">The first range.</param>
        /// <param name="right">The second range.</param>
        /// <returns><c>true</c> if the ranges are equal, <c>false</c> otherwise.</returns>
        public static bool operator ==(Range left, Range right) => left.Equals(right);

        /// <summary>
        ///     Checks whether two <see cref="Range" /> objects are not equal, i.e., whether their <see cref="Max" /> or
        ///     <see cref="Min" /> properties are not equal.
        /// </summary>
        /// <param name="left">The first range.</param>
        /// <param name="right">The second range.</param>
        /// <returns><c>true</c> if the ranges are not equal, <c>false</c> otherwise.</returns>
        public static bool operator !=(Range left, Range right) => !left.Equals(right);

        /// <inheritdoc />
        public bool Equals(Range other) => this.Max.Equals(other.Max) && this.Min.Equals(other.Min);

        #endregion
    }
}
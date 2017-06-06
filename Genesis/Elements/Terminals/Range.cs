// ------------------------------------------
// <copyright file="Range.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis
//    Last updated: 2017/06/06
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;

namespace Genesis.Elements.Terminals
{
    /// <summary>
    ///     Allows the definition of a range of <see cref="double" /> values by setting the minimum and maximum values allowed.
    /// </summary>
    public struct Range : IEquatable<Range>
    {
        #region Static Fields & Constants

        private const int DEFAULT_VALUE = 100;

        /// <summary>
        ///     Returns the full <see cref="double" /> range.
        /// </summary>
        public static Range Default = new Range(-DEFAULT_VALUE, DEFAULT_VALUE);

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

        #region Constructors

        public Range(double min, double max)
        {
            this.Min = min;
            this.Max = max;
        }

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Range && Equals((Range) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.Max.GetHashCode() * 397) ^ this.Min.GetHashCode();
            }
        }

        public override string ToString()
        {
            return $"[{this.Min},{this.Max}]";
        }

        #endregion

        #region Public Methods

        public static bool operator ==(Range left, Range right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Range left, Range right)
        {
            return !left.Equals(right);
        }

        public bool Equals(Range other)
        {
            return this.Max.Equals(other.Max) && this.Min.Equals(other.Min);
        }

        #endregion
    }
}
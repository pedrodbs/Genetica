// ------------------------------------------
// <copyright file="Variable.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis
//    Last updated: 2017/09/11
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;

namespace Genesis.Elements.Terminals
{
    public class Variable : Terminal, IEquatable<Variable>
    {
        #region Constructors

        public Variable(string label) : this(label, Range.Default)
        {
        }

        public Variable(string label, double value, Range range)
        {
            this.Value = value;
            this.Label = label;
            this.Range = range;
        }

        public Variable(string label, Range range)
        {
            this.Label = label;
            this.Range = range;
        }

        #endregion

        #region Properties & Indexers

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

        public override bool Equals(object obj)
        {
            return !ReferenceEquals(null, obj) &&
                   (ReferenceEquals(this, obj) || obj is Variable && this.Equals((Variable) obj));
        }

        public override int GetHashCode() => this.Label.GetHashCode();

        public override double GetValue() => this.Value; // this._valuedObject?.Value ?? 0d;

        #endregion

        #region Public Methods

        public static bool operator ==(Variable left, Variable right)
        {
            return ReferenceEquals(left, right) || !ReferenceEquals(null, left) && left.Equals(right);
        }

        public static bool operator !=(Variable left, Variable right)
        {
            return !(left == right);
        }

        public bool Equals(Variable other)
        {
            return !ReferenceEquals(null, other) &&
                   (ReferenceEquals(this, other) || string.Equals(this.Label, other.Label));
        }

        #endregion
    }
}
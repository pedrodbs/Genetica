// ------------------------------------------
// <copyright file="Variable.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis
//    Last updated: 2017/09/06
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
        #region Fields

        private readonly IValued _valuedObject;

        #endregion

        #region Constructors

        public Variable(string label, IValued valuedObject) : this(label, valuedObject, Range.Default)
        {
        }

        public Variable(string label, IValued valuedObject, Range range)
        {
            this.Label = label;
            this._valuedObject = valuedObject;
            this.Range = range;
        }

        #endregion

        #region Properties & Indexers

        public override string Label { get; }

        /// <summary>
        ///     The minimum and maximum value allowed for this variable.
        /// </summary>
        public Range Range { get; }

        #endregion

        #region Public Methods

        public override IElement Clone() => new Variable(this.Label, this._valuedObject, this.Range);

        public override bool Equals(object obj)
        {
            return !ReferenceEquals(null, obj) &&
                   (ReferenceEquals(this, obj) || obj is Variable && this.Equals((Variable) obj));
        }

        public override int GetHashCode() => this.Label.GetHashCode();

        public override double GetValue() => this._valuedObject?.Value ?? 0d;

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
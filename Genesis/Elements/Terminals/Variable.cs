// ------------------------------------------
// <copyright file="Variable.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis
//    Last updated: 2017/05/12
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

        private readonly int _hashCode;

        private readonly IValued _valuedObject;

        #endregion

        #region Properties & Indexers

        public override string Label { get; }

        #endregion

        #region Constructors

        public Variable(string label, IValued valuedObject)
        {
            this.Label = label;
            this._valuedObject = valuedObject;
        }

        #endregion

        #region Public Methods

        public override IElement Clone()
        {
            return new Variable(this.Label, this._valuedObject);
        }

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
// ------------------------------------------
// <copyright file="Constant.cs" company="Pedro Sequeira">
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
using System.Globalization;

namespace Genesis.Elements.Terminals
{
    public class Constant : Terminal, IEquatable<Constant>
    {
        #region Fields

        private readonly int _hashCode;
        private readonly double _value;

        #endregion

        #region Constructors

        public Constant(double val)
        {
            this._value = val;
            this._hashCode = this._value.GetHashCode();
        }

        #endregion

        #region Properties & Indexers

        public override string Label => this._value.ToString("0.##", CultureInfo.InvariantCulture);

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            return !ReferenceEquals(null, obj) &&
                   (ReferenceEquals(this, obj) || obj is Constant && this.Equals((Constant) obj));
        }

        public override int GetHashCode() => this._hashCode;

        public override double GetValue() => this._value;

        #endregion

        #region Public Methods

        public static bool operator ==(Constant left, Constant right)
        {
            return ReferenceEquals(left, right) || !ReferenceEquals(null, left) && left.Equals(right);
        }

        public static bool operator !=(Constant left, Constant right)
        {
            return !(left == right);
        }

        public bool Equals(Constant other)
        {
            return !ReferenceEquals(null, other) && (ReferenceEquals(this, other) || this._value.Equals(other._value));
        }

        #endregion
    }
}
// ------------------------------------------
// <copyright file="BinaryFunction.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis
//    Last updated: 2017/08/12
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;

namespace Genesis.Elements.Functions
{
    public abstract class BinaryFunction : IFunction, IEquatable<BinaryFunction>
    {
        #region Fields

        private readonly IElement[] _children;

        private readonly int _hashCode;

        #endregion

        #region Constructors

        protected BinaryFunction(IElement firstElement, IElement secondElement)
        {
            this._children = new[] {firstElement, secondElement};
            this._hashCode = this.ProduceHashCode();
            this.Length = (ushort) (1 + firstElement.Length + this.SecondElement.Length);
            this.Expression = $"{this.Label}({this.FirstElement.Expression},{this.SecondElement.Expression})";
        }

        #endregion

        #region Properties & Indexers

        public IElement FirstElement => this._children[0];

        public uint NumChildren => 2;

        public IElement SecondElement => this._children[1];

        public virtual string Expression { get; }

        public virtual string Label { get; } = "f";

        public IReadOnlyList<IElement> Children => this._children;

        public ushort Length { get; }

        IReadOnlyList<ITreeNode> ITreeNode.Children => this.Children;

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            return !ReferenceEquals(null, obj) &&
                   (ReferenceEquals(this, obj) || obj.GetType() == this.GetType() && this.Equals((BinaryFunction) obj));
        }

        public override int GetHashCode() => this._hashCode;

        public override string ToString()
        {
            return this.Expression;
        }

        #endregion

        #region Public Methods

        public static bool operator ==(BinaryFunction left, BinaryFunction right)
        {
            return ReferenceEquals(left, right) || !ReferenceEquals(null, left) && left.Equals(right);
        }

        public static bool operator !=(BinaryFunction left, BinaryFunction right)
        {
            return !(left == right);
        }

        public int CompareTo(IElement other)
        {
            return string.CompareOrdinal(this.Expression, other.Expression);
        }

        public IElement Clone() => this.CreateNew(new[] {this.FirstElement, this.SecondElement});

        public abstract IElement CreateNew(IList<IElement> children);

        public abstract double GetValue();

        public bool Equals(BinaryFunction other)
        {
            return !ReferenceEquals(null, other) &&
                   (ReferenceEquals(this, other) ||
                    this._hashCode == other._hashCode &&
                    string.Equals(this.Label, other.Label) &&
                    this.FirstElement.Equals(other.FirstElement) &&
                    this.SecondElement.Equals(other.SecondElement));
        }

        #endregion

        #region Private & Protected Methods

        private int ProduceHashCode()
        {
            unchecked
            {
                const int hashingBase = (int) 2166136261;
                const int hashingMultiplier = 16777619;

                var hashCode = hashingBase;
                hashCode = (hashCode * hashingMultiplier) ^ this.FirstElement.GetHashCode();
                hashCode = (hashCode * hashingMultiplier) ^ this.SecondElement.GetHashCode();
                return (hashCode * hashingMultiplier) ^ this.Label.GetHashCode();
            }
        }

        #endregion
    }
}
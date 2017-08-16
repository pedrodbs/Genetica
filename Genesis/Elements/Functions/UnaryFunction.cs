// ------------------------------------------
// <copyright file="UnaryFunction.cs" company="Pedro Sequeira">
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

using System.Collections.Generic;

namespace Genesis.Elements.Functions
{
    public abstract class UnaryFunction : IFunction
    {
        #region Fields

        private readonly IElement[] _children;

        private readonly int _hashCode;

        #endregion

        #region Constructors

        protected UnaryFunction(IElement operand)
        {
            this._children = new[] {operand};
            this._hashCode = this.ProduceHashCode();
            this.Length = (ushort) (1 + operand.Length);
            this.Expression = $"{this.Label}({this.Operand.Expression})";
        }

        #endregion

        #region Properties & Indexers

        public uint NumChildren => 1;

        public IElement Operand => this.Children[0];

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
                   (ReferenceEquals(this, obj) || obj.GetType() == this.GetType() && this.Equals((UnaryFunction) obj));
        }

        public override int GetHashCode() => this._hashCode;

        public override string ToString()
        {
            return this.Expression;
        }

        #endregion

        #region Public Methods

        public static bool operator ==(UnaryFunction left, UnaryFunction right)
        {
            return ReferenceEquals(left, right) || !ReferenceEquals(null, left) && left.Equals(right);
        }

        public static bool operator !=(UnaryFunction left, UnaryFunction right)
        {
            return !(left == right);
        }

        public bool Equals(UnaryFunction other)
        {
            return !ReferenceEquals(null, other) &&
                   (ReferenceEquals(this, other) ||
                    this._hashCode == other._hashCode &&
                    string.Equals(this.Label, other.Label) && this.Operand.Equals(other.Operand));
        }

        public int CompareTo(IElement other)
        {
            return string.CompareOrdinal(this.Expression, other.Expression);
        }

        public IElement Clone() => this.CreateNew(new[] {this.Operand});

        public abstract IElement CreateNew(IList<IElement> children);

        public abstract double GetValue();

        #endregion

        #region Private & Protected Methods

        private int ProduceHashCode()
        {
            unchecked
            {
                const int hashingBase = (int) 2166136261;
                const int hashingMultiplier = 16777619;

                var hashCode = hashingBase;
                hashCode = (hashCode * hashingMultiplier) ^ this.Operand.GetHashCode();
                return (hashCode * hashingMultiplier) ^ this.Label.GetHashCode();
            }
        }

        #endregion
    }
}
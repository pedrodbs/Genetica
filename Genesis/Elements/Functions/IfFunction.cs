// ------------------------------------------
// <copyright file="IfFunction.cs" company="Pedro Sequeira">
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
    public class IfFunction : IFunction
    {
        #region Fields

        private readonly IElement[] _children;

        private readonly int _hashCode;

        #endregion

        #region Constructors

        public IfFunction(
            IElement conditionElement, IElement zeroElement, IElement positiveElement, IElement negativeElement)
        {
            this._children = new[] {conditionElement, zeroElement, positiveElement, negativeElement};
            this._hashCode = this.ProduceHashCode();
            this.Length = (ushort) (1 + conditionElement.Length + zeroElement.Length +
                                    positiveElement.Length + negativeElement.Length);
            this.Expression = $"({this.ConditionElement.Expression}?{this.ZeroElement.Expression}:" +
                              $"{this.PositiveElement.Expression}:{this.NegativeElement.Expression})";
        }

        #endregion

        #region Properties & Indexers

        public IElement ConditionElement => this._children[0];

        public IElement NegativeElement => this._children[3];

        public uint NumChildren => 4;

        public IElement PositiveElement => this._children[2];

        public IElement ZeroElement => this._children[1];

        public IReadOnlyList<IElement> Children => this._children;

        public string Expression { get; }

        public string Label => "if";

        public ushort Length { get; }

        IReadOnlyList<ITreeNode> ITreeNode.Children => this.Children;

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            return !ReferenceEquals(null, obj) &&
                   (ReferenceEquals(this, obj) || obj.GetType() == this.GetType() && this.Equals((IfFunction) obj));
        }

        public override int GetHashCode() => this._hashCode;

        public override string ToString() => this.Expression;

        #endregion

        #region Public Methods

        public static bool operator ==(IfFunction left, IfFunction right)
        {
            return ReferenceEquals(left, right) || !ReferenceEquals(null, left) && left.Equals(right);
        }

        public static bool operator !=(IfFunction left, IfFunction right)
        {
            return !(left == right);
        }

        public bool Equals(IfFunction other)
        {
            return !ReferenceEquals(null, other) &&
                   (ReferenceEquals(this, other) ||
                    this._hashCode == other._hashCode &&
                    string.Equals(this.Label, other.Label) &&
                    this.ConditionElement.Equals(other.ConditionElement) &&
                    this.PositiveElement.Equals(other.PositiveElement) &&
                    this.NegativeElement.Equals(other.NegativeElement) &&
                    this.ZeroElement.Equals(other.ZeroElement));
        }

        public int CompareTo(IElement other) => string.CompareOrdinal(this.Expression, other.Expression);

        public IElement Clone()
        {
            return new IfFunction(
                this.ConditionElement, this.ZeroElement, this.PositiveElement, this.NegativeElement);
        }

        public IElement CreateNew(IList<IElement> children)
        {
            return children == null || children.Count != 4
                ? null
                : new IfFunction(children[0], children[1], children[2], children[3]);
        }

        public double GetValue()
        {
            var val = this.ConditionElement.GetValue();
            return val.Equals(0)
                ? this.ZeroElement.GetValue()
                : (val > 0
                    ? this.PositiveElement.GetValue()
                    : this.NegativeElement.GetValue());
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
                hashCode = (hashCode * hashingMultiplier) ^ this.ConditionElement.GetHashCode();
                hashCode = (hashCode * hashingMultiplier) ^ this.ZeroElement.GetHashCode();
                hashCode = (hashCode * hashingMultiplier) ^ this.PositiveElement.GetHashCode();
                hashCode = (hashCode * hashingMultiplier) ^ this.NegativeElement.GetHashCode();
                return (hashCode * hashingMultiplier) ^ this.Label.GetHashCode();
            }
        }

        #endregion
    }
}
using System.Collections.Generic;

namespace Genesis.Elements.Functions
{
	public class IfFunction : IFunction
	{
		private readonly IElement[] _children;

	    public IElement ConditionElement => this._children[0];

		public IElement ZeroElement => this._children[1];

        public IElement PositiveElement => this._children[2];

        public IElement NegativeElement => this._children[3];

        public IReadOnlyList<IElement> Children => this._children;

        IReadOnlyList<ITreeNode> ITreeNode.Children => this.Children;

        public uint NumChildren => 4;

	    private readonly int _hashCode;

		public string Label => "if";

	    public string Expression =>
	        $"({this.ConditionElement.Expression}?{this.ZeroElement.Expression}:" +
	        $"{this.PositiveElement.Expression}:{this.NegativeElement.Expression})";

        public ushort Count { get; }

        public double GetValue()
		{
			var val = this.ConditionElement.GetValue();
			return val.Equals(0)
					  ? this.ZeroElement.GetValue()
					  : (val > 0
						 ? this.PositiveElement.GetValue()
						 : this.NegativeElement.GetValue());
		}

		public IfFunction(
            IElement conditionElement, IElement zeroElement, IElement positiveElement, IElement negativeElement)
		{
		    this._children = new[] {conditionElement, zeroElement, positiveElement, negativeElement};
		    this._hashCode = this.ProduceHashCode();
            this.Count = 
                (ushort)(1 + conditionElement.Count + zeroElement.Count + positiveElement.Count + negativeElement.Count);
        }

        public override bool Equals(object obj)
        {
            return !ReferenceEquals(null, obj) &&
                   (ReferenceEquals(this, obj) || (obj.GetType() == this.GetType() && this.Equals((IfFunction)obj)));
        }

        public bool Equals(IfFunction other)
        {
            return !ReferenceEquals(null, other) &&
                   (ReferenceEquals(this, other) ||
                    (this._hashCode == other._hashCode &&
                     string.Equals(this.Label, other.Label) &&
                     this.ConditionElement.Equals(other.ConditionElement) &&
                     this.PositiveElement.Equals(other.PositiveElement) &&
                     this.NegativeElement.Equals(other.NegativeElement) &&
                     this.ZeroElement.Equals(other.ZeroElement)));
        }

	    public override int GetHashCode() => this._hashCode;

        private int ProduceHashCode()
        {
            unchecked
            {
                const int hashingBase = (int)2166136261;
                const int hashingMultiplier = 16777619;

                var hashCode = hashingBase;
                hashCode = (hashCode * hashingMultiplier) ^ this.ConditionElement.GetHashCode();
                hashCode = (hashCode * hashingMultiplier) ^ this.ZeroElement.GetHashCode();
                hashCode = (hashCode * hashingMultiplier) ^ this.PositiveElement.GetHashCode();
                hashCode = (hashCode * hashingMultiplier) ^ this.NegativeElement.GetHashCode();
                return (hashCode * hashingMultiplier) ^ this.Label.GetHashCode();
            }
        }

        public static bool operator ==(IfFunction left, IfFunction right)
        {
            return ReferenceEquals(left, right) || (!ReferenceEquals(null, left) && left.Equals(right));
        }

        public static bool operator !=(IfFunction left, IfFunction right)
        {
            return !(left == right);
        }

        public override string ToString() => this.Expression;

		public IElement CreateNew(IList<IElement> children)
		{
			return (children == null || children.Count != 4)
				? null
				: new IfFunction(children[0], children[1], children[2], children[3]);
		}

		public IElement Clone()
		{
			return new IfFunction(
				this.ConditionElement, this.ZeroElement, this.PositiveElement, this.NegativeElement);
		}

        public int CompareTo(IElement other) => string.CompareOrdinal(this.Expression, other.Expression);
    }
}

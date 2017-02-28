using System.Collections.Generic;

namespace Genesis.Elements.Functions
{
	public abstract class UnaryFunction : IFunction
	{
	    public IElement Operand => this.Children[0];

		private readonly IElement[] _children;

		public IReadOnlyList<IElement> Children => this._children;

		public uint NumChildren => 1;

	    private readonly int _hashCode;

		public abstract string Label { get; }

		public virtual string Expression => $"f({this.Operand.Expression})";

        public ushort Count { get; }

        public abstract double GetValue();

		protected UnaryFunction(IElement operand)
		{
			this._children = new[] { operand };
		    this._hashCode = this.ProduceHashCode();
		    this.Count = (ushort) (1 + operand.Count);
		}

        public override bool Equals(object obj)
        {
            return !ReferenceEquals(null, obj) &&
                   (ReferenceEquals(this, obj) || (obj.GetType() == this.GetType() && this.Equals((UnaryFunction)obj)));
        }

        public bool Equals(UnaryFunction other)
        {
            return !ReferenceEquals(null, other) &&
                   (ReferenceEquals(this, other) ||
                    (string.Equals(this.Label, other.Label) && this.Operand.Equals(other.Operand)));
        }

        public override int GetHashCode() => this._hashCode;

        private int ProduceHashCode()
        {
            unchecked
            {
                const int hashingBase = (int)2166136261;
                const int hashingMultiplier = 16777619;

                var hashCode = hashingBase;
                hashCode = (hashCode * hashingMultiplier) ^ this.Operand.GetHashCode();
                return (hashCode * hashingMultiplier) ^ this.Label.GetHashCode();
            }
        }

        public static bool operator ==(UnaryFunction left, UnaryFunction right)
        {
            return ReferenceEquals(left, right) || (!ReferenceEquals(null, left) && left.Equals(right));
        }

        public static bool operator !=(UnaryFunction left, UnaryFunction right)
        {
            return !(left == right);
        }

        public abstract IElement CreateNew(IList<IElement> children);

		public override string ToString()
		{
			return this.Expression;
		}

		public IElement Clone()
		{
			return this.CreateNew(new[] { this.Operand });
		}

        public int CompareTo(IElement other)
        {
            return string.CompareOrdinal(this.Expression, other.Expression);
        }
    }
}

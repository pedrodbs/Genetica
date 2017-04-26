using System;
using System.Collections.Generic;

namespace Genesis.Elements.Functions
{
	public abstract class BinaryFunction : IFunction, IEquatable<BinaryFunction>
	{
		private readonly IElement[] _children;

	    public IElement FirstElement => this._children[0];

		public IElement SecondElement => this._children[1];

		public abstract double GetValue();

		public IReadOnlyList<IElement> Children => this._children;

        IReadOnlyList<ITreeNode> ITreeNode.Children => this.Children;

        public uint NumChildren => 2;

	    private readonly int _hashCode;

		public virtual string Expression => $"f({this.FirstElement.Expression},{this.SecondElement.Expression})";

	    public ushort Count { get; }

	    public abstract string Label { get; }

		protected BinaryFunction(IElement firstElement, IElement secondElement)
		{
			this._children = new[] { firstElement, secondElement };
			this._hashCode = this.ProduceHashCode();
		    this.Count = (ushort) (1 + firstElement.Count + this.SecondElement.Count);
		}

		public override string ToString()
		{
			return this.Expression;
		}

		public override bool Equals(object obj)
		{
			return !ReferenceEquals(null, obj) &&
				   (ReferenceEquals(this, obj) || (obj.GetType() == this.GetType() && this.Equals((BinaryFunction)obj)));
		}

		public bool Equals(BinaryFunction other)
		{
			return !ReferenceEquals(null, other) &&
				   (ReferenceEquals(this, other) ||
                   (this._hashCode == other._hashCode &&
					string.Equals(this.Label, other.Label) &&
					 this.FirstElement.Equals(other.FirstElement) &&
					 this.SecondElement.Equals(other.SecondElement)));
		}

        public override int GetHashCode() => this._hashCode;

        private int ProduceHashCode()
		{
			unchecked
			{
				const int hashingBase = (int)2166136261;
				const int hashingMultiplier = 16777619;

				var hashCode = hashingBase;
				hashCode = (hashCode * hashingMultiplier) ^ this.FirstElement.GetHashCode();
				hashCode = (hashCode * hashingMultiplier) ^ this.SecondElement.GetHashCode();
				return (hashCode * hashingMultiplier) ^ this.Label.GetHashCode();
			}
		}

		public static bool operator ==(BinaryFunction left, BinaryFunction right)
		{
			return ReferenceEquals(left, right) || (!ReferenceEquals(null, left) && left.Equals(right));
		}

		public static bool operator !=(BinaryFunction left, BinaryFunction right)
		{
			return !(left == right);
		}

		public abstract IElement CreateNew(IList<IElement> children);

		public IElement Clone()
		{
			return this.CreateNew(new[] { this.FirstElement, this.SecondElement });
		}

	    public int CompareTo(IElement other)
	    {
	       return string.CompareOrdinal(this.Expression, other.Expression);
	    }
	}
}

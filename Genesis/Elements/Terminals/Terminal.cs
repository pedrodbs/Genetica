using System.Collections.Generic;

namespace Genesis.Elements.Terminals
{
	public abstract class Terminal : IElement
	{
		private readonly IElement[] _children = new IElement[0];

		public IReadOnlyList<IElement> Children => this._children;

        IReadOnlyList<ITreeNode> ITreeNode.Children => this.Children;

        public abstract string Label { get; }

		public string Expression => this.Label;

	    public ushort Length => 1;

        public IElement CreateNew(IList<IElement> children)
        {
            return this.Clone();
        }

        public abstract IElement Clone();

		public override string ToString()
		{
			return this.Label;
		}

        public override bool Equals(object obj)
        {
            return !ReferenceEquals(null, obj) &&
                (ReferenceEquals(this, obj) || (obj.GetType() == this.GetType() && Equals((Terminal)obj)));
        }

        public bool Equals(Terminal other)
        {
            return !ReferenceEquals(null, other) && (ReferenceEquals(this, other) || string.Equals(this.Label, other.Label));
        }

        public override int GetHashCode()
        {
            return this.Expression.GetHashCode();
        }

        public static bool operator ==(Terminal left, Terminal right)
        {
            return ReferenceEquals(left, right) || (!ReferenceEquals(null, left) && left.Equals(right));
        }

        public static bool operator !=(Terminal left, Terminal right)
        {
            return !(left == right);
        }

	    public abstract double GetValue();

        public int CompareTo(IElement other) => string.CompareOrdinal(this.Expression, other.Expression);
    }
}

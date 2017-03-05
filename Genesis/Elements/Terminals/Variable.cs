using System;

namespace Genesis.Elements.Terminals
{
	public class Variable : Terminal, IEquatable<Variable>
	{
	    private readonly IValued _valuedObject;
		
		public Variable(string name, IValued valuedObject)
		{
			this.Label = name;
			this._valuedObject = valuedObject;
		    this._hashCode = this.ProduceHashCode();
		}

		private readonly int _hashCode;

	    public override string Label { get; }

	    public override IElement Clone()
		{
			return new Variable(this.Label, this._valuedObject);
		}

		public override bool Equals(object obj)
        {
            return !ReferenceEquals(null, obj) &&
                   (ReferenceEquals(this, obj) || (obj.GetType() == this.GetType() && this.Equals((Variable)obj)));
        }

        public bool Equals(Variable other)
        {
            return !ReferenceEquals(null, other) &&
                   (ReferenceEquals(this, other) || (string.Equals(this.Label, other.Label)));
        }

	    public override int GetHashCode() => this._hashCode;

	    private int ProduceHashCode()
	    {
	        unchecked
	        {
	            const int hashingBase = (int) 2166136261;
	            const int hashingMultiplier = 16777619;
	            return (hashingBase * hashingMultiplier) ^ this.Label?.GetHashCode() ?? 0;
	        }
	    }

	    public static bool operator ==(Variable left, Variable right)
        {
            return ReferenceEquals(left, right) || (!ReferenceEquals(null, left) && left.Equals(right));
        }

        public static bool operator !=(Variable left, Variable right)
        {
            return !(left == right);
        }

	    public override double GetValue() => this._valuedObject?.Value ?? 0d;
	}
}

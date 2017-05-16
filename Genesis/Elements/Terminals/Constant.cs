using System;

namespace Genesis.Elements.Terminals
{
	public class Constant : Terminal, IEquatable<Constant>
	{
	    private readonly double _value;

		public Constant(double val)
		{
			this._value = val;
			this._hashCode = this._value.GetHashCode();
		}

	    private readonly int _hashCode;

	    public override string Label => this._value.ToString("0.##");

		public override IElement Clone()
		{
			return new Constant(this._value);
		}

	    public override bool Equals(object obj)
	    {
	        return !ReferenceEquals(null, obj) && 
                (ReferenceEquals(this, obj) || (obj is Constant && Equals((Constant) obj)));
	    }

	    public bool Equals(Constant other)
	    {
	        return !ReferenceEquals(null, other) && (ReferenceEquals(this, other) || this._value.Equals(other._value));
	    }

        public override int GetHashCode() => this._hashCode;

        public static bool operator ==(Constant left, Constant right)
        {
            return ReferenceEquals(left, right) || (!ReferenceEquals(null, left) && left.Equals(right));
        }

        public static bool operator !=(Constant left, Constant right)
        {
            return !(left == right);
        }

        public override double GetValue() => this._value;
    }
}

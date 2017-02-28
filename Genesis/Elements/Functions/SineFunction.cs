using System;
using System.Collections.Generic;

namespace Genesis.Elements.Functions
{
	public class SineFunction : UnaryFunction
	{
		public override string Label => "sin";

	    public override string Expression => $"sin({this.Operand.Expression})";

        public SineFunction(IElement operand) : base(operand)
		{
		}

		public override double GetValue()
		{
			return Math.Sin(this.Operand.GetValue());
		}

		public override IElement CreateNew(IList<IElement> children)
		{
			return (children == null || children.Count != 1)
				? null
				: new SineFunction(children[0]);
		}
	}
}

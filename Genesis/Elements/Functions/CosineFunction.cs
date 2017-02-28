using System;
using System.Collections.Generic;

namespace Genesis.Elements.Functions
{
	public class CosineFunction : UnaryFunction
	{
		public override string Label => "cos";

	    public override string Expression => $"cos({this.Operand.Expression})";

        public CosineFunction(IElement operand) : base(operand)
		{
		}

		public override double GetValue()
		{
			return Math.Cos(this.Operand.GetValue());
		}

		public override IElement CreateNew(IList<IElement> children)
		{
			return (children == null || children.Count != 1)
				? null
				: new CosineFunction(children[0]);
		}
	}
}

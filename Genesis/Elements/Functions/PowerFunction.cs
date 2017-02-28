using System;
using System.Collections.Generic;

namespace Genesis.Elements.Functions
{
	public class PowerFunction : BinaryFunction
	{
		public override string Label => "^";

	    public override string Expression => $"({this.FirstElement.Expression}^{this.SecondElement.Expression})";

        public PowerFunction(IElement baseElement, IElement exponentElement) :
			base(baseElement, exponentElement)
		{
		}

		public override double GetValue()
		{
			return Math.Pow(this.FirstElement.GetValue(), this.SecondElement.GetValue());
		}

		public override IElement CreateNew(IList<IElement> children)
		{
			return (children == null || children.Count != 2)
				? null
				: new PowerFunction(children[0], children[1]);
		}
	}
}

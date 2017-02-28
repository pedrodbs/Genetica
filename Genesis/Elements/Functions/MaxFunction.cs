using System;
using System.Collections.Generic;

namespace Genesis.Elements.Functions
{
	public class MaxFunction : CommutativeBinaryFunction
	{
		public override string Label => "max";

	    public override string Expression => $"max({this.FirstElement.Expression},{this.SecondElement.Expression})";

        public MaxFunction(IElement firstElement, IElement secondElement) :
			base(firstElement, secondElement)
		{
		}

		public override double GetValue()
		{
			return Math.Max(this.FirstElement.GetValue(), this.SecondElement.GetValue());
		}

		public override IElement CreateNew(IList<IElement> children)
		{
			return (children == null || children.Count != 2)
				? null
				: new MaxFunction(children[0], children[1]);
		}
	}
}

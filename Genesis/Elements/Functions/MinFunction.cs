using System;
using System.Collections.Generic;

namespace Genesis.Elements.Functions
{
	public class MinFunction : CommutativeBinaryFunction
	{
		public override string Label => "min";

	    public override string Expression => $"min({this.FirstElement.Expression},{this.SecondElement.Expression})";

        public MinFunction(IElement firstElement, IElement secondElement) :
			base(firstElement, secondElement)
		{
		}

		public override double GetValue()
		{
			return Math.Min(this.FirstElement.GetValue(), this.SecondElement.GetValue());
		}

		public override IElement CreateNew(IList<IElement> children)
		{
			return (children == null || children.Count != 2)
				? null
				: new MinFunction(children[0], children[1]);
		}
	}
}

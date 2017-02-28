using System;
using System.Collections.Generic;

namespace Genesis.Elements.Functions
{
	public class LogarithmFunction : BinaryFunction
	{
		public override string Label => "log";

	    public override string Expression => $"log({this.FirstElement.Expression},{this.SecondElement.Expression})";

        public LogarithmFunction(IElement baseElement, IElement valueElement) :
			base(baseElement, valueElement)
		{
		}

		public override double GetValue()
		{
			return Math.Log(this.FirstElement.GetValue(), this.SecondElement.GetValue());
		}

		public override IElement CreateNew(IList<IElement> children)
		{
			return (children == null || children.Count != 2)
				? null
				: new LogarithmFunction(children[0], children[1]);
		}
	}
}

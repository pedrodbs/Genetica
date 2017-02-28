using System.Collections.Generic;

namespace Genesis.Elements.Functions
{
	public class DivisionFunction : BinaryFunction
	{
		public override string Label => "/";

	    public override string Expression => $"({this.FirstElement.Expression}/{this.SecondElement.Expression})";

        public DivisionFunction(IElement numeratorElement, IElement denominatorElement) :
			base(numeratorElement, denominatorElement)
		{
		}

		public override double GetValue()
		{
			return this.FirstElement.GetValue() / this.SecondElement.GetValue();
		}

		public override IElement CreateNew(IList<IElement> children)
		{
			return (children == null || children.Count != 2)
				? null
				: new DivisionFunction(children[0], children[1]);
		}
	}
}

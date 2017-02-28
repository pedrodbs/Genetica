using System.Collections.Generic;

namespace Genesis.Elements.Functions
{
	public class AdditionFunction : CommutativeBinaryFunction
	{
		public override string Label => "+";

	    public override string Expression => $"({this.FirstElement.Expression}+{this.SecondElement.Expression})";

        public AdditionFunction(IElement firstElement, IElement secondElement) :
			base(firstElement, secondElement)
		{
		}

		public override double GetValue()
		{
			return this.FirstElement.GetValue() + this.SecondElement.GetValue();
		}

		public override IElement CreateNew(IList<IElement> children)
		{
			return (children == null || children.Count != 2)
				? null
				: new AdditionFunction(children[0], children[1]);
		}
	}
}

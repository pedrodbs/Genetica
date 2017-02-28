using Genesis.Elements.Terminals;
using System.Linq;
using Genesis.Elements.Functions;
using Genesis.Evaluation;
using System;

namespace Genesis.Elements
{
	public static class SimplificationExtensions
	{
		/// <summary>
		/// Verifies whether the <see cref="IElement"/> has a constant value, i.e., whether all its descendant leaf 
		/// elements are instances of <see cref="Constant"/>.
		/// </summary>
		/// <returns><c>true</c>, if all leaf elements are constant, <c>false</c> otherwise.</returns>
		/// <param name="element">The element to verify whether it is a constant.</param>
		public static bool IsConstant(this IElement element)
		{
			return ((element != null) &&
					((element is Constant) ||
					 ((element.Children != null) && (element.Children.Count > 0) &&
					  element.Children.All(child => child.IsConstant()))));
		}

		/// <summary>
		/// Verifies whether the <see cref="IElement"/> is a constant value, i.e., whether all its descendant leaf 
		/// elements are instances of <see cref="Constant"/>, and whether the associated value equals to 
		/// <paramref name="val"/>.
		/// </summary>
		/// <returns><c>true</c>, if element is a constant and its value equals <paramref name="val"/>, <c>false</c> 
		/// otherwise.</returns>
		/// <param name="element">The element to verify whether it is a constant.</param>
		/// <param name="val">The value to test for the element.</param>
		public static bool EqualsConstant(this IElement element, double val)
		{
			return element.IsConstant() && element.GetValue().Equals(val);
		}

		/// <summary>
		/// Verifies whether the <see cref="IElement"/> contains a constant value, i.e., whether any one of its 
		/// descendant elements are instances have a constant value equal to <paramref name="val"/>.
		/// </summary>
		/// <returns><c>true</c>, if element contains a constant value equal to <paramref name="val"/>, <c>false</c> 
		/// otherwise.</returns>
		/// <param name="element">The element to verify whether it contains a constant.</param>
		/// <param name="val">The value to test for the element.</param>
		public static bool ContainsConstant(this IElement element, double val)
		{
			return ((element != null) &&
					(element.EqualsConstant(val) ||
					 ((element.Children != null) && (element.Children.Count > 0) &&
					  element.Children.Any(child => child.ContainsConstant(val)))));
		}

		/// <summary>
		/// Simplifies the expression of the given element by returning a new <see cref="IElement"/> which value will 
		/// always be equal to the original element, i.e. it removes reduntant operations, e.g. subtrees with functions
		/// that will always result in the value of one of its operands.
		/// </summary>
		/// <returns>An element corresponding to a simplification of the given element.</returns>
		/// <param name="element">The element we want to simplify.</param>
		public static IElement Simplify(this IElement element)
		{
			if (element == null) return null;

			// if its a terminal, just return the element, no more simplifications possible
			if ((element.Children == null) || (element.Children.Count == 0))
				return element;

			// if its a constant value, just return a constant with that value
			if (element.IsConstant())
				return new Constant(element.GetValue());

			// otherwise first tries to simplify children
			var children = new IElement[element.Children.Count];
			for (var i = 0; i < element.Children.Count; i++)
				children[i] = element.Children[i].Simplify();

			// if its an addition, check whether one operand is 0 and return the other
			if (element is AdditionFunction)
				if (children[0].EqualsConstant(0))
					return children[1];
				else if (children[1].EqualsConstant(0))
					return children[0];

			// if its an subtraction
			if (element is SubtractionFunction)
				// if the operands are equal, return 0
				if (children[0].Equals(children[1]))
					return new Constant(0);
				// check whether second operand is 0 and return the first
				else if (children[1].EqualsConstant(0))
					return children[0];

			// if its a multiplication, check whether one operand is 1 and return the other
			if (element is MultiplicationFunction)
				if (children[0].EqualsConstant(1))
					return children[1];
				else if (children[1].EqualsConstant(1))
					return children[0];

			// if its a multiplication, check whether one of operands is 0 and return 0
			if ((element is MultiplicationFunction) &&
				(children[0].EqualsConstant(0) || children[1].EqualsConstant(0)))
				return new Constant(0);

			// if its a division or power, check whether second operand is 1 and return the first
			if (((element is DivisionFunction) || (element is PowerFunction)) && children[1].EqualsConstant(1))
				return children[0];

			// if its a division
			if (element is DivisionFunction)
				//if operands are equal, return 1
				if (children[0].Equals(children[1]))
					return new Constant(1);
				//if first operand is 0 return 0
				else if (children[0].EqualsConstant(0))
					return new Constant(0);

			// if its a min or max, check whether operands are equal and return the first
			if (((element is MaxFunction) || (element is MinFunction)) && children[0].Equals(children[1]))
				return children[0];

			// if its an if clause, check whether the first child is a constant and returns one of the other children
			// accordingly
			if ((element is IfFunction) && children[0].IsConstant())
			{
				var val = children[0].GetValue();
				return val.Equals(0) ? children[1] : (val > 0 ? children[2] : children[3]);
			}

			// if its an if clause, check whether result children are equal, in which case replace by one of them
			if ((element is IfFunction) && children[1].Equals(children[2]) && children[1].Equals(children[3]))
				return children[1];

			return element.CreateNew(children);
		}

		/// <summary>
		/// Simplifies the expression of the given element by returning a new <see cref="IElement"/> whose fitness is 
		/// approximately equal to that of the original element by a factor of <paramref name="epsilon"/>, and whose 
		/// expression is simpler, i.e., has fewer sub-elements.
		/// </summary>
		/// <returns>An element corresponding to a simplification of the given element.</returns>
		/// <param name="element">The element we want to simplify.</param>
		/// <param name="fitnessFunction">The fitness function used to determine equivalence.</param>
		/// <param name="epsilon">The acceptable difference between the fitness of the given element and that of a 
		/// simplified element for them to be considered equivalent.</param>
		public static IElement Simplify(this IElement element, IFitnessFunction fitnessFunction, double epsilon = 0.001)
		{
			// gets original element fitness and count
			var fitness = fitnessFunction.Evaluate(element);

			// gets all subcombinations of the element
			var simplified = element;
			var count = element.Count;
			var subCombs = element.GetSubCombinations();
			foreach (var subComb in subCombs)
			{
				var subFit = fitnessFunction.Evaluate(subComb);
				var subCount = subComb.Count;

				//checks their fitness and count
				if ((Math.Abs(subFit - fitness) <= epsilon) && (subCount < count))
				{
					simplified = subComb;
					count = subCount;
				}
			}

			subCombs.Clear();
			return simplified;
		}
	}
}

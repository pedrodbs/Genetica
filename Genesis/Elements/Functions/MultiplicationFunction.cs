// ------------------------------------------
// <copyright file="MultiplicationFunction.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis
//    Last updated: 2017/08/12
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System.Collections.Generic;

namespace Genesis.Elements.Functions
{
    public class MultiplicationFunction : CommutativeBinaryFunction
    {
        #region Constructors

        public MultiplicationFunction(IElement firstElement, IElement secondElement) :
            base(firstElement, secondElement)
        {
            this.Expression = $"({this.FirstElement.Expression}{this.Label}{this.SecondElement.Expression})";
        }

        #endregion

        #region Properties & Indexers

        public override string Expression { get; }

        public override string Label => "*";

        #endregion

        #region Public Methods

        public override IElement CreateNew(IList<IElement> children)
        {
            return children == null || children.Count != 2
                ? null
                : new MultiplicationFunction(children[0], children[1]);
        }

        public override double GetValue()
        {
            return this.FirstElement.GetValue() * this.SecondElement.GetValue();
        }

        #endregion
    }
}
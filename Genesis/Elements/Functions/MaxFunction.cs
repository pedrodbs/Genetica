// ------------------------------------------
// <copyright file="MaxFunction.cs" company="Pedro Sequeira">
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

using System;
using System.Collections.Generic;

namespace Genesis.Elements.Functions
{
    public class MaxFunction : CommutativeBinaryFunction
    {
        #region Constructors

        public MaxFunction(IElement firstElement, IElement secondElement) :
            base(firstElement, secondElement)
        {
            this.Expression = $"{this.Label}({this.FirstElement.Expression},{this.SecondElement.Expression})";
        }

        #endregion

        #region Properties & Indexers

        public override string Expression { get; }

        public override string Label => "max";

        #endregion

        #region Public Methods

        public override IElement CreateNew(IList<IElement> children)
        {
            return children == null || children.Count != 2
                ? null
                : new MaxFunction(children[0], children[1]);
        }

        public override double GetValue()
        {
            return Math.Max(this.FirstElement.GetValue(), this.SecondElement.GetValue());
        }

        #endregion
    }
}
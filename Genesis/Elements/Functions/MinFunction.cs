// ------------------------------------------
// <copyright file="MinFunction.cs" company="Pedro Sequeira">
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
    public class MinFunction : CommutativeBinaryFunction
    {
        #region Constructors

        public MinFunction(IElement firstElement, IElement secondElement) :
            base(firstElement, secondElement)
        {
            this.Expression = $"{this.Label}({this.FirstElement.Expression},{this.SecondElement.Expression})";
        }

        #endregion

        #region Properties & Indexers

        public override string Expression { get; }

        public override string Label => "min";

        #endregion

        #region Public Methods

        public override IElement CreateNew(IList<IElement> children)
        {
            return children == null || children.Count != 2
                ? null
                : new MinFunction(children[0], children[1]);
        }

        public override double GetValue()
        {
            return Math.Min(this.FirstElement.GetValue(), this.SecondElement.GetValue());
        }

        #endregion
    }
}
// ------------------------------------------
// <copyright file="PowerFunction.cs" company="Pedro Sequeira">
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
    public class PowerFunction : BinaryFunction
    {
        #region Constructors

        public PowerFunction(IElement baseElement, IElement exponentElement) :
            base(baseElement, exponentElement)
        {
            this.Expression = $"({this.FirstElement.Expression}{this.Label}{this.SecondElement.Expression})";
        }

        #endregion

        #region Properties & Indexers

        public override string Expression { get; }

        public override string Label => "^";

        #endregion

        #region Public Methods

        public override IElement CreateNew(IList<IElement> children)
        {
            return children == null || children.Count != 2
                ? null
                : new PowerFunction(children[0], children[1]);
        }

        public override double GetValue()
        {
            return Math.Pow(this.FirstElement.GetValue(), this.SecondElement.GetValue());
        }

        #endregion
    }
}
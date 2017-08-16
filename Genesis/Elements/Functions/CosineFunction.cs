// ------------------------------------------
// <copyright file="CosineFunction.cs" company="Pedro Sequeira">
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
    public class CosineFunction : UnaryFunction
    {
        #region Constructors

        public CosineFunction(IElement operand) : base(operand)
        {
            this.Expression = $"{this.Label}({this.Operand.Expression})";
        }

        #endregion

        #region Properties & Indexers

        public override string Expression { get; }

        public override string Label => "cos";

        #endregion

        #region Public Methods

        public override IElement CreateNew(IList<IElement> children)
        {
            return children == null || children.Count != 1
                ? null
                : new CosineFunction(children[0]);
        }

        public override double GetValue()
        {
            return Math.Cos(this.Operand.GetValue());
        }

        #endregion
    }
}
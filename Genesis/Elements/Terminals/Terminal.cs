// ------------------------------------------
// <copyright file="Terminal.cs" company="Pedro Sequeira">
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

namespace Genesis.Elements.Terminals
{
    public abstract class Terminal : IElement
    {
        #region Fields

        private readonly IElement[] _children = new IElement[0];

        #endregion

        #region Properties & Indexers

        public IReadOnlyList<IElement> Children => this._children;

        public string Expression => this.Label;

        public abstract string Label { get; }

        public ushort Length => 1;

        IReadOnlyList<ITreeNode> ITreeNode.Children => this.Children;

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            return !ReferenceEquals(null, obj) &&
                   (ReferenceEquals(this, obj) || obj.GetType() == this.GetType() && this.Equals((Terminal) obj));
        }

        public override int GetHashCode() => this.Expression.GetHashCode();

        public override string ToString() => this.Label;

        #endregion

        #region Public Methods

        public static bool operator ==(Terminal left, Terminal right)
        {
            return ReferenceEquals(left, right) || !ReferenceEquals(null, left) && left.Equals(right);
        }

        public static bool operator !=(Terminal left, Terminal right)
        {
            return !(left == right);
        }

        public bool Equals(Terminal other)
        {
            return !ReferenceEquals(null, other) &&
                   (ReferenceEquals(this, other) || string.Equals(this.Label, other.Label));
        }

        public int CompareTo(IElement other) => string.CompareOrdinal(this.Expression, other.Expression);

        public abstract IElement Clone();

        public IElement CreateNew(IList<IElement> children) => this.Clone();

        public abstract double GetValue();

        #endregion
    }
}
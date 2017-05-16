// ------------------------------------------
// <copyright file="ExpressionConverter.cs" company="Pedro Sequeira">
//     Some copyright
// </copyright>
// <summary>
//    Project: Genesis
//    Last updated: 2017/05/12
// 
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Genesis.Elements.Terminals;

namespace Genesis.Elements
{
    public class ExpressionConverter : IDisposable
    {
        #region Static Fields & Constants

        private const string SUB_ELEM_PATTERN = @"((?>[^\(\)]+|(?<l>\()|(?<-l>\)))+(?(l)(?!))|[^\(\)]+)";

        #endregion

        #region Fields

        private readonly Dictionary<string, IElement> _functionPatterns = new Dictionary<string, IElement>();
        private readonly string[] _parentheses = {"(", ")"};
        private readonly Dictionary<string, IElement> _primitiveLabels = new Dictionary<string, IElement>();

        #endregion

        #region Constructors

        public ExpressionConverter(PrimitiveSet primitives)
        {
            var allPrimitives = new List<IElement>(primitives.Functions);
            allPrimitives.AddRange(primitives.Terminals);

            // stores info on all primitive labels
            foreach (var primitive in allPrimitives)
                this._primitiveLabels.Add(primitive.Label, primitive);

            // stores regex patterns for functions
            var rndLabel = DateTime.Now.ToString("yyyyMMMMddHHmmssfff");
            var dummy = new DummyTerminal(rndLabel);
            foreach (var function in primitives.Functions.ToList())
            {
                var pattern = GetFunctionPattern(function, dummy);
                this._functionPatterns.Add(pattern, function);
            }
        }

        #endregion

        #region Public Methods

        public IElement FromNormalNotation(string expression)
        {
            if (expression == null) return null;

            // cleans expression
            expression = expression.Replace(" ", string.Empty);

            // tests for terminals
            if (this._primitiveLabels.ContainsKey(expression))
                return this._primitiveLabels[expression].Clone();
            double constValue;
            if (double.TryParse(expression, out constValue))
                return new Constant(constValue);

            // tests for functions
            foreach (var functionPattern in this._functionPatterns)
            {
                // checks if pattern matches
                var subExprs = RegexSplit(expression, functionPattern.Key);
                var numChildren = functionPattern.Value.Children.Count;
                if (subExprs.Count != numChildren || subExprs.Count == 1 && string.Equals(subExprs[0], expression))
                    continue;

                // tries to parse sub-expressions as children
                var children = new IElement[numChildren];
                var invalid = false;
                for (var i = 0; i < numChildren; i++)
                {
                    var child = FromNormalNotation(subExprs[i]);
                    if (child == null)
                    {
                        // if a child's expression is invalid, break and try next
                        invalid = true;
                        break;
                    }
                    children[i] = child;
                }
                if (invalid) continue;

                // creates a new element
                return functionPattern.Value.CreateNew(children);
            }
            return null;
        }

        public IElement FromPrefixNotation(string prefixNotationExpression)
        {
            // cleans expression
            if (string.IsNullOrWhiteSpace(prefixNotationExpression)) return null;
            foreach (var superfluousStr in this._parentheses)
                prefixNotationExpression = prefixNotationExpression.Replace(superfluousStr, string.Empty);
            if (string.IsNullOrEmpty(prefixNotationExpression)) return null;

            // builds element from expression
            var index = 0u;
            var expression = prefixNotationExpression.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            var elem = this.FromPrefixNotation(expression, ref index);
            return index != expression.Length - 1 ? null : elem;
        }

        public string ToPrefixNotation(IElement element, bool includeParentheses = true)
        {
            if (element == null) return null;

            // if is a terminal just return the label
            if (element.Children == null || element.Children.Count == 0) return GetName(element);

            // if a function, create an expression in prefix notation
            var strBuilder = new StringBuilder(includeParentheses ? "(" : string.Empty);
            strBuilder.AppendFormat("{0} ", GetName(element));
            foreach (var child in element.Children)
                strBuilder.AppendFormat("{0} ", ToPrefixNotation(child, includeParentheses));
            strBuilder.Remove(strBuilder.Length - 1, 1);
            strBuilder.Append(includeParentheses ? ")" : string.Empty);
            return strBuilder.ToString();
        }

        #endregion

        #region Private & Protected Methods

        private static string GetFunctionPattern(IElement primitive, DummyTerminal dummy)
        {
            // creates a new function element whose children are dummies
            var children = new IElement[primitive.Children?.Count ?? 0];
            for (var i = 0; i < children.Length; i++)
                children[i] = dummy;
            var dummyPrimitive = primitive.CreateNew(children);

            // replaces dummy labels to get elements of the expression
            var expression = dummyPrimitive.Expression;
            var exprElements = Regex.Split(expression, $"{dummy.Label}");

            // creates a regex splitting pattern
            var pattern = new StringBuilder();
            foreach (var exprElement in exprElements)
            {
                var correctedParenthesis = exprElement.Replace("(", @"\(");
                var subPattern = (correctedParenthesis.Length == 1 ? @"\" : string.Empty) + correctedParenthesis;
                pattern.Append($"{subPattern}{SUB_ELEM_PATTERN}");
            }
            if (exprElements.Length > 0)
                pattern.Remove(pattern.Length - SUB_ELEM_PATTERN.Length, SUB_ELEM_PATTERN.Length);
            return pattern.ToString();
        }

        private static string GetName(IElement element)
        {
            return element.Label;
        }

        private static IList<string> RegexSplit(string expression, string pattern)
        {
            // applies pattern, removes empty entries
            var subExprs = Regex.Split(expression, pattern);
            return subExprs.Where(s => !string.IsNullOrEmpty(s)).ToList();
        }

        private IElement FromPrefixNotation(string[] expression, ref uint index)
        {
            if (expression == null || index >= expression.Length)
                return null;

            // checks if primitive set contains element or is a numerical constant
            IElement elem;
            double constValue;
            if (this._primitiveLabels.ContainsKey(expression[index]))
                elem = this._primitiveLabels[expression[index]];
            else if (double.TryParse(expression[index], out constValue))
                elem = new Constant(constValue);
            else
                return null;

            // reads children if element is a function
            var numChildren = elem.Children.Count;
            var children = new IElement[numChildren];
            for (var i = 0; i < numChildren; i++)
            {
                index++;
                var child = FromPrefixNotation(expression, ref index);
                if (child == null) return null;
                children[i] = child;
            }
            return elem.CreateNew(children);
        }

        #endregion

        #region Nested type: DummyTerminal

        private class DummyTerminal : Terminal
        {
            #region Properties & Indexers

            public override string Label { get; }

            #endregion

            #region Constructors

            public DummyTerminal(string label)
            {
                this.Label = label;
            }

            #endregion

            #region Public Methods

            public override IElement Clone()
            {
                return new DummyTerminal(this.Label);
            }

            public override double GetValue() => 0;

            #endregion
        }

        #endregion

        #region IDisposable Support

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (this._disposed) return;
            if (disposing) this._primitiveLabels.Clear();
            this._disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
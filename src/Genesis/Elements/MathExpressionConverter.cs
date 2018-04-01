// ------------------------------------------
// <copyright file="MathExpressionConverter.cs" company="Pedro Sequeira">
// 
//     Copyright (c) 2018 Pedro Sequeira
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to the following conditions:
//  
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS
// OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// </copyright>
// <summary>
//    Project: Genesis
//    Last updated: 03/31/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Genesis.Elements.Terminals;

namespace Genesis.Elements
{
    /// <summary>
    ///     Represents a class include a set of methods to convert <see cref="MathProgram" /> objects to and from
    ///     <see cref="string" /> expressions in different notations.
    /// </summary>
    public class MathExpressionConverter : ITreeExpressionConverter<MathProgram>, IDisposable
    {
        #region Static Fields & Constants

        private const string SUB_ELEM_PATTERN = @"((?>[^\(\)]+|(?<l>\()|(?<-l>\)))+(?(l)(?!))|[^\(\)]+)";

        #endregion

        #region Fields

        private readonly Dictionary<string, MathProgram> _functionPatterns = new Dictionary<string, MathProgram>();

        private readonly string[] _parentheses = {"(", ")"};

        private readonly Dictionary<string, MathProgram> _primitiveLabels = new Dictionary<string, MathProgram>();

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="MathExpressionConverter" /> according to the given primitive set.
        /// </summary>
        /// <param name="primitives">The set containing the primitives that the converter can read from the expressions.</param>
        public MathExpressionConverter(PrimitiveSet<MathProgram> primitives)
        {
            var allPrimitives = new List<MathProgram>(primitives.Functions);
            allPrimitives.AddRange(primitives.Terminals);

            // stores info on all primitive labels
            foreach (var primitive in allPrimitives)
                this._primitiveLabels.Add(primitive.Label, primitive);

            // stores regex patterns for functions
            var rndLabel = DateTime.Now.ToString("yyyyMMMMddHHmmssfff");
            var dummy = new Variable(rndLabel);
            foreach (var function in primitives.Functions.ToList())
            {
                var pattern = GetFunctionPattern(function, dummy);
                this._functionPatterns.Add(pattern, function);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Converts the given <see cref="MathProgram" /> to a <see cref="string" /> expression in prefix notation, i.e., where
        ///     functions are written in the form (func arg1 arg2 ...).
        /// </summary>
        /// <param name="program">The program we want to convert to an expression.</param>
        /// <param name="includeParentheses">
        ///     Whether to write opening '(' and closing ')' parentheses when writing the expression of functions.
        /// </param>
        /// <returns>A <see cref="string" /> representing the given program in prefix notation.</returns>
        public static string ToPrefixNotation(MathProgram program, bool includeParentheses = true)
        {
            if (program == null) return null;

            // if is a terminal just return the label
            if (program.Input == null || program.Input.Count == 0) return GetName(program);

            // if a function, create an expression in prefix notation
            var strBuilder = new StringBuilder(includeParentheses ? "(" : string.Empty);
            strBuilder.AppendFormat("{0} ", GetName(program));
            foreach (var child in program.Input)
                strBuilder.AppendFormat("{0} ", ToPrefixNotation((MathProgram) child, includeParentheses));
            strBuilder.Remove(strBuilder.Length - 1, 1);
            strBuilder.Append(includeParentheses ? ")" : string.Empty);
            return strBuilder.ToString();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this._primitiveLabels.Clear();
        }

        /// <inheritdoc />
        /// <remarks>Equivalent to <see cref="FromPrefixNotation" />.</remarks>
        public MathProgram FromString(string programStr) => this.FromPrefixNotation(programStr);

        /// <inheritdoc />
        /// <remarks>Equivalent to <see cref="ToPrefixNotation" />.</remarks>
        public string ToString(MathProgram program) => ToPrefixNotation(program, false);

        /// <summary>
        ///     Converts the given <see cref="string" /> expression written in normal notation, i.e., where functions are written
        ///     in the form func(arg1 arg2 ...) or (arg1 func arg2) to a <see cref="MathProgram" />.
        /// </summary>
        /// <param name="expression">The expression we want to convert to an program.</param>
        /// <returns>A <see cref="MathProgram" /> converted from the given expression.</returns>
        public MathProgram FromNormalNotation(string expression)
        {
            if (expression == null) return null;

            // cleans expression
            expression = expression.Replace(" ", string.Empty);

            // tests for terminals
            if (this._primitiveLabels.ContainsKey(expression))
                return this._primitiveLabels[expression];
            if (double.TryParse(expression, out var constValue))
                return new Constant(constValue);

            // tests for functions
            foreach (var functionPattern in this._functionPatterns)
            {
                // checks if pattern matches
                var subExprs = RegexSplit(expression, functionPattern.Key);
                var numChildren = functionPattern.Value.Input.Count;
                if (subExprs.Count != numChildren || subExprs.Count == 1 && string.Equals(subExprs[0], expression))
                    continue;

                // tries to parse sub-expressions as children
                var children = new ITreeProgram<double>[numChildren];
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

                // creates a new program
                return (MathProgram) functionPattern.Value.CreateNew(children);
            }

            return null;
        }

        /// <summary>
        ///     Converts the given <see cref="string" /> expression written in prefix notation, i.e., where functions are written
        ///     in the form (func arg1 arg2 ...) to a <see cref="MathProgram" />.
        /// </summary>
        /// <param name="expression">The expression we want to convert to an program.</param>
        /// <returns>A <see cref="MathProgram" /> converted from the given expression.</returns>
        public MathProgram FromPrefixNotation(string expression)
        {
            // cleans expression
            if (string.IsNullOrWhiteSpace(expression)) return null;
            foreach (var superfluousStr in this._parentheses)
                expression = expression.Replace(superfluousStr, string.Empty);
            if (string.IsNullOrEmpty(expression)) return null;

            // builds program from expression
            var index = 0u;
            var expr = expression.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            var prog = this.FromPrefixNotation(expr, ref index);
            return index != expr.Length - 1 ? default(MathProgram) : prog;
        }

        /// <inheritdoc />
        public string ToNormalNotation(MathProgram program, bool includeParentheses = true) => program.ToString();

        #endregion

        #region Private & Protected Methods

        private static string GetFunctionPattern(MathProgram primitive, Variable dummy)
        {
            // creates a new function program whose children are dummies
            var children = new ITreeProgram<double>[primitive.Input?.Count ?? 0];
            for (var i = 0; i < children.Length; i++)
                children[i] = dummy;
            var dummyPrimitive = primitive.CreateNew(children);

            // replaces dummy labels to get programs of the expression
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

        private static string GetName(MathProgram program) => program.Label;

        private static IList<string> RegexSplit(string expression, string pattern)
        {
            // applies pattern, removes empty entries
            var subExprs = Regex.Split(expression, pattern);
            return subExprs.Where(s => !string.IsNullOrEmpty(s)).ToList();
        }

        private MathProgram FromPrefixNotation(string[] expression, ref uint index)
        {
            if (expression == null || index >= expression.Length)
                return null;

            // checks if primitive set contains program or is a numerical constant
            ITreeProgram<double> prog;
            if (this._primitiveLabels.ContainsKey(expression[index]))
                prog = this._primitiveLabels[expression[index]];
            else if (
                double.TryParse(expression[index], NumberStyles.Any, CultureInfo.InvariantCulture,
                    out var constValue) ||
                double.TryParse(expression[index], out constValue))
                prog = new Constant(constValue);
            else
                return null;

            // reads children if program is a function
            var numChildren = prog.Input.Count;
            var children = new ITreeProgram<double>[numChildren];
            for (var i = 0; i < numChildren; i++)
            {
                index++;
                var child = FromPrefixNotation(expression, ref index);
                if (child == null) return null;
                children[i] = child;
            }

            return (MathProgram) prog.CreateNew(children);
        }

        string ITreeExpressionConverter<MathProgram>.ToPrefixNotation(MathProgram program, bool includeParentheses) =>
            ToPrefixNotation(program, includeParentheses);

        #endregion
    }
}
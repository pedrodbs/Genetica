// ------------------------------------------
// <copyright file="StringUtil.cs" company="Pedro Sequeira">
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
//    Project: Genetica
//    Last updated: 03/23/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.CodeDom;
using System.IO;
using System.Text;
using Microsoft.CSharp;

namespace Genetica.Util
{
    public static class StringUtil
    {
        #region Public Methods

        /// <summary>
        ///     Gets the edit or Levenshtein distance between two strings. The Damereau-Levenshein Distance algorithm calculates
        ///     the number of letter additions, subtractions, substitutions, and transpositions (swaps) necessary to convert one
        ///     string to another. The lower the score, the more similar they are.
        /// </summary>
        /// <param name="a">The first string.</param>
        /// <param name="b">The second string.</param>
        /// <returns>The edit or Levenshtein distance between the given strings.</returns>
        /// <remarks>
        ///     Code based on:
        ///     <a
        ///         href="https://stackoverflow.com/questions/9453731/how-to-calculate-distance-similarity-measure-of-given-2-strings" />
        /// </remarks>
        public static int LevenshteinDistance(this string a, string b)
        {
            if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b)) return 0;

            var lengthA = a.Length;
            var lengthB = b.Length;
            var distances = new int[lengthA + 1, lengthB + 1];
            for (var i = 0; i <= lengthA; distances[i, 0] = i++) ;
            for (var j = 0; j <= lengthB; distances[0, j] = j++) ;

            for (var i = 1; i <= lengthA; i++)
            for (var j = 1; j <= lengthB; j++)
            {
                var cost = b[j - 1] == a[i - 1] ? 0 : 1;
                distances[i, j] =
                    Math.Min(
                        Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
                        distances[i - 1, j - 1] + cost
                    );
            }
            return distances[lengthA, lengthB];
        }

        public static string Repeat(this string str, uint num)
        {
            var sb = new StringBuilder();
            for (var i = 0u; i < num; i++)
                sb.Append(str);
            return sb.ToString();
        }

        public static string Repeat(this char c, uint num)
        {
            return new string(c, (int) num);
        }

        public static string ToLiteral(this string input)
        {
            var writer = new StringWriter();
            var provider = new CSharpCodeProvider();
            provider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, null);
            return writer.GetStringBuilder().ToString().Replace("\"", string.Empty);
        }

        #endregion
    }
}
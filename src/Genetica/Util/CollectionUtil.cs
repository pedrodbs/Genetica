// ------------------------------------------
// <copyright file="CollectionUtil.cs" company="Pedro Sequeira">
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
//    Last updated: 03/31/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Random;

namespace Genetica.Util
{
    /// <summary>
    ///     Provides several utility methods to create and update collection objects.
    /// </summary>
    public static class CollectionUtil
    {
        #region Static Fields & Constants

        private static readonly Random Random = new WH2006(RandomSeed.Robust());

        #endregion

        #region Public Methods

        /// <summary>
        ///     Adds the given values to the dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of key stored in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of value stored in the dictionary.</typeparam>
        /// <param name="set">The dictionary we want to add the values to.</param>
        /// <param name="values">The values to be added to the dictionary.</param>
        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> set, IDictionary<TKey, TValue> values)
        {
            foreach (var val in values) set.Add(val);
        }

        /// <summary>
        ///     Adds the given values to the dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of key stored in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of value stored in the dictionary.</typeparam>
        /// <param name="set">The dictionary we want to add the values to.</param>
        /// <param name="values">The values to be added to the dictionary.</param>
        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> set,
            IEnumerable<KeyValuePair<TKey, TValue>> values)
        {
            foreach (var val in values) set.Add(val);
        }

        /// <summary>
        ///     Adds the given values to the given set.
        /// </summary>
        /// <typeparam name="T">The type of items in the collections.</typeparam>
        /// <param name="set">The set we want to add the values to.</param>
        /// <param name="values">The values to be added to the set.</param>
        public static void AddRange<T>(this ISet<T> set, IEnumerable<T> values)
        {
            foreach (var val in values) set.Add(val);
        }

        /// <summary>
        ///     Adds the given item to the sorted list at the correct index.
        /// </summary>
        /// <remarks>
        ///     This method assumes the given list is already sorted.
        ///     <see href="https://stackoverflow.com/a/22801345" />
        /// </remarks>
        /// <param name="list">The sorted list onto which we want to add the given item.</param>
        /// <param name="item">The item to add to list.</param>
        /// <returns>The index where the given item was added.</returns>
        /// <typeparam name="T">The type of items in the list</typeparam>
        public static int AddSorted<T>(this List<T> list, T item) where T : IComparable<T>
        {
            if (list.Count == 0)
            {
                list.Add(item);
                return 0;
            }

            if (list[list.Count - 1].CompareTo(item) <= 0)
            {
                list.Add(item);
                return list.Count - 1;
            }

            if (list[0].CompareTo(item) >= 0)
            {
                list.Insert(0, item);
                return 0;
            }

            var index = list.BinarySearch(item);
            if (index < 0) index = ~index;
            list.Insert(index, item);
            return index;
        }

        /// <summary>
        ///     Modifies the given lists by aligning their items so that equal items have the same index.
        /// </summary>
        /// <typeparam name="T">The type of items stored in the given lists.</typeparam>
        /// <param name="list1">The first list.</param>
        /// <param name="list2">The second list.</param>
        public static void Align<T>(IList<T> list1, IList<T> list2)
        {
            var minCount = Math.Min(list1.Count, list2.Count);
            for (var i = 0; i < minCount; i++)
            {
                // for each item in 1, see if equal item exists in 2
                var idx2 = list2.IndexOf(list1[i]);
                if (idx2 == -1) continue;

                // swaps items in 2 so that common items are aligned
                var child2 = list2[idx2];
                list2[idx2] = list1[i];
                list2[i] = child2;
            }
        }

        /// <summary>
        ///     Disposes of all items in the given collection.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection, has to implement <see cref="IDisposable" />.</typeparam>
        /// <param name="collection">The collection whose items we want to dispose.</param>
        public static void Dispose<T>(this IEnumerable<T> collection)
            where T : IDisposable
        {
            foreach (var item in collection) item?.Dispose();
        }

        /// <summary>
        ///     Gets an IList representing all combinations of <typeparamref name="T" /> items from the provided lists.
        ///     Combinations are created in the following form: [item from the 1st list, item from the 2nd list , ..., item
        ///     from the nth list].
        /// </summary>
        /// <returns>A list containing all combinations of items.</returns>
        /// <param name="list">The list containing the lists of items to combine.</param>
        /// <typeparam name="T">The type of items in the lists.</typeparam>
        public static IEnumerable<IList<T>> GetAllCombinations<T>(this IList<IEnumerable<T>> list)
        {
            var combinations = new List<IList<T>> {new T[list.Count]};
            for (var i = 0; i < list.Count; i++)
            {
                var newCombs = new List<IList<T>>();
                foreach (var item in list[i])
                foreach (var combination in combinations)
                {
                    var newComb = combination.ToArray();
                    newComb[i] = item;
                    newCombs.Add(newComb);
                }

                combinations = newCombs;
            }

            return combinations;
        }

        /// <summary>
        ///     Gets a random item from the list.
        /// </summary>
        /// <returns>The random item.</returns>
        /// <param name="list">The list we want to get a random item from.</param>
        /// <param name="random">The random number generator to be used in the item selection.</param>
        /// <typeparam name="T">The type of items stored in the list.</typeparam>
        public static T GetRandomItem<T>(this IList<T> list, Random random)
        {
            return list[random.Next(list.Count)];
        }

        /// <summary>
        ///     Gets a random item from the list. Uses a static random number generator.
        /// </summary>
        /// <returns>The random item.</returns>
        /// <param name="list">The list we want to get a random item from.</param>
        /// <typeparam name="T">The type of items stored in the list.</typeparam>
        public static T GetRandomItem<T>(this IList<T> list)
        {
            return GetRandomItem(list, Random);
        }

        /// <summary>
        ///     Gets an item from the given dictionary keys, randomly sampled according to the probabilities defined by the
        ///     corresponding values in the dictionary.
        /// </summary>
        /// <returns>The random item.</returns>
        /// <param name="dict">
        ///     The dictionary we want to get a random item from. Assumes <paramref name="dict" />.Values.
        ///     Sum() == 1.
        /// </param>
        /// <param name="random">The random source used in this operation.</param>
        /// <typeparam name="T">The type of items stored in the dictionary keys.</typeparam>
        public static T GetRandomItem<T>(this IDictionary<T, double> dict, Random random)
        {
            var rnd = random.NextDouble();
            var sum = 0d;
            foreach (var item in dict.Keys)
            {
                sum += dict[item];
                if (sum > rnd) return item;
            }

            return dict.Count > 0 ? dict.Keys.First() : default(T);
        }

        /// <summary>
        ///     Gets an item from the given dictionary keys, randomly sampled according to the probabilities defined by the
        ///     corresponding values in the dictionary. Uses a static random number generator.
        /// </summary>
        /// <returns>The random item.</returns>
        /// <param name="dict">
        ///     The dictionary we want to get a random item from. Assumes <paramref name="dict" />.Values.
        ///     Sum() == 1.
        /// </param>
        /// <typeparam name="T">The type of items stored in the dictionary keys.</typeparam>
        public static T GetRandomItem<T>(this IDictionary<T, double> dict)
        {
            return GetRandomItem(dict, Random);
        }

        /// <summary>
        ///     Gets a subset of the given list between the given indexes.
        /// </summary>
        /// <typeparam name="T">The type of items stored in the given collection.</typeparam>
        /// <param name="collection">The original collection that we want to get a sub-set.</param>
        /// <param name="startIdex">The initial index of the collection sub-set.</param>
        /// <param name="finalIndex">The final index of the collection sub-set.</param>
        /// <returns>The subset of the given list.</returns>
        public static IList<T> GetSubset<T>(
            this IList<T> collection, int startIdex = 0, int finalIndex = int.MaxValue)
        {
            if (collection == null) return null;
            var length = Math.Max(finalIndex, collection.Count) - startIdex;
            if (length <= 0) return null;

            var subSet = new T[length];
            Array.Copy(collection.ToArray(), startIdex, subSet, 0, length);
            return subSet;
        }

        /// <summary>
        ///     Gets a list of items which is equivalent to a given list (in terms Equals and GetHashCode) but where the items are
        ///     replaced by the corresponding objects referenced in the given dictionray.
        /// </summary>
        /// <typeparam name="T">The type of items stored in the collection.</typeparam>
        /// <param name="list">The list containing the collection of items.</param>
        /// <param name="allItems">A table where items can be fetched by hashcode.</param>
        /// <returns>A list of items which is equivalent to a given list.</returns>
        /// <remarks>
        ///     This method is useful when we want to make sure we don't have useless copies of the same objects (as dictated
        ///     by GetHasCode() and Equals()) stored in different places.
        /// </remarks>
        public static IList<T> GetUniqueItems<T>(this IList<T> list, IDictionary<T, T> allItems)
        {
            var items = new T[list.Count];
            for (var i = 0; i < list.Count; i++)
            {
                var item = list[i];
                if (allItems.ContainsKey(item))
                {
                    items[i] = allItems[item];
                }
                else
                {
                    allItems.Add(item, item);
                    items[i] = item;
                }
            }

            return items;
        }

        /// <summary>
        ///     Shuffles the given list so that the items are sorted in a random way.
        /// </summary>
        /// <typeparam name="T">The type of items stored in the given collection.</typeparam>
        /// <param name="list">The original list that we want to shuffle.</param>
        /// <param name="random">The random number generator used in the shuffling process.</param>
        /// <remarks>
        ///     Based on the Fisher-Yates shuffle as stated in the solution:
        ///     <see href="https://stackoverflow.com/questions/273313/randomize-a-listt" />
        /// </remarks>
        public static void Shuffle<T>(this IList<T> list, Random random)
        {
            if (random == null) random = new Random();
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = random.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        ///     Shuffles the given list so that the items are sorted in a random way. Uses a static random number generator.
        /// </summary>
        /// <typeparam name="T">The type of items stored in the given collection.</typeparam>
        /// <param name="list">The original list that we want to shuffle.</param>
        /// <remarks>
        ///     Based on the Fisher-Yates shuffle as stated in the solution:
        ///     <see href="https://stackoverflow.com/questions/273313/randomize-a-listt" />
        /// </remarks>
        public static void Shuffle<T>(this IList<T> list)
        {
            list.Shuffle(Random);
        }

        #endregion
    }
}
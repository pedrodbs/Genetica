// ------------------------------------------
// <copyright file="Population.cs" company="Pedro Sequeira">
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
using System.Linq;
using Genesis.Elements;
using Genesis.Operators.Crossover;
using Genesis.Operators.Generation;
using Genesis.Operators.Mutation;
using Genesis.Operators.Selection;
using MathNet.Numerics.Random;

namespace Genesis
{
    /// <summary>
    ///     Represents a simple implementation of <see cref="IPopulation{TProgram}" /> composed of
    ///     <see cref="ITreeProgram{TOutput}" />. The algorithm follows a traditional evolutionary algorithm to step one
    ///     generation:
    ///     1 - performs selection to get a pool of <em>n</em> parents for crossover, where <em>n</em> is the size of the
    ///     population;
    ///     2 - performs crossover from parent pool to get some offspring (given percentage of the population);
    ///     3 - performs mutation from parent pool to get some mutated programs (given percentage of the population);
    ///     4 - performs elite selection (keeps a given percentage of the population corresponding to the best programs);
    ///     5 - creates some random programs (given percentage of the population).
    /// </summary>
    /// <typeparam name="TProgram"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public class Population<TProgram, TOutput> : HashSet<TProgram>, IPopulation<TProgram>
        where TProgram : ITreeProgram<TOutput>
    {
        #region Fields

        private readonly ICrossoverOperator<TProgram> _crossoverOperator;
        private readonly uint _maxElementLength;
        private readonly uint _maxGenerationDepth;
        private readonly uint _maxSize;
        private readonly IMutationOperator<TProgram> _mutationOperator;
        private readonly PrimitiveSet<TProgram> _primitives;
        private readonly IComparer<TProgram> _programComparer;
        private readonly IProgramGenerator<TProgram, TOutput> _programGenerator;
        private readonly Random _random = new WH2006(RandomSeed.Robust());
        private readonly ISelectionOperator<TProgram> _selectionOperator;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates a new <see cref="Population{TProgram,TOutput}" /> with the given arguments.
        /// </summary>
        /// <param name="maxSize">The maximum size of the population.</param>
        /// <param name="primitives">The primitive set used to generate new programs.</param>
        /// <param name="programGenerator">The generator of new programs.</param>
        /// <param name="programComparer">The function used to compare programs and select the best program.</param>
        /// <param name="selectionOperator">The operator to perform selection.</param>
        /// <param name="crossoverOperator">The operator to crossover programs. </param>
        /// <param name="mutationOperator">The operator to mutate programs.</param>
        /// <param name="maxGenerationDepth">The maximum depth of elements generated during GP.</param>
        /// <param name="maxElementLength">The maximum length of elements generated during GP.</param>
        /// <param name="crossoverPercent">The percentage of a population used for the crossover operator during GP.</param>
        /// <param name="mutationPercent">The percentage of a population used for the mutation operator during GP.</param>
        /// <param name="elitismPercent">The percentage of a population used for elite selection during GP.</param>
        public Population(
            uint maxSize,
            PrimitiveSet<TProgram> primitives,
            IProgramGenerator<TProgram, TOutput> programGenerator,
            IComparer<TProgram> programComparer,
            ISelectionOperator<TProgram> selectionOperator,
            ICrossoverOperator<TProgram> crossoverOperator,
            IMutationOperator<TProgram> mutationOperator,
            uint maxGenerationDepth = 4,
            uint maxElementLength = 20,
            double crossoverPercent = 0.65d,
            double mutationPercent = 0.2d,
            double elitismPercent = 0.1d)
        {
            this._maxSize = maxSize;
            this._primitives = primitives;
            this._programGenerator = programGenerator;
            this._maxGenerationDepth = maxGenerationDepth;
            this._maxElementLength = maxElementLength;
            this._programComparer = programComparer;
            this._selectionOperator = selectionOperator;
            this._mutationOperator = mutationOperator;
            this._crossoverOperator = crossoverOperator;
            this.ElitismPercent = elitismPercent;
            this.MutationPercent = mutationPercent;
            this.CrossoverPercent = crossoverPercent;
        }

        #endregion

        #region Properties & Indexers

        /// <inheritdoc />
        public TProgram BestProgram { get; private set; }

        /// <inheritdoc />
        public double CrossoverPercent { get; set; }

        /// <inheritdoc />
        public double ElitismPercent { get; set; }

        /// <inheritdoc />
        public double MutationPercent { get; set; }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public void Dispose()
        {
            this.Clear();
        }

        /// <inheritdoc />
        public virtual void Step()
        {
            // checks initialization
            if (this.Count == 0) this.Init(null);

            var newGeneration = new List<TProgram>((int) this._maxSize);

            // 1 - performs selection to get pool of parents for crossover
            var selection = new List<TProgram>(this._selectionOperator.Select(this));

            // 2 - performs crossover to get some offspring
            var numOffspring = (int) (this.CrossoverPercent * this._maxSize);
            for (var i = 0; i < numOffspring; i++)
            {
                //randomly selects 2 parents (may be equal)
                var parent1 = selection[this._random.Next(selection.Count)];
                var parent2 = selection[this._random.Next(selection.Count)];
                var descendant = this._crossoverOperator.Crossover(parent1, parent2);
                if (descendant.Length <= this._maxElementLength)
                    newGeneration.Add(descendant);
            }

            // 3 - performs mutation from selection
            var numMutations = (int) (this.MutationPercent * this._maxSize);
            for (var i = 0; i < numMutations; i++)
            {
                //randomly selects program
                var program = selection[this._random.Next(selection.Count)];
                newGeneration.Add(this._mutationOperator.Mutate(program));
            }

            // 4 - performs elite selection (keeps some best programs)
            var numElite = (int) (this.ElitismPercent * this._maxSize);
            var j = 0;
            foreach (var program in this.Reverse())
            {
                if (j++ >= numElite) break;
                newGeneration.Add(program);
            }

            // 5 - creates random programs
            for (var i = newGeneration.Count; i < this._maxSize; i++)
                newGeneration.Add(this._programGenerator.Generate(this._primitives, this._maxGenerationDepth));

            // 6 - replace population with new generation
            this.Clear();
            foreach (var program in newGeneration)
                this.Add(program);

            newGeneration.Clear();
            selection.Clear();

            // gets best program
            foreach (var program in this)
                if (this.BestProgram == null || this._programComparer.Compare(program, this.BestProgram) > 0)
                    this.BestProgram = program;
        }

        /// <inheritdoc />
        public void Init(ISet<TProgram> seeds)
        {
            // clear pop
            this.Clear();

            // add seeds directly to the initial pop
            if (seeds != null)
            {
                var i = 0;
                foreach (var prog in seeds)
                    if (i++ < this._maxSize)
                        this.Add(prog);
            }

            // creates new programs
            for (var i = this.Count; i < this._maxSize; i++)
            {
                TProgram program;
                do
                {
                    program = this._programGenerator.Generate(this._primitives, this._maxGenerationDepth);
                } while (this.Contains(program));

                this.Add(program);

                // gets best program
                if (this.BestProgram == null || this._programComparer.Compare(program, this.BestProgram) > 0)
                    this.BestProgram = program;
            }
        }

        #endregion
    }
}
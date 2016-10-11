/* ***************************************************************************
 * This file is part of SharpNEAT - Evolution of Neural Networks.
 * 
 * Copyright 2004-2016 Colin Green (sharpneat@gmail.com)
 *
 * SharpNEAT is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * SharpNEAT is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with SharpNEAT.  If not, see <http://www.gnu.org/licenses/>.
 */

using System.Collections.Generic;
using SharpNeat.Core;

// Disable missing comment warnings for non-private variables.
#pragma warning disable 1591

namespace SharpNeat.EvolutionAlgorithms
{
    /// <summary>
    /// Abstract class providing some common/baseline data and methods for implementions of IEvolutionAlgorithm.
    /// </summary>
    /// <typeparam name="TGenome">The genome type that the algorithm will operate on.</typeparam>
    public abstract class AbstractGenerationalAlgorithm<TGenome> :
        IEvolutionAlgorithm<TGenome>
        where TGenome : class, IGenome<TGenome>
    {
        protected IGenomeListEvaluator<TGenome> _genomeListEvaluator;
        protected IGenomeFactory<TGenome> _genomeFactory;
        protected List<TGenome> _genomeList;
        protected int _populationSize;
        protected TGenome _currentBestGenome;
        protected uint _currentGeneration;

        /// <summary>
        /// Gets the current generation.
        /// </summary>
        public uint CurrentGeneration
        {
            get { return _currentGeneration; }
        }

        /// <summary>
        /// Gets a list of all current genomes. The current population of genomes. These genomes
        /// are also divided into the species available through the SpeciesList property.
        /// </summary>
        public IList<TGenome> GenomeList
        {
            get { return _genomeList; }
        }

        /// <summary>
        /// Gets the population's current champion genome.
        /// </summary>
        public TGenome CurrentChampGenome
        {
            get { return _currentBestGenome; }
        }

        /// <summary>
        /// Gets a value indicating whether some goal fitness has been achieved and that the algorithm has therefore stopped.
        /// </summary>
        public bool StopConditionSatisfied
        {
            get { return _genomeListEvaluator.StopConditionSatisfied; }
        }

        /// <summary>
        /// Initializes the evolution algorithm with the provided IGenomeListEvaluator, IGenomeFactory
        /// and an initial population of genomes.
        /// </summary>
        /// <param name="genomeListEvaluator">The genome evaluation scheme for the evolution algorithm.</param>
        /// <param name="genomeFactory">The factory that was used to create the genomeList and which is therefore referenced by the genomes.</param>
        /// <param name="genomeList">An initial genome population.</param>
        public virtual void Initialize(IGenomeListEvaluator<TGenome> genomeListEvaluator,
            IGenomeFactory<TGenome> genomeFactory,
            List<TGenome> genomeList)
        {
            _currentGeneration = 0;
            _genomeListEvaluator = genomeListEvaluator;
            _genomeFactory = genomeFactory;
            _genomeList = genomeList;
            _populationSize = _genomeList.Count;
        }

        /// <summary>
        /// Initializes the evolution algorithm with the provided IGenomeListEvaluator
        /// and an IGenomeFactory that can be used to create an initial population of genomes.
        /// </summary>
        /// <param name="genomeListEvaluator">The genome evaluation scheme for the evolution algorithm.</param>
        /// <param name="genomeFactory">The factory that was used to create the genomeList and which is therefore referenced by the genomes.</param>
        /// <param name="populationSize">The number of genomes to create for the initial population.</param>
        public virtual void Initialize(
            IGenomeListEvaluator<TGenome> genomeListEvaluator,
            IGenomeFactory<TGenome> genomeFactory,
            int populationSize)
        {
            var list = genomeFactory.CreateGenomeList(populationSize, _currentGeneration);

            Initialize(genomeListEvaluator, genomeFactory, list);
        }

        public void Execute()
        {
            PerformOneGeneration();

            _currentGeneration++;
        }

        /// <summary>
        /// Progress forward by one generation. Perform one generation/cycle of the evolution algorithm.
        /// </summary>
        protected abstract void PerformOneGeneration();
    }
}

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

namespace SharpNeat.Core
{
    /// <summary>
    /// A generic interface for evolution algorithm classes.
    /// </summary>
    public interface IEvolutionAlgorithm<TGenome>:
        IAlgorithm
        where TGenome : class, IGenome<TGenome>
    {
        /// <summary>
        /// Gets the population's current champion genome.
        /// </summary>
        TGenome CurrentChampGenome { get; }

        /// <summary>
        /// Gets the current population.
        /// </summary>
        IList<TGenome> GenomeList { get; }

        /// <summary>
        /// Initializes the evolution algorithm with the provided IGenomeListEvaluator, IGenomeFactory
        /// and an initial population of genomes.
        /// </summary>
        void Initialize(
            IGenomeListEvaluator<TGenome> genomeListEvaluator,
            IGenomeFactory<TGenome> genomeFactory,
            List<TGenome> genomeList);

        /// <summary>
        /// Initializes the evolution algorithm with the provided IGenomeListEvaluator
        /// and an IGenomeFactory that can be used to create an initial population of genomes.
        /// </summary>
        void Initialize(
            IGenomeListEvaluator<TGenome> genomeListEvaluator,
            IGenomeFactory<TGenome> genomeFactory,
            int populationSize);
    }
}

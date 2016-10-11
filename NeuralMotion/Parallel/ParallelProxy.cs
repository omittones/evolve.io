using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralMotion.Parallel
{
    public static class ParallelProxy
    {
        public static int GlobalDegreeOfParallelism { get; set; }

        static ParallelProxy()
        {
            GlobalDegreeOfParallelism = Environment.ProcessorCount;
        }

        public static ParallelQuery<TSource> AsParallelEx<TSource>(this IEnumerable<TSource> source)
        {
            return ParallelEnumerable
                .AsParallel(source)
                .WithDegreeOfParallelism(GlobalDegreeOfParallelism);
        }
    }
}
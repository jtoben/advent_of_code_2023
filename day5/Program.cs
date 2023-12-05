using System.Collections.Concurrent;

namespace day5
{
    internal class Program
    {
        static void Main()
        {
            var input = File.ReadAllText("input.txt")
                .Split(new string[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            var seedsString = input[0]
                .Split(": ")[1];

            var sourceToDestinationMaps = input
                .Skip(1)
                .Select(map => new Map(map
                    .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                    .Skip(1)
                    .Select(line => {
                        var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                        var length = long.Parse(parts[2]);

                        return new Mapping(new Range(long.Parse(parts[1]), length), new Range(long.Parse(parts[0]), length));
                    }).ToList()))
                .ToList();

            Console.WriteLine(PartOne(seedsString, sourceToDestinationMaps));
            Console.WriteLine(PartTwo(seedsString, sourceToDestinationMaps));
        }

        private static long PartOne(string seedsString, List<Map> sourceToDestinationMaps)
        {
            List<long> seeds = seedsString
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .ToList();

            long smallestDestination = long.MaxValue;
            foreach (var seed in seeds) {
                long previousDestination = seed;
                foreach (var map in sourceToDestinationMaps) {
                    previousDestination = map.GetDestination(previousDestination);
                }

                if (previousDestination < smallestDestination) {
                    smallestDestination = previousDestination;
                }
            }

            return smallestDestination;
        }

        private static long PartTwo(string seedsString, List<Map> sourceToDestinationMaps)
        {
            var seedPairs = seedsString
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .Select((Seed, Index) => new { Seed, Index })
                .GroupBy(tuple => tuple.Index / 2, tuple => tuple.Seed)
                .ToList();

            ConcurrentBag<long> smallesDestinations = [];

            List<Task> tasks = [];
            for (int i = 0; i < seedPairs.Count; i++) {
                int localCount = i;
                var task = new Task(() => {
                    var seedPair = seedPairs[localCount];

                    List<long> localSmallestDestinations = [];
                    for (long seed = seedPair.First(); seed < seedPair.First() + seedPair.Last(); seed++) {
                        long previousDestination = seed;
                        foreach (var map in sourceToDestinationMaps) {

                            previousDestination = map.GetDestination(previousDestination);
                        }

                        localSmallestDestinations.Add(previousDestination);
                    }

                    smallesDestinations.Add(localSmallestDestinations.Min());
                });

                tasks.Add(task);
                task.Start();
            }

            Task.WaitAll([.. tasks]);

            return smallesDestinations.Min();
        }

        private record Map(List<Mapping> Mappings)
        {
            public long GetDestination(long number)
            {
                foreach (var mapping in Mappings) {
                    if (mapping.Source.Contains(number)) {
                        var sourceToDestinationDifference = mapping.Source.Start - mapping.Destination.Start;

                        return number - sourceToDestinationDifference;
                    }
                }

                return number;
            }
        }

        private record Mapping(Range Source, Range Destination);

        private record Range(long Start, long Length)
        {
            public bool Contains(long number) => number >= Start && number < Start + Length;
        }
    }
}

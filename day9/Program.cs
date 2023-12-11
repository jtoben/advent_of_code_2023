namespace day9
{
    internal class Program
    {
        static void Main()
        {
            var readings = File.ReadAllLines("input.txt")
                .Select(line => line
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray())
                .ToArray();

            Console.WriteLine(PartOne(readings));
            Console.WriteLine(PartTwo(readings));
        }

        private static int PartOne(int[][] readings)
        {
            List<int> nextValues = [];
            foreach (var reading in readings) {
                var recursiveDifferences = GetRecursiveDifferencesInReverseOrder(reading);

                for (int i = 1; i < recursiveDifferences.Count; i++) {
                    recursiveDifferences[i].Add(recursiveDifferences[i].Last() + recursiveDifferences[i - 1].Last());
                }

                nextValues.Add(reading.Last() + recursiveDifferences.Last().Last());
            }

            return nextValues.Sum();
        }

        private static int PartTwo(int[][] readings)
        {
            List<int> previousValues = [];
            foreach (var reading in readings) {
                var recursiveDifferences = GetRecursiveDifferencesInReverseOrder(reading);

                for (int i = 1; i < recursiveDifferences.Count; i++) {
                    recursiveDifferences[i].Insert(0, recursiveDifferences[i].First() - recursiveDifferences[i - 1].First());
                }

                previousValues.Add(reading.First() - recursiveDifferences.Last().First());
            }

            return previousValues.Sum();
        }

        private static List<List<int>> GetRecursiveDifferencesInReverseOrder(int[] values)
        {
            List<List<int>> recursiveDifferences = [];
            var differences = GetDifferences(values);
            recursiveDifferences.Add([.. differences]);
            while (differences.Any(number => number != 0)) {
                differences = GetDifferences(differences);
                recursiveDifferences.Add([.. differences]);
            }

            recursiveDifferences.Reverse();
            return recursiveDifferences;

            static int[] GetDifferences(int[] values)
            {
                int[] result = new int[values.Length - 1];

                for (int i = 1; i < values.Length; i++) {
                    result[i - 1] = (values[i] - values[i - 1]);
                }

                return result;
            }
        }
    }
}

namespace day1
{
    internal class Program
    {
        static void Main()
        {
            var input = File.ReadAllLines("input.txt");

            Console.WriteLine(PartOne(input));
            Console.WriteLine(PartTwo(input));
        }

        private static int PartOne(string[] input)
        {
            return GetSumOfFirstAndLastNumberInStringArray(input);
        }

        private static int PartTwo(string[] input)
        {
            Dictionary<string, string> stringToNumbers = new() {
                { "one", "o1e" },
                { "two", "t2o" },
                { "three", "th3ee" },
                { "four", "fo4r" },
                { "five", "fi5e" },
                { "six", "s6x" },
                { "seven", "se7en" },
                { "eight", "ei8ht" },
                { "nine", "ni9e" },
            };
            var sanitizedInput = input.Select(
                line => stringToNumbers.Aggregate(line, (current, replacement) => current.Replace(replacement.Key, replacement.Value))
                ).ToArray();

            return GetSumOfFirstAndLastNumberInStringArray(sanitizedInput);
        }

        private static int GetSumOfFirstAndLastNumberInStringArray(string[] input)
        {
            return input.Select(
                    line => line.Where(char.IsDigit)
                                .ToArray()
                )
                .Select(x => x.First().ToString() + x.Last().ToString())
                            .Select(int.Parse)
                            .Sum();
        }
    }
}

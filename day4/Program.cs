namespace day4
{
    internal class Program
    {
        static void Main()
        {
            var cards = File.ReadAllLines("input.txt")
                .Select(line => line.Replace("  ", " "))
                .Select(Card.FromString)
                .ToArray();

            Console.WriteLine(PartOne(cards));
            Console.WriteLine(PartTwo(cards));
        }

        private static int PartOne(Card[] cards)
        {
            int totalScore = 0;
            foreach (var card in cards) {
                int numberOfScratchedNumbersInWinningNumbers = card.ScratchedNumbers.Where(card.WinningNumbers.Contains).Count();

                totalScore += (int)Math.Pow(2, numberOfScratchedNumbersInWinningNumbers - 1);
            }


            return totalScore;
        }

        private static int PartTwo(Card[] cards)
        {
            Dictionary<int, int> numberOfCardInstances = [];

            foreach (var card in cards) {
                numberOfCardInstances[card.Number] = 1;
            }

            foreach (var card in cards) {
                int numberOfScratchedNumbersInWinningNumbers = card.ScratchedNumbers.Where(card.WinningNumbers.Contains).Count();

                int cardInstances = numberOfCardInstances[card.Number];

                for (int i = card.Number; i < card.Number + numberOfScratchedNumbersInWinningNumbers; i++) {
                    numberOfCardInstances[i + 1] += cardInstances;
                }
            }

            return numberOfCardInstances.Select(pair => pair.Value).Sum();
        }

        private record Card(int Number, List<int> WinningNumbers, List<int> ScratchedNumbers)
        {
            public static Card FromString(string input)
            {
                var number = int.Parse(input.Split(": ")[0].Split(" ").Last());

                var winningNumbers = input.Split(": ")[1].Split(" | ")[0].TrimStart().Split(" ").Select(int.Parse).ToList();
                var scratchedNumbers = input.Split(": ")[1].Split(" | ")[1].TrimStart().Split(" ").Select(int.Parse).ToList();

                return new Card(number, winningNumbers, scratchedNumbers);
            }
        }
    }
}

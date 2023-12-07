namespace day7
{
    internal class Program
    {
        private static readonly Dictionary<char, int> _cardOrders = new() {
            { 'A', 1 },
            { 'K', 2 },
            { 'Q', 3 },
            { 'T', 5 },
            { '9', 6 },
            { '8', 7 },
            { '7', 8 },
            { '6', 9 },
            { '5', 10 },
            { '4', 11 },
            { '3', 12 },
            { '2', 13 },
        };

        static void Main()
        {
            var input = File.ReadAllLines("input.txt");

            var hands = input
                .Select(line => line.Split(" ", StringSplitOptions.RemoveEmptyEntries)[0])
                .ToArray();
            var bids = input
                .Select(line => line.Split(" ", StringSplitOptions.RemoveEmptyEntries)[1])
                .Select(int.Parse)
                .ToArray();

            Console.WriteLine(PartOne(hands, bids, input));
            Console.WriteLine(PartTwo(hands, bids, input));
        }

        private static int PartOne(string[] hands, int[] bids, string[] input)
        {
            _cardOrders.Add('J', 4);

            var orderedHands = hands
                .Select(Hand.FromString)
                .OrderDescending().ToList();

            return CalculateWinnings(orderedHands, bids, input);
        }

        private static long PartTwo(string[] hands, int[] bids, string[] input)
        {
            _cardOrders['J'] = 14;

            var orderedHands = hands
                .Select(Hand.FromStringPartTwo)
                .OrderDescending().ToList();

            return CalculateWinnings(orderedHands, bids, input);
        }

        private static int CalculateWinnings(List<Hand> orderedHands, int[] bids, string[] input)
        {
            int totalWinnings = 0;
            for (int multiplier = 0; multiplier < orderedHands.Count; multiplier++) {
                for (int index = 0; index < input.Length; index++) {
                    if (input[index].Contains(orderedHands[multiplier].Value)) {
                        totalWinnings += bids[index] * (multiplier + 1);
                        break;
                    }
                }
            }

            return totalWinnings;
        }

        private record Hand(string Value, HandType Type) : IComparer<Hand>, IComparable<Hand>
        {
            public static Hand FromString(string input)
            {
                return new Hand(input, DetermineHandType(input));
            }

            public static Hand FromStringPartTwo(string input)
            {
                int jCount = input.Count(c => c == 'J');

                if (jCount == 0) {
                    return FromString(input);
                }

                var type = DetermineHandType(input);
                type = type switch {
                    HandType.FiveOfAKind => HandType.FiveOfAKind,
                    HandType.FourOfAKind => HandType.FiveOfAKind,
                    HandType.FullHouse => HandType.FiveOfAKind,
                    HandType.ThreeOfAKind => HandType.FourOfAKind,
                    HandType.TwoPair => jCount == 2 ? HandType.FourOfAKind : HandType.FullHouse,
                    HandType.OnePair => HandType.ThreeOfAKind,
                    HandType.HighCard => HandType.OnePair,
                    _ => throw new NotImplementedException(),
                };

                return new Hand(input, type);
            }

            public int Compare(Hand? x, Hand? y)
            {
                if (x!.Type != y!.Type) {
                    return x.Type.CompareTo(y.Type);
                }

                for (int c = 0; c < x.Value.Length; c++) {
                    if (x.Value[c] == y.Value[c]) {
                        continue;
                    }

                    return _cardOrders[x.Value[c]].CompareTo(_cardOrders[y.Value[c]]);
                }

                return 0;
            }

            public int CompareTo(Hand? other)
            {
                return Compare(this, other);
            }

            private static HandType DetermineHandType(string input)
            {
                var groupedCards = input.GroupBy(card => card);
                var cardCounts = groupedCards
                    .Select(grouping => grouping.Count())
                    .ToArray();

                return cardCounts.Length switch {
                    1 => HandType.FiveOfAKind,
                    2 => groupedCards.First().Count() == 4 || groupedCards.Last().Count() == 4 ? HandType.FourOfAKind : HandType.FullHouse,
                    3 => cardCounts.Any(count => count == 3) ? HandType.ThreeOfAKind : HandType.TwoPair,
                    4 => HandType.OnePair,
                    _ => HandType.HighCard
                };
            }
        }

        private enum HandType
        {
            FiveOfAKind,
            FourOfAKind,
            FullHouse,
            ThreeOfAKind,
            TwoPair,
            OnePair,
            HighCard
        }
    }
}

namespace day2
{
    internal class Program
    {
        static void Main()
        {
            var games = File.ReadAllLines("input.txt")
                .Select(line => line.Split(": ")[1])
                .Select(Game.FromString)
                .ToArray();

            Console.WriteLine(PartOne(games));
            Console.WriteLine(PartTwo(games));
        }

        private static int PartOne(Game[] games)
        {
            Dictionary<string, int> _maximumNumberOfBallsPerColor = new() {
                { "red", 12 },
                { "green", 13 },
                { "blue", 14 }
            };

            int result = 0;
            for (int i = 0; i < games.Length; i++) {
                bool gamePossible = true;
                foreach (var set in games[i].Sets) {
                    foreach (var hand in set.Hands) {
                        gamePossible = gamePossible && hand.Value <= _maximumNumberOfBallsPerColor[hand.Color];
                    }
                }

                if (gamePossible) {
                    result += (i + 1);
                }
            }

            return result;
        }

        private static int PartTwo(Game[] games)
        {
            int result = 0;

            for (int i = 0; i < games.Length; i++) {

                int highestRed = 0;
                int highestGreen = 0;
                int highestBlue = 0;

                foreach (var set in games[i].Sets) {
                    foreach (var hand in set.Hands) {

                        switch (hand.Color) {
                            case "red":
                                highestRed = Math.Max(highestRed, hand.Value);
                                break;
                            case "green":
                                highestGreen = Math.Max(highestGreen, hand.Value);
                                break;
                            case "blue":
                                highestBlue = Math.Max(highestBlue, hand.Value);
                                break;
                        }
                    }
                }

                result += highestRed * highestGreen * highestBlue;
            }

            return result;
        }

        private record Game(Set[] Sets)
        {
            public static Game FromString(string input)
            {
                var sets = input.Split("; ").Select(Set.FromString).ToArray();

                return new Game(sets);
            }
        }

        private record Set(Hand[] Hands)
        {
            public static Set FromString(string input)
            {
                var hands = input.Split(", ").Select(Hand.FromString).ToArray();

                return new Set(hands);
            }
        }

        private record Hand(string Color, int Value)
        {
            public static Hand FromString(string input)
            {
                int value = int.Parse(input.Split(" ")[0]);
                string color = input.Split(" ")[1];

                return new Hand(color, value);
            }
        }
    }
}

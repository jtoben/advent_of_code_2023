namespace day3
{
    internal class Program
    {
        static void Main()
        {
            var engineSchematic = File.ReadAllLines("input.txt");

            List<Number> numbers = [];
            for (int y = 0; y < engineSchematic.Length; y++) {
                var line = engineSchematic[y];

                for (int x = 0; x < line.Length; x++) {
                    if (char.IsNumber(line[x])) {
                        int startX = x;

                        int offset = 1;
                        while (startX + offset < line.Length && char.IsNumber(line[startX + offset])) {
                            offset++;
                        }

                        int endX = startX + offset - 1;
                        numbers.Add(new Number(int.Parse(line.Substring(startX, offset)), startX, endX, y));

                        x = endX;
                    }
                }
            }

            Console.WriteLine(PartOne(numbers, engineSchematic));
            Console.WriteLine(PartTwo(numbers, engineSchematic));
        }

        private static int PartOne(List<Number> numbers, string[] engineSchematic)
        {
            int result = 0;

            foreach (var number in numbers) {
                if (IsPart(number, engineSchematic)) {
                    result += number.Value;
                }
            }

            return result;
        }

        private static int PartTwo(List<Number> numbers, string[] engineSchematic)
        {
            List<Vector2> gearLocations = GetGearLocations(engineSchematic);

            List<int> gearRatios = [];
            foreach (var gearLocation in gearLocations) {
                var neighbours = GetNeighbours(gearLocation, 0, engineSchematic.Length - 1, 0, engineSchematic[0].Length - 1);

                HashSet<Number> numberNeighbours = [];
                foreach (var neighbour in neighbours) {
                    var result = numbers.FirstOrDefault(number => number.ContainsLocation(neighbour));
                    if (result != null) {
                        numberNeighbours.Add(result);
                    }
                }

                if (numberNeighbours.Count == 2) {
                    gearRatios.Add(numberNeighbours.First().Value * numberNeighbours.Last().Value);
                }
            }

            return gearRatios.Sum();

            static List<Vector2> GetGearLocations(string[] engineSchematic)
            {
                List<Vector2> gearLocations = [];
                for (int y = 0; y < engineSchematic.Length; y++) {
                    var line = engineSchematic[y];

                    for (int x = 0; x < line.Length; x++) {
                        if (line[x] == '*') {
                            gearLocations.Add(new Vector2(x, y));
                        }
                    }
                }

                return gearLocations;
            }
        }

        private static bool IsPart(Number number, string[] engineSchematic)
        {
            var neighbours = GetNeighbours(number, 0, engineSchematic.Length - 1, 0, engineSchematic[0].Length - 1);

            foreach (var neighbour in neighbours) {
                var character = engineSchematic[neighbour.Y][neighbour.X];

                if (!char.IsNumber(character) && character != '.') {
                    return true;
                }
            }

            return false;
        }

        private static Vector2[] GetNeighbours(Number number, int minX, int maxX, int minY, int maxY)
        {
            List<Vector2> neighbours = [];

            for (int i = number.StartX - 1; i <= number.EndX + 1; i++) {
                neighbours.Add(new Vector2(i, number.Y - 1));
                neighbours.Add(new Vector2(i, number.Y + 1));
            }
            neighbours.Add(new Vector2(number.StartX - 1, number.Y));
            neighbours.Add(new Vector2(number.EndX + 1, number.Y));

            return neighbours.Where(
                neighbour => neighbour.X >= minX && neighbour.X <= maxX && neighbour.Y >= minY && neighbour.Y <= maxY)
                .ToArray();
        }

        private static Vector2[] GetNeighbours(Vector2 location, int minX, int maxX, int minY, int maxY)
        {
            List<Vector2> neighbours = [
                new Vector2(location.X - 1, location.Y - 1),
                new Vector2(location.X, location.Y - 1),
                new Vector2(location.X + 1, location.Y - 1),
                new Vector2(location.X + 1, location.Y),
                new Vector2(location.X + 1, location.Y + 1),
                new Vector2(location.X, location.Y + 1),
                new Vector2(location.X - 1, location.Y + 1),
                new Vector2(location.X - 1, location.Y),
            ];

            return neighbours.Where(
                neighbour => neighbour.X >= minX && neighbour.X <= maxX && neighbour.Y >= minY && neighbour.Y <= maxY)
                .ToArray();
        }

        private record Number(int Value, int StartX, int EndX, int Y)
        {
            public bool ContainsLocation(Vector2 location)
            {
                return location.Y == Y && location.X >= StartX && location.X <= EndX;
            }
        }

        private record Vector2(int X, int Y)
        {

        }
    }
}

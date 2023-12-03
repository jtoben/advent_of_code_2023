namespace day3
{
    internal class Program
    {
        static void Main()
        {
            var input = File.ReadAllLines("input.txt");

            List<Number> numbers = [];
            for (int y = 0; y < input.Length; y++) {
                var line = input[y];

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

            Console.WriteLine(PartOne(numbers, input));
            Console.WriteLine(PartTwo());
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

        private static int PartTwo()
        {
            return -1;
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

        private static Location[] GetNeighbours(Number number, int minX, int maxX, int minY, int maxY)
        {
            List<Location> neighbours = [];

            for (int i = number.StartX - 1; i <= number.EndX + 1; i++) {
                neighbours.Add(new Location(i, number.Y - 1));
                neighbours.Add(new Location(i, number.Y + 1));
            }
            neighbours.Add(new Location(number.StartX - 1, number.Y));
            neighbours.Add(new Location(number.EndX + 1, number.Y));

            return neighbours.Where(
                neighbour => neighbour.X >= minX && neighbour.X <= maxX && neighbour.Y >= minY && neighbour.Y <= maxY)
                .ToArray();
        }

        private record Number(int Value, int StartX, int EndX, int Y)
        {

        }

        private record Location(int X, int Y)
        {

        }
    }
}

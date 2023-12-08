namespace day8
{
    internal class Program
    {
        static void Main()
        {
            var input = File.ReadAllLines("input.txt");

            string instructions = input[0];
            var nodes = input
                .Skip(2)
                .Select(Node.FromString)
                .ToArray();

            Console.WriteLine(PartOne(instructions, nodes));
            Console.WriteLine(PartTwo(instructions, nodes));
        }

        private static int PartOne(string instructions, Node[] nodes)
        {
            var firstNode = nodes.First(node => node.Key == "AAA");
            return CalculateNumberOfStepsFromStartingNodeToFinish(firstNode, instructions, nodes, (key) => key == "ZZZ");
        }

        private static long PartTwo(string instructions, Node[] nodes)
        {
            var currentNodes = nodes
                .Where(node => node.Key.EndsWith('A'))
                .ToArray();

            // Find the number of steps per node.
            List<long> stepsForEachNode = [];
            foreach (var node in currentNodes) {
                stepsForEachNode.Add(CalculateNumberOfStepsFromStartingNodeToFinish(node, instructions, nodes, (key) => key.EndsWith('Z')));
            }

            return CalculateLeastCommonMultiple(stepsForEachNode);

            // Use Least Common Multiple algortihm to find when all cycles sync up:
            // https://www.w3resource.com/csharp-exercises/math/csharp-math-exercise-20.php
            long CalculateLeastCommonMultiple(List<long> values)
            {
                return values.Aggregate((a, b) => a * b / CalculateGreatestCommonDivisor(a, b));
            }

            long CalculateGreatestCommonDivisor(long a, long b) => b == 0 ? a : CalculateGreatestCommonDivisor(b, a % b);
        }

        private static int CalculateNumberOfStepsFromStartingNodeToFinish(Node startingNode, string instructions, Node[] nodes, Func<string, bool> isFinishNodeFunc)
        {
            int numberOfSteps = 0;
            bool endNodeFound = false;
            Node currentNode = startingNode;
            while (!endNodeFound) {
                for (int c = 0; c < instructions.Length; c++) {
                    numberOfSteps++;
                    currentNode = instructions[c] switch {
                        'R' => nodes.First(node => node.Key == currentNode.Right),
                        'L' => nodes.First(node => node.Key == currentNode.Left),
                        _ => throw new Exception("Should never happen!")
                    };

                    if (isFinishNodeFunc.Invoke(currentNode.Key)) {
                        endNodeFound = true;
                        break;
                    }
                }
            }

            return numberOfSteps;
        }

        private record Node(string Key, string Left, string Right)
        {
            public static Node FromString(string input)
            {
                var parts = input.Split(" = (");
                string key = parts[0];

                string leftKey = parts[1].Split(", ")[0];
                string rightKey = new(parts[1].Split(", ")[1].SkipLast(1).ToArray());

                return new Node(key, leftKey, rightKey);
            }
        }
    }
}

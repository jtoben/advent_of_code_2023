namespace day6
{
    internal class Program
    {
        static void Main()
        {
            var input = File.ReadAllLines("input.txt")
                .Select(line => line.Split(":")[1])
                .ToArray();

            List<int> times = input[0].Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();
            List<int> distances = input[1].Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();

            Console.WriteLine(PartOne(times, distances));
            Console.WriteLine(PartTwo(times, distances));
        }

        private static int PartOne(List<int> times, List<int> distances)
        {
            int numberOfWays = 1;
            for (int i = 0; i < times.Count; i++) {
                int time = times[i];
                int distance = distances[i];

                int possibilities = 0;
                for (int speed = 1; speed < time; speed++) {
                    if (WinsRace(speed, time, distance)) {
                        possibilities++;
                    }
                }
                numberOfWays *= possibilities;
            }

            return numberOfWays;
        }

        private static long PartTwo(List<int> times, List<int> distances)
        {
            long correctTime = long.Parse(string.Concat(times));
            long correctDistance = long.Parse(string.Concat(distances));

            // Using binary search, find the first value that has enough distance travelled.
            // The last value is simply the correctTime - first value.
            // Then, we have found the range!
            long rangeMin = 1;
            long rangeMax = correctTime - 1;
            while (true) {

                long difference = rangeMax - rangeMin;
                long speed = rangeMin + difference / 2;

                if (WinsRace(speed, correctTime, correctDistance)) {
                    rangeMax = speed;
                } else {
                    rangeMin = speed + 1;
                }

                if (rangeMin == rangeMax) {
                    break;
                }
            }

            return correctTime - (rangeMin * 2) + 1;
        }

        private static bool WinsRace(long speed, long time, long distance)
        {
            long timeLeft = time - speed;
            long distanceTravelled = speed * timeLeft;

            return distanceTravelled > distance;
        }
    }
}

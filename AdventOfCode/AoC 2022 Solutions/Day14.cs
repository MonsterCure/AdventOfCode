using System.Diagnostics;
using System.Numerics;

namespace AoC_2022_Solutions
{
    public class Day14 //--- Day 14: Regolith Reservoir ---
    {
        public static void Part01and02()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var input = File.ReadAllLines(@"..\..\..\..\AoC 2022 Inputs\Day14.txt").ToList();
            Dictionary<Complex, char> caveMap = new Dictionary<Complex, char>();

            foreach (var line in input)
            {
                var rockPaths = (
                    from rockLines in line.Split(new string[] { " -> " }, StringSplitOptions.None)
                    let coordinates = rockLines.Split(new string[] { "," }, StringSplitOptions.None)
                    select new Complex(int.Parse(coordinates[0]), int.Parse(coordinates[1]))
                ).ToList();

                for (var i = 1; i < rockPaths.Count; i++)
                {
                    var path = new Complex(
                        Math.Sign(rockPaths[i].Real - rockPaths[i - 1].Real),
                        Math.Sign(rockPaths[i].Imaginary - rockPaths[i - 1].Imaginary)
                    );

                    for (var currPos = rockPaths[i - 1]; currPos != rockPaths[i] + path; currPos += path)
                        caveMap[currPos] = '#';
                }
            }

            int limit = (int)caveMap.Keys.Select(pos => pos.Imaginary).Max();

            var result1 = SimulateFallingSand(caveMap, new Complex(500, 0), limit, false);
            var result2 = SimulateFallingSand(caveMap, new Complex(500, 0), limit, true);

            Console.WriteLine($"Units of sand with abyss: {result1}\nUnits of sand with floor: {result2}.\n");
            sw.Stop();
            Console.WriteLine($"Time elapsed: {sw.Elapsed.Milliseconds}ms.\n\n");
            Console.ReadKey();
        }

        private static int SimulateFallingSand(Dictionary<Complex, char> caveMap, Complex sandPoint, int limit, bool hasFloor)
        {
            while (true)
            {
                var down = new Complex(0, 1);
                var downLeft = new Complex(-1, 1);
                var downRight = new Complex(1, 1);

                var sand = sandPoint;

                while (sand.Imaginary < limit + 1)
                {
                    if (!caveMap.ContainsKey(sand + down))
                        sand += down;
                    else if (!caveMap.ContainsKey(sand + downLeft))
                        sand += downLeft;
                    else if (!caveMap.ContainsKey(sand + downRight))
                        sand += downRight;
                    else
                        break;
                }

                if (caveMap.ContainsKey(sand) || (!hasFloor && sand.Imaginary == limit + 1))
                    break;

                caveMap[sand] = 'o';
            }

            return caveMap.Values.Count(x => x == 'o');
        }
    }
}

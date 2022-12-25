using System.Diagnostics;

namespace AoC_2022_Solutions
{
    public class Day24 //--- Day 24: Blizzard Basin ---
    {
        public static void Part01and02()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var input = File.ReadAllLines(@"..\..\..\..\AoC 2022 Inputs\Day24.txt");

            var height = input.Length - 2;
            var width = input[0].Length - 2;

            var upGrid = input.Skip(1).Take(input.Length - 2).Select(line => line.Skip(1).Take(line.Length - 2).Select(c => c == '^').ToList()).ToList();
            var downGrid = input.Skip(1).Take(input.Length - 2).Select(line => line.Skip(1).Take(line.Length - 2).Select(c => c == 'v').ToList()).ToList();
            var leftGrid = input.Skip(1).Take(input.Length - 2).Select(line => line.Skip(1).Take(line.Length - 2).Select(c => c == '<').ToList()).ToList();
            var rightGrid = input.Skip(1).Take(input.Length - 2).Select(line => line.Skip(1).Take(line.Length - 2).Select(c => c == '>').ToList()).ToList();

            int PosMod(int a, int b) => ((a % b) + b) % b;
            bool IsLeft(int x, int y, int t) => leftGrid[y][PosMod(x + t, width)];
            bool IsRight(int x, int y, int t) => rightGrid[y][PosMod(x - t, width)];
            bool IsUp(int x, int y, int t) => upGrid[PosMod(y + t, height)][x];
            bool IsDown(int x, int y, int t) => downGrid[PosMod(y - t, height)][x];
            bool IsOccupied(int x, int y, int t) => x < 0 || y < 0 || x >= width || y >= height || IsLeft(x, y, t) || IsRight(x, y, t) || IsUp(x, y, t) || IsDown(x, y, t);
            int Navigate(Coord from, Coord to, int t) => DepthFirstSearch(from, t, from, to, new HashSet<(Coord Pos, int T)>(), t + 500);

            int DepthFirstSearch(Coord pos, int t, Coord start, Coord end, ISet<(Coord Pos, int T)> tried, int timeLimit)
            {
                var bestTime = timeLimit;

                if (tried.Contains((pos, t)) || t >= bestTime) return bestTime;

                tried.Add((pos, t));

                foreach (var move in new[] { new Coord(0, 1), new Coord(1, 0), new Coord(0, 0), new Coord(0, -1), new Coord(-1, 0) })
                {
                    var newPos = pos + move;

                    if (newPos == end) return t;

                    if (newPos == start || !IsOccupied(newPos.X, newPos.Y, t))
                    {
                        var endTime = DepthFirstSearch(newPos, t + 1, start, end, tried, bestTime);

                        if (endTime < bestTime) bestTime = endTime;
                    }
                }

                return bestTime;
            }

            var topLeft = new Coord(0, -1);
            var bottomRight = new Coord(width - 1, height);
            var part1 = Navigate(topLeft, bottomRight, 1);
            Console.WriteLine($"Fewest number of minutes to goal: {part1}\nTime elapsed: {sw.Elapsed.Milliseconds}ms.\n\n");

            var retrace = Navigate(bottomRight, topLeft, part1 + 1);
            var result2 = Navigate(topLeft, bottomRight, retrace + 1);

            sw.Stop();
            Console.WriteLine($"Fewest number of minutes to goal, back, and to goal again: {result2}\nTime elapsed: {sw.Elapsed.Milliseconds}ms.\n\n");
            Console.ReadKey();
        }
    }
}

using System.Diagnostics;

namespace AoC_2022_Solutions
{
    public class Day09
    {
        public static void Part01and02(int ropeOneLength, int ropeTwoLength)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var input = File.ReadAllLines(@"..\..\..\..\AoC 2022 Inputs\Day09.txt");

            int longerRope = Math.Max(ropeOneLength, ropeTwoLength);
            (int x, int y)[] knotPositions = new (int x, int y)[longerRope];
            HashSet<(int, int)> ropeOneTailVisitedPositions = new HashSet<(int, int)>();
            HashSet<(int, int)> ropeTwoTailVisitedPositions = new HashSet<(int, int)>();

            foreach (var line in input)
            {
                string[] parsedDirections = line.Trim().Split(' ');
                string direction = parsedDirections[0];
                int steps = int.Parse(parsedDirections[1]);

                for (int i = 0; i < steps; i++)
                {
                    switch (direction)
                    {
                        case "R":
                            knotPositions[0].x += 1;
                            break;
                        case "L":
                            knotPositions[0].x -= 1;
                            break;
                        case "U":
                            knotPositions[0].y -= 1;
                            break;
                        case "D":
                            knotPositions[0].y += 1;
                            break;
                        default:
                            throw new Exception();
                    }

                    for (int j = 1; j < longerRope; j++)
                    {
                        int dx = knotPositions[j - 1].x - knotPositions[j].x;
                        int dy = knotPositions[j - 1].y - knotPositions[j].y;

                        if (Math.Abs(dx) > 1 || Math.Abs(dy) > 1)
                        {
                            knotPositions[j].x += Math.Sign(dx);
                            knotPositions[j].y += Math.Sign(dy);
                        }
                    }

                    ropeOneTailVisitedPositions.Add(knotPositions[ropeOneLength - 1]); //save position of 2nd knot if unique
                    ropeTwoTailVisitedPositions.Add(knotPositions[ropeTwoLength - 1]); //save position of 9th knot if unique
                }
            }

            Console.WriteLine($"Positions visited with a 2-knots rope: {ropeOneTailVisitedPositions.Count}\nPositions visited with a 10-knots rope: {ropeTwoTailVisitedPositions.Count}.\n");
            sw.Stop();
            Console.WriteLine($"Time elapsed: {sw.Elapsed.Milliseconds}ms.\n\n");
            Console.ReadKey();
        }
    }
}

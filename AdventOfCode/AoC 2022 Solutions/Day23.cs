using System.Diagnostics;

namespace AoC_2022_Solutions
{
    public class Day23 //--- Day 23: Unstable Diffusion ---
    {
        public static void Part01and02()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var input = File.ReadAllLines(@"..\..\..\..\AoC 2022 Inputs\Day23.txt").ToList();

            var map = new Dictionary<(int X, int Y), bool>();

            for (int y = 0; y < input.Count; y++)
                for (int x = 0; x < input[y].Length; x++)
                    if (input[y][x] == '#') map[(x, y)] = true;


            bool simulateRound(int round)
            {
                var occupy = new Dictionary<(int X, int Y), int>();
                var propose = new List<((int X, int Y) Old, (int X, int Y) New)>();

                foreach (var (X, Y) in map.Keys)
                {
                    bool consider = false;

                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            if (x == 0 && y == 0) continue;
                            if (map.Read((X + x, Y + y))) consider = true;
                        }
                    }

                    if (consider)
                    {
                        bool proposed = false;

                        for (int i = 0; i < 4; i++)
                        {
                            if (((round + i) % 4) == 0 && !(map.Read((X, Y - 1)) || map.Read((X - 1, Y - 1)) || map.Read((X + 1, Y - 1))))
                            {
                                propose.Add(((X, Y), (X, Y - 1)));
                                proposed = true;
                            }

                            if (((round + i) % 4) == 1 && !(map.Read((X, Y + 1)) || map.Read((X - 1, Y + 1)) || map.Read((X + 1, Y + 1))))
                            {
                                propose.Add(((X, Y), (X, Y + 1)));
                                proposed = true;
                            }

                            if (((round + i) % 4) == 2 && !(map.Read((X - 1, Y)) || map.Read((X - 1, Y - 1)) || map.Read((X - 1, Y + 1))))
                            {
                                propose.Add(((X, Y), (X - 1, Y)));
                                proposed = true;
                            }

                            if (((round + i) % 4) == 3 && !(map.Read((X + 1, Y)) || map.Read((X + 1, Y - 1)) || map.Read((X + 1, Y + 1))))
                            {
                                propose.Add(((X, Y), (X + 1, Y)));
                                proposed = true;
                            }

                            if (proposed) break;
                        }
                    }
                }

                foreach (var (Old, New) in propose)
                    occupy[(New.X, New.Y)] = occupy.Read((New.X, New.Y)) + 1;

                foreach (var (Old, New) in propose)
                {
                    if (occupy.Read((New.X, New.Y)) == 1)
                    {
                        map.Remove(Old);
                        map[New] = true;
                    }
                }

                if (propose.Count == 0) return false;

                propose.Clear();
                occupy.Clear();

                return true;
            }

            int roundCounter = 0;

            for (; roundCounter < 10; roundCounter++)
                simulateRound(roundCounter);

            int minX, minY, maxX, maxY;
            minX = map.Min(c => c.Key.X);
            maxX = map.Max(c => c.Key.X);
            minY = map.Min(c => c.Key.Y);
            maxY = map.Max(c => c.Key.Y);

            var result1 = (maxX - minX + 1) * (maxY - minY + 1) - map.Count;
            Console.WriteLine($"Empty ground tiles in rectangle: {result1}\n");

            while (simulateRound(roundCounter))
                roundCounter++;

            sw.Stop();
            Console.WriteLine($"Round where no Elf moves: {roundCounter + 1}\nTime elapsed: {sw.Elapsed.Milliseconds}ms.\n\n");
            Console.ReadKey();
        }
    }
}

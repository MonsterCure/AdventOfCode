using System.Diagnostics;

namespace AoC_2022_Solutions
{
    public class Day17 //--- Day 17: Pyroclastic Flow ---
    {
        public static void Part01and02()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var input = File.ReadAllText(@"..\..\..\..\AoC 2022 Inputs\Day17.txt");

            var map = new Dictionary<(int X, int Y), int>();

            string[][] blocks = {
                new[] {
                    "####"
                },
                new[] {
                    " # ",
                    "###",
                    " # "
                },
                new[] {
                    "###",
                    "  #",
                    "  #"
                },
                new[] {
                    "#",
                    "#",
                    "#",
                    "#"
                },
                new[] {
                    "##",
                    "##"
                }
            };

            string tape = input;
            int index = 0;
            long repeats = 0;

            int maxY = 0;
            int dispMax = 0;

            bool isInPlay = false;

            var (blockX, blockY) = (0, 0);

            long nth = 0;
            int block = 0;
            string[] current = blocks[block];

            var revisit = new Dictionary<(int Tape, int Shape), (long Rocks, long Height)>();

            bool CheckCollision(int bx, int by)
            {
                bool ok = true;

                for (int y = by; y < by + current.Length; y++)
                    for (int x = bx; x < bx + current[y - by].Length; x++)
                        if (by <= y && y <= by + current.Length - 1 && bx <= x && x <= bx + current[y - by].Length - 1)
                            if (current[y - by][x - bx] == '#' && map.Read((x, y), -1) != -1)
                                ok = false;

                return ok;
            }

            long result1 = 0;
            long result2 = 0;

            while (result1 == 0 || result2 == 0)
            {
                if (!isInPlay)
                {
                    blockX = 2;
                    blockY = (map.Count > 1 ? maxY : -1) + 4;
                    dispMax = blockY + blocks[block].Length;
                    current = blocks[block];
                    isInPlay = true;
                    nth++;

                    if (nth == 2023) result1 = maxY + 1;
                }

                int next = blockX;

                if (tape[index] == '<') next--;
                else next++;

                if (0 <= next && next + current[0].Length <= 7 && CheckCollision(next, blockY)) blockX = next;

                index = (index + 1) % tape.Length;

                if (index == 0) repeats++;

                next = blockY - 1;

                if (0 <= next && CheckCollision(blockX, next)) blockY = next;
                else
                {
                    for (int y = 0; y < blocks[block].Length; y++)
                    {
                        for (int x = 0; x < blocks[block][y].Length; x++)
                        {
                            bool place = blocks[block][y][x] == '#';

                            if (place) map[(blockX + x, blockY + y)] = block;

                            maxY = Math.Max(maxY, blockY + y);
                        }
                    }

                    block = (block + 1) % blocks.Length;
                    isInPlay = false;

                    if (revisit.ContainsKey((index, block)) && result2 == 0)
                    {
                        var last = revisit[(index, block)];
                        long cycle = nth - last.Rocks;
                        long adds = maxY + 1 - last.Height;
                        long remaining = 1000000000000 - nth - 1;
                        long combo = (remaining / (cycle) + 1);

                        if (nth + combo * cycle == 1000000000000) result2 = maxY + 1 + combo * adds;
                    }
                    else revisit[(index, block)] = (nth, maxY + 1);
                }
            }

            sw.Stop();
            Console.WriteLine($"After 2022 rocks, the tower is {result1} units tall.\nAfter a trillion rocks, the tower is {result2} units tall.\nTime elapsed: {sw.Elapsed.Milliseconds}ms.\n\n");
            Console.ReadKey();
        }
    }
}

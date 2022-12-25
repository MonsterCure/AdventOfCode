using System.Diagnostics;

namespace AoC_2022_Solutions
{
    public class Day01 //--- Day 1: Calorie Counting ---
    {
        public static void Part01and02()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            string[] inputLines = File.ReadAllLines(@"..\..\..\..\AoC 2022 Inputs\Day01.txt");

            List<int> elfSnacks = new List<int>();
            int snack = 0;

            foreach (string line in inputLines)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    snack += int.Parse(line);
                }
                else
                {
                    elfSnacks.Add(snack);
                    snack = 0;
                }
            }

            int topElf;
            int topElfSnack;
            int topElvesSnacksTotal = 0;

            for (int i = 0; i < 3; i++)
            {
                topElf = elfSnacks.IndexOf(elfSnacks.Max()) + 1;
                topElfSnack = elfSnacks.Max();
                topElvesSnacksTotal += topElfSnack;

                Console.WriteLine($"{i + 1}. Elf {topElf} is carrying {topElfSnack} calories.\n");

                elfSnacks.Remove(elfSnacks.Max());
            }

            sw.Stop();
            Console.WriteLine($"The top three elves are carrying {topElvesSnacksTotal} calories.\nTime elapsed: {sw.Elapsed.Milliseconds}ms.\n\n");
            Console.ReadKey();
        }
    }
}
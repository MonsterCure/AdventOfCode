using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace AoC_2022_Solutions
{
    public class Day05
    {
        public static void Part01()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var input = File.ReadAllLines(@"..\..\..\..\AoC 2022 Inputs\Day05.txt");

            int stacksLine = Array.FindIndex(input, line => line.StartsWith(" 1"));
            int stacksNumber = input[stacksLine].Trim().Last() - '0';

            var cratesStartingStack = input.Take(stacksLine).ToArray().Reverse();
            var instructions = Array.FindAll(input, line => line.StartsWith("move"));

            List<Stack<string>> cratesToRearrange = new List<Stack<string>>();

            for (int i = 0; i < stacksNumber; i++)
                cratesToRearrange.Add(new Stack<string>());

            foreach (var line in cratesStartingStack)
            {
                int lineCounter = 0;

                for (int j = 1; j <= line.Length; j += 4)
                {
                    var crate = line.ElementAt(j).ToString();

                    if (!string.IsNullOrWhiteSpace(crate))
                        cratesToRearrange.ElementAt(lineCounter).Push(crate);

                    lineCounter++;
                }
            }

            foreach (var line in instructions)
            {
                var moves = line.Trim().Split(' ');
                int cratesToMove = int.Parse(moves.ElementAt(1));
                int previousStack = int.Parse(moves.ElementAt(3)) - 1;
                int nextStack = int.Parse(moves.ElementAt(5)) - 1;

                while (cratesToMove > 0)
                {
                    var crate = cratesToRearrange.ElementAt(previousStack).Pop();
                    cratesToRearrange.ElementAt(nextStack).Push(crate);
                    cratesToMove--;
                }
            }

            string topCrates = "";
            foreach (var stack in cratesToRearrange)
                topCrates += $"{stack.Peek()}";

            Console.WriteLine($"Top crates: {topCrates}\n");
            sw.Stop();
            Console.WriteLine($"Time elapsed: {sw.Elapsed.Milliseconds}ms.\n\n");
            Console.ReadKey();
        }

        public static void Part02()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var input = File.ReadAllLines(@"..\..\..\..\AoC 2022 Inputs\Day05.txt");

            int stacksLine = Array.FindIndex(input, line => line.StartsWith(" 1"));
            int stacksNumber = input[stacksLine].Trim().Last() - '0';

            var cratesStartingStack = input.Take(stacksLine).ToArray().Reverse();
            var instructions = Array.FindAll(input, line => line.StartsWith("move"));

            List<Stack<string>> cratesToRearrange = new List<Stack<string>>();

            for (int i = 0; i < stacksNumber; i++)
                cratesToRearrange.Add(new Stack<string>());

            foreach (var line in cratesStartingStack)
            {
                int lineCounter = 0;

                for (int j = 1; j <= line.Length; j += 4)
                {
                    var crate = line.ElementAt(j).ToString();

                    if (!string.IsNullOrWhiteSpace(crate))
                        cratesToRearrange.ElementAt(lineCounter).Push(crate);

                    lineCounter++;
                }
            }

            foreach (var line in instructions)
            {
                var moves = line.Trim().Split(' ');
                int cratesToMove = int.Parse(moves.ElementAt(1));
                int previousStack = int.Parse(moves.ElementAt(3)) - 1;
                int nextStack = int.Parse(moves.ElementAt(5)) - 1;
                var miniStack = new Stack<string>();

                while (cratesToMove > 0)
                {
                    var crate = cratesToRearrange.ElementAt(previousStack).Pop();
                    miniStack.Push(crate);
                    cratesToMove--;
                }

                while (miniStack.Count() > 0)
                {
                    var crate = miniStack.Pop();
                    cratesToRearrange.ElementAt(nextStack).Push(crate);
                }
            }

            string topCrates = "";
            foreach (var stack in cratesToRearrange)
                topCrates += $"{stack.Peek()}";

            Console.WriteLine($"Top crates: {topCrates}\n");
            sw.Stop();
            Console.WriteLine($"Time elapsed: {sw.Elapsed.Milliseconds}ms.\n\n");
            Console.ReadKey();
        }
    }
}

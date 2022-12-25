using System.Diagnostics;

namespace AoC_2022_Solutions
{
    public class Day10 //--- Day 10: Cathode-Ray Tube ---
    {
        public static void Part01()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var input = File.ReadAllLines(@"..\..\..\..\AoC 2022 Inputs\Day10.txt");

            int cycle = 0;
            int register = 1;
            int signalStrength = 0;

            foreach (var line in input)
            {
                string[] parsedLine = line.Trim().Split(' ');
                string instruction = parsedLine[0];
                int value = 0;

                if (instruction == "noop")
                {
                    cycle++;

                    if ((cycle - 20) % 40 == 0)
                        signalStrength += cycle * register;
                }

                if (instruction == "addx")
                {
                    value = int.Parse(parsedLine[1]);

                    for (int i = 0; i < 2; i++)
                    {
                        cycle++;

                        if ((cycle - 20) % 40 == 0)
                            signalStrength += cycle * register;

                        if (i == 1)
                            register += value;
                    }
                }
            }

            sw.Stop();
            Console.WriteLine($"Sum of the signal strengths: {signalStrength}.\nTime elapsed: {sw.Elapsed.Milliseconds}ms.\n\n");
            Console.ReadKey();
        }

        public static void Part01and02()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var input = File.ReadAllLines(@"..\..\..\..\AoC 2022 Inputs\Day10.txt");

            int cycle = 0;
            int register = 1;
            int signalStrength = 0;
            string draw = "";

            foreach (var line in input)
            {
                string[] parsedLine = line.Trim().Split(' ');
                string instruction = parsedLine[0];
                int value = 0;

                if (instruction == "noop")
                {
                    draw += (Math.Abs(cycle % 40 - register) < 2) ? "#" : ".";
                    cycle++;
                    signalStrength += ((cycle - 20) % 40 == 0) ? cycle * register : 0;
                    draw += (cycle % 40 == 0) ? "\n" : "";
                }
                else if (instruction == "addx")
                {
                    value = int.Parse(parsedLine[1]);

                    for (int i = 0; i < 2; i++)
                    {
                        draw += (Math.Abs(cycle % 40 - register) < 2) ? "#" : ".";
                        cycle++;
                        signalStrength += ((cycle - 20) % 40 == 0) ? cycle * register : 0;
                        draw += (cycle % 40 == 0) ? "\n" : "";
                        register += (i == 1) ? value : 0;
                    }
                }
            }

            sw.Stop();
            Console.WriteLine($"Sum of the signal strengths: {signalStrength}.\n{draw}\n\nTime elapsed: {sw.Elapsed.Milliseconds}ms.\n\n");
            Console.ReadKey();
        }
    }
}

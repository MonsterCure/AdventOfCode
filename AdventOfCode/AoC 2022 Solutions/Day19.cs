using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AoC_2022_Solutions
{
    public class Day19 //--- Day 19: Not Enough Minerals ---
    {
        public static void Part01and02()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var input = File.ReadAllLines(@"..\..\..\..\AoC 2022 Inputs\Day19.txt").ToList();
            var Blueprints = new List<Blueprint>();

            foreach (var line in input)
            {
                var blueprintData = Regex.Matches(line, "-?\\d+").Select(e => int.Parse(e.Value)).ToArray();
                Blueprints.Add(new Blueprint(blueprintData));
            }

            var qualityLevel = 0;
            foreach (var blueprint in Blueprints)
            {
                var geodeCount = GetMaxOpenedGeodes(blueprint, 24);
                qualityLevel += blueprint.ID * geodeCount;
            }

            var maxGeodes = 1;
            foreach (var blueprint in Blueprints.Take(3))
            {
                var geodeCount = GetMaxOpenedGeodes(blueprint, 32);
                maxGeodes *= geodeCount;
            }

            Console.WriteLine($"Quality level of all blueprints: {qualityLevel}\nMax geodes with first three blueprints: {maxGeodes}\n");
            sw.Stop();
            Console.WriteLine($"Time elapsed: {sw.Elapsed.Milliseconds}ms.\n\n");
            Console.ReadKey();
        }

        private static int GetMaxOpenedGeodes(Blueprint blueprint, int time)
        {
            var seenStates = new HashSet<(int, int, int, int, int, int, int, int, int)>();
            var startState = (0, 0, 0, 0, 1, 0, 0, 0, time);
            var queue = new Queue<(int, int, int, int, int, int, int, int, int)>();

            int geodeCount = 0;

            queue.Enqueue(startState);

            while (queue.TryDequeue(out var state))
            {
                var (ore, clay, obsidian, geodes, oreRobots, clayRobots, obsidianRobots, geodeRobots, timeRemaining) = state;

                geodeCount = int.Max(geodeCount, geodes);

                if (timeRemaining == 0) continue;

                oreRobots = int.Min(oreRobots, blueprint.MaxOre);
                clayRobots = int.Min(clayRobots, blueprint.ObsidianRobot.clay);
                obsidianRobots = int.Min(obsidianRobots, blueprint.GeodeCracker.obsidian);

                ore = int.Min(ore, (timeRemaining * blueprint.MaxOre) - (oreRobots * (timeRemaining - 1)));
                clay = int.Min(clay, (timeRemaining * blueprint.ObsidianRobot.clay) - (clayRobots * (timeRemaining - 1)));
                obsidian = int.Min(obsidian, (timeRemaining * blueprint.GeodeCracker.obsidian) - (obsidianRobots * (timeRemaining - 1)));

                var curState = (ore, clay, obsidian, geodes, oreRobots, clayRobots, obsidianRobots, geodeRobots, timeRemaining);

                if (seenStates.Contains(curState)) continue;

                seenStates.Add(curState);

                queue.Enqueue(
                    (ore + oreRobots, 
                    clay + clayRobots, 
                    obsidian + obsidianRobots, 
                    geodes + geodeRobots, 
                    oreRobots, 
                    clayRobots, 
                    obsidianRobots, 
                    geodeRobots, 
                    timeRemaining - 1));

                if (ore >= blueprint.OreRobot) 
                    queue.Enqueue(
                        (ore + oreRobots - blueprint.OreRobot,
                        clay + clayRobots,
                        obsidian + obsidianRobots,
                        geodes + geodeRobots, 
                        oreRobots + 1, 
                        clayRobots, 
                        obsidianRobots, 
                        geodeRobots, 
                        timeRemaining - 1));

                if (ore >= blueprint.ClayRobot)
                    queue.Enqueue(
                        (ore + oreRobots - blueprint.ClayRobot, 
                        clay + clayRobots,
                        obsidian + obsidianRobots, 
                        geodes + geodeRobots, 
                        oreRobots, 
                        clayRobots + 1,
                        obsidianRobots, 
                        geodeRobots, 
                        timeRemaining - 1));

                if (ore >= blueprint.ObsidianRobot.ore 
                    && clay >= blueprint.ObsidianRobot.clay) 
                    queue.Enqueue(
                        (ore + oreRobots - blueprint.ObsidianRobot.ore,
                        clay + clayRobots - blueprint.ObsidianRobot.clay,
                        obsidian + obsidianRobots, 
                        geodes + geodeRobots, 
                        oreRobots, 
                        clayRobots, 
                        obsidianRobots + 1, 
                        geodeRobots, 
                        timeRemaining - 1));

                if (ore >= blueprint.GeodeCracker.ore
                    && obsidian >= blueprint.GeodeCracker.obsidian) 
                    queue.Enqueue(
                        (ore + oreRobots - blueprint.GeodeCracker.ore,
                        clay + clayRobots, 
                        obsidian + obsidianRobots - blueprint.GeodeCracker.obsidian,
                        geodes + geodeRobots, 
                        oreRobots, 
                        clayRobots, 
                        obsidianRobots, 
                        geodeRobots + 1, 
                        timeRemaining - 1));
            }

            return geodeCount;
        }

        internal class Blueprint
        {
            public int ID { get; }
            public int OreRobot { get; }
            public int ClayRobot { get; }
            public (int ore, int clay) ObsidianRobot { get; }
            public (int ore, int obsidian) GeodeCracker { get; }

            public int MaxOre { get; set; }

            public Blueprint() { }
            public Blueprint(int[] values)
            {
                ID = values[0];
                OreRobot = values[1];
                ClayRobot = values[2];
                ObsidianRobot = (values[3], values[4]);
                GeodeCracker = (values[5], values[6]);
                MaxOre = MaxOfMany(OreRobot, ClayRobot, ObsidianRobot.ore, GeodeCracker.ore);
            }
        }

        public static int MaxOfMany(params int[] items)
        {
            int result = items[0];

            for (int i = 1; i < items.Length; i++)
                result = Math.Max(result, items[i]);

            return result;
        }
    }
}

using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AoC_2022_Solutions
{
    public class Day16 //--- Day 16: Proboscidea Volcanium ---
    {
        public static void Part01and02()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var input = File.ReadAllLines(@"..\..\..\..\AoC 2022 Inputs\Day16.txt").ToList();
            Regex regex = new Regex(@"^Valve (\w+) has flow rate=(\d+); tunnels? leads? to valves? (.*)$", RegexOptions.Compiled);

            List<(string label, int flowRate, string[] connectingValves)> valves = new List<(string, int, string[])>();
            var graph = new List<int[]>();
            var cGraph = new List<int[]>();
            var flowRates = new List<int>();
            Dictionary<string, int> labelIndex = new Dictionary<string, int>();
            int start, valvesNum = 0;

            for (int i = 0; i < input.Count; i++)
            {
                Match match = regex.Match(input[i]);
                valves.Add((match.Groups[1].Value, int.Parse(match.Groups[2].Value), match.Groups[3].Value.Split(new string[] { ", " }, StringSplitOptions.None)));
            }

            valves = valves.OrderByDescending(valve => valve.flowRate).ToList();

            foreach (var (label, flowRate, connectingValves) in valves)
            {
                labelIndex[label] = labelIndex.Count;
                flowRates.Add(flowRate);
            }

            foreach (var (label, flowRate, connectingValves) in valves) graph.Add(connectingValves.Select(v => labelIndex[v]).ToArray());

            start = labelIndex["AA"];

            for (int origin = 0; origin < graph.Count; origin++)
            {
                int[] distances = new int[graph.Count];
                cGraph.Add(distances);
                List<int> stack = new List<int> { origin };

                if (flowRates[origin] == 0 && origin != start) continue;
                if (flowRates[origin] > 0) valvesNum++;

                while (stack.Count > 0)
                {
                    List<int> newStack = new List<int> { };

                    foreach (var source in stack)
                    {
                        foreach (var dst in graph[source])
                        {
                            if (dst != origin && distances[dst] == 0)
                            {
                                distances[dst] = distances[source] + 1;
                                newStack.Add(dst);
                            }
                        }
                    }

                    stack = newStack;
                }
            }

            List<(int, int, int, int)> states = new List<(int, int, int, int)> { (start, 0, 0, 0) };
            int[] best = new int[4194304];
            int skipCount = 0;
            int max = 0;

            for (int round = 1; round <= 29; round++)
            {
                List<(int, int, int, int)> newStates = new List<(int, int, int, int)>();

                foreach (var (n, bits, flow, acc) in states)
                {
                    int code = (n << 16) + bits;
                    int projected = acc + flow * (30 - round + 1);

                    if (best[code] > projected + 1)
                    {
                        skipCount++;
                        continue;
                    }

                    if (flowRates[n] > 0 && (bits & (1 << n)) == 0)
                    {
                        int nBits = bits | (1 << n);
                        int nFlow = flow + flowRates[n];
                        code = (n << 16) + nBits;
                        projected = acc + flow + nFlow * (30 - round);

                        if (projected + 1 > best[code])
                        {
                            newStates.Add((n, nBits, nFlow, acc + flow));
                            best[code] = projected + 1;
                            if (projected > max) max = projected;
                        }
                    }

                    foreach (int dst in graph[n])
                    {
                        code = (dst << 16) + bits;
                        projected = acc + flow * (30 - round + 1);

                        if (projected + 1 > best[code])
                        {
                            newStates.Add((dst, bits, flow, acc + flow));
                            best[code] = projected + 1;
                            if (projected > max) max = projected;
                        }
                    }
                }

                states = newStates;
            }

            var maxPressure1 = max.ToString();

            max = 0;
            bool expensive = true;
            best = new int[expensive ? 1 << 27 : 1 << 15];
            List<List<State>> stacks = new List<List<State>>();
            stacks.Add(new List<State> { new State { previousNode = start, nextNode = start } });

            for (int i = 1; i < 26; i++) stacks.Add(new List<State>());

            for (int time = 0; time < 26; time++)
            {
                foreach (State state in stacks[time])
                {
                    int projection = state.acc + state.previousFlow * (26 - state.time);

                    if (state.timeRemaining <= 26) projection += state.nextFlow * (26 - state.timeRemaining);

                    if (projection > max) max = projection;

                    int code = state.bits;

                    if (expensive) code += (state.previousNode < state.nextNode) ? (state.previousNode << 21) + (state.nextNode << 15) : (state.nextNode << 21) + (state.previousNode << 15);

                    if (best[code] > projection + 1) continue;

                    best[code] = projection + 1;

                    foreach (State tState in GenerateStates(state))
                        if (tState.timeRemaining < 26) stacks[tState.time].Add(tState);
                }
            }

            var maxPressure2 = max.ToString();

            List<State> GenerateStates(State state)
            {
                List<State> results = new List<State>();

                for (int i = 0; i < valvesNum; i++)
                {
                    if ((state.bits & (1 << i)) == 0)
                    {
                        State newState = new State { time = state.timeRemaining, previousNode = state.nextNode, previousFlow = state.nextFlow };
                        int cost = cGraph[state.previousNode][i] + 1; 
                        newState.bits = state.bits | (1 << i);
                        newState.acc = state.acc + state.previousFlow * cost;
                        newState.nextNode = i;
                        newState.nextFlow = state.previousFlow + flowRates[i];
                        newState.timeRemaining = state.time + cost;

                        if (newState.timeRemaining < newState.time) newState = new State
                        {
                            time = newState.timeRemaining,
                            previousNode = newState.nextNode,
                            previousFlow = newState.nextFlow,
                            timeRemaining = newState.time,
                            nextNode = newState.previousNode,
                            nextFlow = newState.previousFlow,
                            acc = newState.acc,
                            bits = newState.bits
                        };

                        results.Add(newState);
                    }
                }

                if (state.time == state.timeRemaining)
                {
                    List<State> newResults = new List<State>();

                    foreach (State tState in results) newResults.AddRange(GenerateStates(tState));

                    results = newResults;
                }

                return results;
            }

            sw.Stop();
            Console.WriteLine($"Max pressure in part 1: {maxPressure1} | Max pressure in part 2: {maxPressure2}\nTime elapsed: {sw.Elapsed.Milliseconds}ms.\n\n");
            Console.ReadKey();
        }

        public struct State
        {
            public int time, timeRemaining, previousNode, nextNode, previousFlow, nextFlow, acc, bits;
        }
    }
}

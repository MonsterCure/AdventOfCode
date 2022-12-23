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
            List<int[]> graph = new List<int[]>(), cgraph = new List<int[]>();
            List<int> flowRates = new List<int>();
            Dictionary<string, int> labelIndex = new Dictionary<string, int>();
            int start, valvesNum = 0;

            for (int i = 0; i < input.Count; i++)
            {
                Match m = regex.Match(input[i]);
                valves.Add((m.Groups[1].Value, int.Parse(m.Groups[2].Value), m.Groups[3].Value.Split(new string[] { ", " }, StringSplitOptions.None)));
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
                int[] dist = new int[graph.Count];
                cgraph.Add(dist);
                List<int> stack = new List<int> { origin };

                if (flowRates[origin] == 0 && origin != start) continue;
                if (flowRates[origin] > 0) valvesNum++;

                while (stack.Count > 0)
                {
                    List<int> nstack = new List<int> { };

                    foreach (var src in stack)
                    {
                        foreach (var dst in graph[src])
                        {
                            if (dst != origin && dist[dst] == 0)
                            {
                                dist[dst] = dist[src] + 1;
                                nstack.Add(dst);
                            }
                        }
                    }
                    stack = nstack;
                }
            }

            List<(int, int, int, int)> states = new List<(int, int, int, int)> { (start, 0, 0, 0) };
            int[] best = new int[4194304];
            int skipcnt = 0;
            int max = 0;

            for (int round = 1; round <= 29; round++)
            {
                List<(int, int, int, int)> nstates = new List<(int, int, int, int)>();

                foreach (var (n, bits, flow, acc) in states)
                {
                    int code = (n << 16) + bits;
                    int projected = acc + flow * (30 - round + 1);

                    if (best[code] > projected + 1)
                    {
                        skipcnt++;
                        continue;
                    }

                    if (flowRates[n] > 0 && (bits & (1 << n)) == 0)
                    {
                        int nbits = bits | (1 << n);
                        int nflow = flow + flowRates[n];
                        code = (n << 16) + nbits;
                        projected = acc + flow + nflow * (30 - round);
                        if (projected + 1 > best[code])
                        {
                            nstates.Add((n, nbits, nflow, acc + flow));
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
                            nstates.Add((dst, bits, flow, acc + flow));
                            best[code] = projected + 1;
                            if (projected > max) max = projected;
                        }
                    }
                }
                states = nstates;
            }

            var maxPressure1 = max.ToString();

            max = 0;
            bool expensive = true;
            best = new int[expensive ? 1 << 27 : 1 << 15];
            List<List<State>> stacks = new List<List<State>>();
            stacks.Add(new List<State> { new State { pa = start, pb = start } });
            for (int i = 1; i < 26; i++) stacks.Add(new List<State>());
            for (int time = 0; time < 26; time++)
            {
                foreach (State s in stacks[time])
                {
                    int projection = s.acc + s.fa * (26 - s.ta);
                    if (s.tb <= 26) projection += s.fb * (26 - s.tb);
                    if (projection > max) max = projection;
                    int code = s.bits;
                    if (expensive) code += (s.pa < s.pb) ? (s.pa << 21) + (s.pb << 15) : (s.pb << 21) + (s.pa << 15);
                    if (best[code] > projection + 1) continue;
                    best[code] = projection + 1;
                    foreach (State t in generate(s)) if (t.tb < 26) stacks[t.ta].Add(t);
                }
            }

            var maxPressure2 = max.ToString();

            List<State> generate(State s)
            {
                List<State> results = new List<State>();
                for (int i = 0; i < valvesNum; i++)
                {
                    if ((s.bits & (1 << i)) == 0)
                    {
                        State d = new State { ta = s.tb, pa = s.pb, fa = s.fb };
                        int cost = cgraph[s.pa][i] + 1; 
                        d.bits = s.bits | (1 << i);
                        d.acc = s.acc + s.fa * cost;
                        d.pb = i;
                        d.fb = s.fa + flowRates[i];
                        d.tb = s.ta + cost;

                        if (d.tb < d.ta) d = new State
                        {
                            ta = d.tb,
                            pa = d.pb,
                            fa = d.fb,
                            tb = d.ta,
                            pb = d.pa,
                            fb = d.fa,
                            acc = d.acc,
                            bits = d.bits
                        };
                        results.Add(d);
                    }
                }
                if (s.ta == s.tb)
                {
                    List<State> nresults = new List<State>();
                    foreach (State t in results) nresults.AddRange(generate(t));
                    results = nresults;
                }
                return results;
            }

            Console.WriteLine($"Max pressure in part 1: {maxPressure1} | Max pressure in part 2: {maxPressure2}\n");
            sw.Stop();
            Console.WriteLine($"Time elapsed: {sw.Elapsed.Milliseconds}ms.\n\n");
            Console.ReadKey();
        }

        public struct State
        {
            public int ta, tb, pa, pb, fa, fb, acc, bits;
        }
    }
}

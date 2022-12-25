using System.Diagnostics;

namespace AoC_2022_Solutions
{
    public class Day15 //--- Day 15: Beacon Exclusion Zone ---  
    {
        public static void Part01and02()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var input = File.ReadAllLines(@"..\..\..\..\AoC 2022 Inputs\Day15.txt").ToList();

            var sensors = new HashSet<(int x, int y)>();
            var beacons = new HashSet<(int x, int y)>();
            var noBeacons = new HashSet<(int x, int y)>();
            var diamonds = new List<(int x, int y, int r)>();
            var possiblePositions = new HashSet<(int x, int y)>();

            const int ROW = 2000000;
            const int GRID_START = 0;
            const int GRID_END = 4000000;

            Int64 tuningFrequency = 0;

            var parsedCoordinates = (
                        from line in input
                        from splits in line.Split(':')
                        let parts = splits.Split('=').Skip(1)
                        select (x: int.Parse(parts.ElementAt(0).Substring(0, Math.Max(parts.ElementAt(0).IndexOf(','), 0))), y: int.Parse(parts.ElementAt(1)))).ToList();

            for (int i = 0; i < parsedCoordinates.Count(); i += 2)
            {
                var sensor = (parsedCoordinates[i].x, parsedCoordinates[i].y);
                sensors.Add(sensor);
                var beacon = (parsedCoordinates[i + 1].x, parsedCoordinates[i + 1].y);
                beacons.Add(beacon);
                var manhDistance = Math.Abs(sensor.x - beacon.x) + Math.Abs(sensor.y - beacon.y);
                diamonds.Add((sensor.x, sensor.y, manhDistance));

                if (ROW > manhDistance + sensor.y && ROW < sensor.y - manhDistance)
                    continue;

                var rowDistance = Math.Abs(sensor.y - ROW);
                var sensorReachOnX = manhDistance - rowDistance;

                for (var x = sensor.x - sensorReachOnX; x < sensor.x + sensorReachOnX; x++)
                    noBeacons.Add((x, ROW));
            }

            for (int i = 0; i < diamonds.Count; i++)
            {
                var diamond1 = diamonds[i];

                for (int j = 0; j < diamonds.Count; j++)
                {
                    var diamond2 = diamonds[j];

                    if (i == j)
                        continue;

                    var manhDistance = Math.Abs(diamond1.x - diamond2.x) + Math.Abs(diamond1.y - diamond2.y);

                    if (manhDistance == diamond1.r + diamond2.r + 2)
                    {
                        int endY = Math.Min(diamond1.y + diamond1.r, diamond2.y + diamond2.r);
                        int startY = Math.Max(diamond1.y - diamond1.r, diamond2.y - diamond2.r);

                        int startX = Math.Max(diamond1.x - diamond1.r, diamond2.x - diamond2.r);
                        int endX = Math.Min(diamond1.x + diamond1.r, diamond2.x + diamond2.r);

                        for (int y = startY; y < endY; y++)
                        {
                            int x1 = diamond1.x + (diamond1.r + 1 - Math.Abs(y - diamond1.y));
                            int x2 = diamond1.x - (diamond1.r + 1 - Math.Abs(y - diamond1.y));

                            if (x1 >= GRID_START && x1 <= GRID_END && x1 >= startX && x1 <= endX)
                                possiblePositions.Add((x1, y));
                            if (x2 >= GRID_START && x2 <= GRID_END && x2 >= startX && x2 <= endX)
                                possiblePositions.Add((x2, y));
                        }
                    }
                }
            }

            foreach (var point in possiblePositions)
            {
                if (sensors.Contains(point) || beacons.Contains(point))
                    continue;

                bool found = true;

                foreach (var diamond in diamonds)
                {
                    var manhDistance = Math.Abs(point.x - diamond.x) + Math.Abs(point.y - diamond.y);
                    var isPointInDiamond = manhDistance <= diamond.r ? true : false;

                    if (isPointInDiamond)
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                    tuningFrequency = (Int64)point.x * 4000000L + (Int64)point.y;
            }

            sw.Stop();
            Console.WriteLine($"Positions that can't contain a beacon: {noBeacons.Count}\nThe tuning frequency of the distress beacon: {tuningFrequency}\nTime elapsed: {sw.Elapsed.Milliseconds}ms.\n\n");
            Console.ReadKey();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2022_Solutions
{
    public class Day18 //--- Day 18: Boiling Boulders ---
    {
        public static void Part01and02()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var input = File.ReadAllLines(@"..\..\..\..\AoC 2022 Inputs\Day18.txt").ToList();
            var cubes = new HashSet<Vector3>();

            foreach (var line in input)
            {
                var coordinates = line.Split(',').Select(e => int.Parse(e)).ToArray();
                var cube = new Vector3(coordinates[0], coordinates[1], coordinates[2]);
                cubes.Add(cube);
            }

            var dropletArea = cubes.SelectMany(c => GetNeighbours(c)).Count(c => !cubes.Contains(c));

            var bounds = GetBounds(cubes);
            var waterLocations = FillWithWater(bounds.Min, bounds, cubes);
            var exteriorArea = cubes.SelectMany(c => GetNeighbours(c)).Count(c => waterLocations.Contains(c));

            Console.WriteLine($"The surface area of the scanned lava droplet is {dropletArea}.\nThe exterior surface area of your scanned lava droplet is {exteriorArea}.\n");
            sw.Stop();
            Console.WriteLine($"Time elapsed: {sw.Elapsed.Milliseconds}ms.\n\n");
            Console.ReadKey();
        }

        internal static HashSet<Vector3> FillWithWater(Vector3 from, Bounds bounds, HashSet<Vector3> cubes)
        {
            var result = new HashSet<Vector3>();
            var queue = new Queue<Vector3>();

            result.Add(from);
            queue.Enqueue(from);

            while (queue.Any())
            {
                var water = queue.Dequeue();

                foreach (var neighbour in GetNeighbours(water))
                {
                    if (!result.Contains(neighbour) && IsWithin(bounds, neighbour) && !cubes.Contains(neighbour))
                    {
                        result.Add(neighbour);
                        queue.Enqueue(neighbour);
                    }
                }
            }

            return result;
        }

        internal static Bounds GetBounds(IEnumerable<Vector3> cubes)
        {
            var minX = cubes.Select(c => c.X).Min() - 1;
            var maxX = cubes.Select(c => c.X).Max() + 1;

            var minY = cubes.Select(c => c.Y).Min() - 1;
            var maxY = cubes.Select(c => c.Y).Max() + 1;

            var minZ = cubes.Select(c => c.Z).Min() - 1;
            var maxZ = cubes.Select(c => c.Z).Max() + 1;

            return new Bounds(new Vector3(minX, minY, minZ), new Vector3(maxX, maxY, maxZ));
        }

        internal static List<Vector3> GetNeighbours(Vector3 cube)
        {
            return new List<Vector3>
            {
                new Vector3(cube.X - 1, cube.Y, cube.Z),
                new Vector3(cube.X + 1, cube.Y, cube.Z),
                new Vector3(cube.X, cube.Y - 1, cube.Z),
                new Vector3(cube.X, cube.Y + 1, cube.Z),
                new Vector3(cube.X, cube.Y, cube.Z - 1),
                new Vector3(cube.X, cube.Y, cube.Z + 1)
            };
        }

        internal static bool IsWithin(Bounds bounds, Vector3 cube)
        {
            return bounds.Min.X <= cube.X && cube.X <= bounds.Max.X &&
                   bounds.Min.Y <= cube.Y && cube.Y <= bounds.Max.Y &&
                   bounds.Min.Z <= cube.Z && cube.Z <= bounds.Max.Z;
        }

        internal class Bounds
        {
            public Vector3 Min;
            public Vector3 Max;

            public Bounds(Vector3 min, Vector3 max)
            {
                Min = min;
                Max = max;
            }
        }
    }
}

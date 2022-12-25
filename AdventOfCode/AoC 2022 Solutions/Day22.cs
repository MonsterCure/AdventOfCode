using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AoC_2022_Solutions
{
    public class Day22 //--- Day 22: Monkey Map ---
    {
        public static void Part01and02()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var input = File.ReadAllLines(@"..\..\..\..\AoC 2022 Inputs\Day22.txt").ToList();

            var map = new Dictionary<(int X, int Y), char>();
            int index = 0;

            for (index = 0; input[index] != ""; index++)
            {
                for (int x = 0; x < input[index].Length; x++)
                {
                    char read;

                    if ((read = input[index][x]) != ' ') map[(x, index)] = read;
                }
            }
            index++;

            int maxX = map.Max(c => c.Key.X) + 1;
            int maxY = map.Max(c => c.Key.Y) + 1;

            string _path = string.Join("", input.Skip(index));
            Regex regex = new Regex(@"(\d+|[LR])", RegexOptions.Compiled);
            var parsedPath = regex.Matches(_path);
            string[] path = new string[parsedPath.Count];

            for (int i = 0; i < path.Length; i++)
                path[i] = parsedPath[i].Groups[1].Value;

            var move = new (int x, int y)[] { (1, 0), (0, 1), (-1, 0), (0, -1) };
            var moveChar = new[] { '>', 'v', '<', '^' };
            var rotationChar = new[] { 'U', 'R', 'D', 'L' };

            {
                int x = map.Keys.First().X, y = 0;
                int direction = 0;

                for (int i = 0; i < path.Length; i++)
                {
                    int steps = 0;
                    var moveTo = move[direction];

                    if (path[i] == "L") direction = (direction + 3) % 4;
                    else if (path[i] == "R") direction = (direction + 1) % 4;
                    else steps = int.Parse(path[i]);

                    for (int j = 0; j < steps; j++)
                    {
                        int nextX = x, nextY = y;

                        do
                        {
                            nextX = (nextX + moveTo.x + maxX) % maxX;
                            nextY = (nextY + moveTo.y + maxY) % maxY;

                        } while (map.Read((nextX, nextY)) == '\0');

                        if (map.Read((nextX, nextY)) != '#')
                        {
                            x = nextX;
                            y = nextY;
                        }
                        else break;
                    }
                }

                Console.WriteLine($"Part 1 final password: {(y + 1) * 1000 + (x + 1) * 4 + direction}\n");
            }

            int cubeSize = Math.Abs(maxX - maxY);
            int[] cube = new int[16];
            (int face, int rotation)[,] connects = new (int Face, int Rotation)[7, 4];
            int face = 0;

            for (int sideY = 0; sideY < maxY; sideY += cubeSize)
            {
                for (int sideX = 0; sideX < maxX; sideX += cubeSize)
                {
                    if (map.Read((sideX, sideY)) != '\0')
                    {
                        face++;
                        cube[sideX / cubeSize + (sideY / cubeSize) * 4] = face;
                    }
                }
            }

            for (int sideY = 0; sideY < 4; sideY++)
            {
                for (int sideX = 0; sideX < 4; sideX++)
                {
                    face = cube[sideX + sideY * 4];
                    if (face != 0)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            var moveTo = move[i];
                            int nextX = (sideX + moveTo.x + 4) % 4;
                            int nextY = (sideY + moveTo.y + 4) % 4;
                            int target = cube[nextX + nextY * 4];
                            if (target != 0) connects[face, i] = (target, 0);
                        }
                    }
                }
            }

            var find = new (int d, (int f, int r)[] opposite)[] {
                (0, new[] { (1, -1), (3, 1) }),
                (1, new[] { (0, 1), (2, -1) }),
                (2, new[] { (1, 1), (3, -1) }),
                (3, new[] { (0, -1), (2, 1) }),
            };

            for (int _ = 0; _ < 3; _++)
            {
                for (int i = 1; i <= 6; i++)
                {
                    foreach (var (d, opposite) in find)
                    {
                        var facing = connects[i, d];

                        if (facing.face == 0) continue;

                        face = facing.face;

                        foreach (var (f, r) in opposite)
                        {
                            if (connects[i, f].face != 0) continue;

                            var turning = connects[face, (f + facing.rotation) % 4];

                            for (int s = 0; s < 4; s++)
                                if (connects[i, s].face == turning.face) goto skip;

                            if (turning.face != 0 && i != turning.face)
                            {
                                int rotn = (facing.rotation + turning.rotation + r + 4) % 4;
                                connects[i, f] = (turning.face, rotn);
                            }

                        skip:;
                        }
                    }
                }
            }

            {
                int x = map.Keys.First().X, y = 0;
                int direction = 0;

                for (int i = 0; i < path.Length; i++)
                {
                    int steps = 0;
                    int cellX = x / cubeSize;
                    int cellY = y / cubeSize;
                    face = cube[cellX + cellY * 4];

                    if (path[i] == "L") direction = (direction + 3) % 4;
                    else if (path[i] == "R") direction = (direction + 1) % 4;
                    else
                    {
                        steps = int.Parse(path[i]);

                        for (int j = 0; j < steps; j++)
                        {
                            var moveTo = move[direction];
                            int nextX = x, nextY = y;
                            int nextdDirection = direction;
                            {
                                nextX = nextX + moveTo.x;
                                nextY = nextY + moveTo.y;
                                int nextCellX = (int)Math.Floor(nextX / (float)cubeSize);
                                int nextCellY = (int)Math.Floor(nextY / (float)cubeSize);
                                int moveToIndex = Array.IndexOf(move, (nextCellX - cellX, nextCellY - cellY));

                                if (moveToIndex != -1)
                                {
                                    var (f, r) = connects[face, moveToIndex];
                                    int cubeIndex = Array.IndexOf(cube, f);
                                    int cubeX = cubeIndex % 4;
                                    int cubeY = cubeIndex / 4;
                                    int oppositeX = (nextX + cubeSize) % cubeSize - cubeSize / 2;
                                    int oppositeY = (nextY + cubeSize) % cubeSize - cubeSize / 2;

                                    switch (r)
                                    {
                                        case 0:
                                            break;
                                        case 1:
                                            nextdDirection = (direction + 1) % 4;
                                            (oppositeX, oppositeY) = (-oppositeY - 1, oppositeX);
                                            break;
                                        case 2:
                                            nextdDirection = (direction + 2) % 4;
                                            (oppositeX, oppositeY) = (-oppositeX - 1, -oppositeY - 1);
                                            break;
                                        case 3:
                                            nextdDirection = (direction + 3) % 4;
                                            (oppositeX, oppositeY) = (oppositeY, -oppositeX - 1);
                                            break;
                                    }

                                    oppositeX += cubeSize / 2;
                                    oppositeY += cubeSize / 2;
                                    nextX = (cubeX * cubeSize + oppositeX);
                                    nextY = (cubeY * cubeSize + oppositeY);
                                }
                            }

                            if (map.Read((nextX, nextY)) != '#')
                            {
                                x = nextX;
                                y = nextY;
                                direction = nextdDirection;
                            }
                            else break;
                        }
                    }
                }

                Console.WriteLine($"Part 2 final password: {(y + 1) * 1000 + (x + 1) * 4 + direction}\n");
            }

            sw.Stop();
            Console.WriteLine($"Time elapsed: {sw.Elapsed.Milliseconds}ms.\n\n");
            Console.ReadKey();
        }
    }
}

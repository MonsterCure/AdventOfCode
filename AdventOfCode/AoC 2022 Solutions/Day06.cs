using System.Diagnostics;

namespace AoC_2022_Solutions
{
    public class Day06
    {
        public static void Part01()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var input = File.ReadAllText(@"..\..\..\..\AoC 2022 Inputs\Day06.txt");

            for (int i = 0; i <= input.Length; i++)
            {
                string checkString = input.Substring(i);

                if (checkString.Length >= 4)
                {
                    string marker = input.Substring(i, 4);
                    int uniqueLetters = marker.Distinct().Count();

                    if (uniqueLetters == 4)
                    {
                        Console.WriteLine($"The marker is {marker} at index {i} with processed {i + 4} characters.\n");
                        break;
                    }
                }
            }

            sw.Stop();
            Console.WriteLine($"Time elapsed: {sw.Elapsed.Milliseconds}ms.\n\n");
            Console.ReadKey();
        }

        public static void Part02()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var input = File.ReadAllText(@"..\..\..\..\AoC 2022 Inputs\Day06.txt");

            for (int i = 0; i <= input.Length; i++)
            {
                string checkString = input.Substring(i);

                if (checkString.Length >= 14)
                {
                    string marker = input.Substring(i, 14);
                    int uniqueLetters = marker.Distinct().Count();

                    if (uniqueLetters == 14)
                    {
                        Console.WriteLine($"The marker is {marker} at index {i} with processed {i + 14} characters.\n");
                        break;
                    }
                }
            }

            sw.Stop();
            Console.WriteLine($"Time elapsed: {sw.Elapsed.Milliseconds}ms.\n\n");
            Console.ReadKey();
        }

        public static void Part01and02(int characters)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var input = File.ReadAllText(@"..\..\..\..\AoC 2022 Inputs\Day06.txt");

            for (int i = 0; i <= input.Length; i++)
            {
                string checkString = input.Substring(i);

                if (checkString.Length >= characters)
                {
                    string marker = input.Substring(i, characters);
                    int uniqueLetters = marker.Distinct().Count();

                    if (uniqueLetters == characters)
                    {
                        Console.WriteLine($"The marker is {marker} at index {i} with processed {i + characters} characters.\n");
                        break;
                    }
                }
            }

            sw.Stop();
            Console.WriteLine($"Time elapsed: {sw.Elapsed.Milliseconds}ms.\n\n");
            Console.ReadKey();
        }
    }
}

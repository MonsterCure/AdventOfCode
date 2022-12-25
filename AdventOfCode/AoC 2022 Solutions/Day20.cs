using System.Diagnostics;

namespace AoC_2022_Solutions
{
    public class Day20 //--- Day 20: Grove Positioning System ---
    {
        public static void Part01and02()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var input = File.ReadAllLines(@"..\..\..\..\AoC 2022 Inputs\Day20.txt").ToList();

            var result1 = Mixing(input);
            var result2 = Mixing(input, 811589153L, 10);

            sw.Stop();
            Console.WriteLine($"Sum of the grove coordinates:\npart 1: {result1} | part 2: {result2}\nTime elapsed: {sw.Elapsed.Milliseconds}ms.\n\n");
            Console.ReadKey();
        }

        internal static Int64 Mixing(List<string> input, Int64 decriptionKey = 1, int mixCount = 1)
        {
            var parsedInput = input.Select(e => Int64.Parse(e) * decriptionKey).ToList();
            var encryptedFile = new List<(Int64 value, int index)>();

            for (int i = 0; i < parsedInput.Count; i++)
                encryptedFile.Add((parsedInput[i], i));

            var listToMix = new List<(Int64 value, int index)>(encryptedFile);
            var count = encryptedFile.Count;

            for (int mc = 0; mc < mixCount; mc++)
            {
                for (int i = 0; i < count; i++)
                {
                    var number = encryptedFile[i];
                    var oldIndex = listToMix.IndexOf(number);

                    var newIndex = (oldIndex + number.value) % (count - 1);

                    if (newIndex < 0) newIndex = count + newIndex - 1;

                    listToMix.Remove(number);
                    listToMix.Insert((int)newIndex, number);
                }
            }

            var indexZero = listToMix.FindIndex(e => e.value == 0);
            var index1000 = (1000 + indexZero) % count;
            var index2000 = (2000 + indexZero) % count;
            var index3000 = (3000 + indexZero) % count;

            var coordinatesSum = listToMix[index1000].value + listToMix[index2000].value + listToMix[index3000].value;

            return coordinatesSum;
        }
    }
}

using System.Diagnostics;

namespace AoC_2022_Solutions
{
    public class Day08 //--- Day 8: Treetop Tree House ---
    {
        public static void Part01and02()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var input = File.ReadAllLines(@"..\..\..\..\AoC 2022 Inputs\Day08.txt");

            int visibleTrees = 0;
            int highestScenicScore = 0;

            for (int row = 0; row < input.Length; row++)
            {
                string treeLine = input.ElementAt(row);

                for (int column = 0; column < treeLine.Length; column++)
                {
                    Tuple<bool, int> tuple = GetVisibilityAndScenicScore(input, row, column);
                    bool isVisible = tuple.Item1;

                    if (isVisible) visibleTrees++;

                    highestScenicScore = Math.Max(highestScenicScore, tuple.Item2);
                }
            }

            sw.Stop();
            Console.WriteLine($"Visible trees: {visibleTrees} | Highest scenic score: {highestScenicScore}.\nTime elapsed: {sw.Elapsed.Milliseconds}ms.\n\n");
            Console.ReadKey();
        }
        internal static Tuple<bool, int> GetVisibilityAndScenicScore(string[] treePatch, int x, int y)
        {
            int patchWidth = treePatch.ElementAt(0).Count();
            int patchLength = treePatch.Length;

            bool isVisible = false;
            bool isLineClear = true;
            int score = 1;
            int clearLineLength = 0;

            //corners
            if (x == 0 || y == 0 || (x == patchLength - 1) || (y == patchWidth - 1))
            {
                isVisible = true;
                return new Tuple<bool, int>(isVisible, score);
            }

            //upwards
            for (int row = x - 1; row >= 0; row--)
            {
                clearLineLength++;

                if (treePatch.ElementAt(row).ElementAt(y) >= treePatch.ElementAt(x).ElementAt(y))
                {
                    isLineClear = false;
                    break;
                }
            }

            isVisible = isVisible || isLineClear;
            isLineClear = true;
            score *= clearLineLength;
            clearLineLength = 0;

            //downwards
            for (int row = x + 1; row < patchLength; row++)
            {
                clearLineLength++;

                if (treePatch.ElementAt(row).ElementAt(y) >= treePatch.ElementAt(x).ElementAt(y))
                {
                    isLineClear = false;
                    break;
                }
            }

            isVisible = isVisible || isLineClear;
            isLineClear = true;
            score *= clearLineLength;
            clearLineLength = 0;

            //right
            for (int column = y + 1; column < patchWidth; column++)
            {
                clearLineLength++;

                if (treePatch.ElementAt(x).ElementAt(column) >= treePatch.ElementAt(x).ElementAt(y))
                {
                    isLineClear = false;
                    break;
                }
            }

            isVisible = isVisible || isLineClear;
            isLineClear = true;
            score *= clearLineLength;
            clearLineLength = 0;

            //left
            for (int column = y - 1; column >= 0; column--)
            {
                clearLineLength++;

                if (treePatch.ElementAt(x).ElementAt(column) >= treePatch.ElementAt(x).ElementAt(y))
                {
                    isLineClear = false;
                    break;
                }
            }

            isVisible = isVisible || isLineClear;
            score *= clearLineLength;

            return new Tuple<bool, int>(isVisible, score);
        }
    }
}

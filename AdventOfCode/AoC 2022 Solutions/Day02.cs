using System.Diagnostics;

namespace AoC_2022_Solutions
{
    public class Day02 //--- Day 2: Rock Paper Scissors ---
    {
        public static void Part01()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            string[] game = File.ReadAllLines(@"..\..\..\..\AoC 2022 Inputs\Day02.txt");

            int score = 0;

            foreach (string round in game)
            {
                string opponentPlay = round.ElementAt(0).ToString();
                string ownPlay = round.ElementAt(2).ToString();

                if (ownPlay == "X")
                {
                    score += 1;

                    if (opponentPlay == "A")
                        score += 3;
                    else if (opponentPlay == "C")
                        score += 6;
                }
                else if (ownPlay == "Y")
                {
                    score += 2;
                    if (opponentPlay == "A")
                        score += 6;
                    else if (opponentPlay == "B")
                        score += 3;
                }
                else if (ownPlay == "Z")
                {
                    score += 3;
                    if (opponentPlay == "B")
                        score += 6;
                    else if (opponentPlay == "C")
                        score += 3;
                }
            }

            Console.WriteLine($"Your Rock Paper Scissors score is {score}.\n");
            sw.Stop();
            Console.WriteLine($"Time elapsed: {sw.Elapsed.Milliseconds}ms.\n\n");
            Console.ReadKey();
        }

        public static void Part02()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            string[] game = File.ReadAllLines(@"..\..\..\..\AoC 2022 Inputs\Day02.txt");

            int score = 0;

            foreach (string round in game)
            {
                string opponentPlay = round.ElementAt(0).ToString();
                string loseDrawWin = round.ElementAt(2).ToString();

                if (loseDrawWin == "X")
                {
                    if (opponentPlay == "A")
                        score += 3;
                    else if (opponentPlay == "B")
                        score += 1;
                    else if (opponentPlay == "C")
                        score += 2;
                }
                else if (loseDrawWin == "Y")
                {
                    score += 3;
                    if (opponentPlay == "A")
                        score += 1;
                    else if (opponentPlay == "B")
                        score += 2;
                    else if (opponentPlay == "C")
                        score += 3;
                }
                else if (loseDrawWin == "Z")
                {
                    score += 6;
                    if (opponentPlay == "A")
                        score += 2;
                    else if (opponentPlay == "B")
                        score += 3;
                    else if (opponentPlay == "C")
                        score += 1;
                }
            }

            Console.WriteLine($"Your Rock Paper Scissors score is {score}.\n");
            sw.Stop();
            Console.WriteLine($"Time elapsed: {sw.Elapsed.Milliseconds}ms.\n\n");
            Console.ReadKey();
        }
    }
}

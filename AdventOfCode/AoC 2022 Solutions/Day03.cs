using System.Diagnostics;

namespace AoC_2022_Solutions
{
    public class Day03 //--- Day 3: Rucksack Reorganization ---
    {
        public static void Part01()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            List<string> rucksackItemsList = new List<string>(File.ReadAllLines(@"..\..\..\..\AoC 2022 Inputs\Day03.txt"));

            int prioritiesSum = 0;

            foreach (var rucksackItems in rucksackItemsList)
            {
                string firstCompartment = rucksackItems.Substring(0, rucksackItems.Length / 2);
                string secondCompartment = rucksackItems.Substring(rucksackItems.Length / 2, rucksackItems.Length / 2);
                char repeatedItem = '\0';

                foreach (var item in secondCompartment)
                {
                    if (firstCompartment.Contains(item))
                    {
                        repeatedItem = item;
                        break;
                    }
                }

                prioritiesSum += GetItemPriority(repeatedItem);
            }

            Console.WriteLine($"The sum of the priorities is {prioritiesSum}.\n");
            sw.Stop();
            Console.WriteLine($"Time elapsed: {sw.Elapsed.Milliseconds}ms.\n\n");
            Console.ReadKey();
        }

        public static void Part02()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            List<string> rucksackItemsList = new List<string>(File.ReadAllLines(@"..\..\..\..\AoC 2022 Inputs\Day03.txt"));

            int prioritiesSum = 0;

            for (int i = 0; i < rucksackItemsList.Count; i += 3)
            {
                string firstElfSack = rucksackItemsList.ElementAt(i);
                string secondELfSack = rucksackItemsList.ElementAt(i + 1);
                string thirdElfSack = rucksackItemsList.ElementAt(i + 2);
                char repeatedItem = '\0';

                foreach (var item in secondELfSack)
                {
                    if (firstElfSack.Contains(item) && thirdElfSack.Contains(item))
                    {
                        repeatedItem = item;
                        break;
                    }
                }

                prioritiesSum += GetItemPriority(repeatedItem);
            }

            Console.WriteLine($"The sum of the priorities is {prioritiesSum}.\n");
            sw.Stop();
            Console.WriteLine($"Time elapsed: {sw.Elapsed.Milliseconds}ms.\n\n");
            Console.ReadKey();
        }

        public static void Parts01and02()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            List<string> rucksackItemsList = new List<string>(File.ReadAllLines(@"..\..\..\..\AoC 2022 Inputs\Day03.txt"));

            int prioritiesSum = 0;

            foreach (var rucksackItems in rucksackItemsList)
            {
                string firstCompartment = rucksackItems.Substring(0, rucksackItems.Length / 2);
                string secondCompartment = rucksackItems.Substring(rucksackItems.Length / 2, rucksackItems.Length / 2);
                char repeatedItem = '\0';

                foreach (var item in secondCompartment)
                {
                    if (firstCompartment.Contains(item))
                    {
                        repeatedItem = item;
                        break;
                    }
                }

                prioritiesSum += GetItemPriority(repeatedItem);
            }

            Console.WriteLine($"The sum of the repeated items' priorities is {prioritiesSum}.\n");

            prioritiesSum = 0;

            for (int i = 0; i < rucksackItemsList.Count; i += 3)
            {
                string firstElfSack = rucksackItemsList.ElementAt(i);
                string secondELfSack = rucksackItemsList.ElementAt(i + 1);
                string thirdElfSack = rucksackItemsList.ElementAt(i + 2);
                char repeatedItem = '\0';

                foreach (var item in secondELfSack)
                {
                    if (firstElfSack.Contains(item) && thirdElfSack.Contains(item))
                    {
                        repeatedItem = item;
                        break;
                    }
                }
                prioritiesSum += GetItemPriority(repeatedItem);
            }

            Console.WriteLine($"The sum of the priorities between groups of three elves is {prioritiesSum}.\n");
            sw.Stop();
            Console.WriteLine($"Time elapsed: {sw.Elapsed.Milliseconds}ms.\n\n");
            Console.ReadKey();
        }

        internal static int GetItemPriority(char item)
        {
            return item - 'a' >= 0 ? item - 'a' + 1 : item - 'A' + 27;
        }
    }
}


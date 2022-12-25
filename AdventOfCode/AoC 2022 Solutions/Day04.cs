using System.Diagnostics;

namespace AoC_2022_Solutions
{
    public class Day04 //--- Day 4: Camp Cleanup ---
    {
        public static void Part01()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var sectionAssignmentsList = File.ReadAllLines(@"..\..\..\..\AoC 2022 Inputs\Day04.txt");

            int containedPairs = 0;

            foreach (var sectionAssignmentsPair in sectionAssignmentsList)
            {
                List<string[]> sections = sectionAssignmentsPair.Split(',').Select(pair => pair.Split('-')).ToList();

                int firstStart = int.Parse(sectionAssignmentsPair.Split(',').ElementAt(0).Split('-').ElementAt(0));
                int firstEnd = int.Parse(sectionAssignmentsPair.Split(',').ElementAt(0).Split('-').ElementAt(1));
                int secondStart = int.Parse(sectionAssignmentsPair.Split(',').ElementAt(1).Split('-').ElementAt(0));
                int secondEnd = int.Parse(sectionAssignmentsPair.Split(',').ElementAt(1).Split('-').ElementAt(1));

                if ((firstStart <= secondStart && firstEnd >= secondEnd) || (secondStart <= firstStart && secondEnd >= firstEnd))
                    containedPairs++;
            }

            sw.Stop();
            Console.WriteLine($"Pairs where one range contains the other: {containedPairs}.\nTime elapsed: {sw.Elapsed.Milliseconds}ms.\n\n");
            Console.ReadKey();
        }

        public static void Part02()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var sectionAssignmentsList = File.ReadAllLines(@"..\..\..\..\AoC 2022 Inputs\Day04.txt");

            int overlappingPairs = 0;

            foreach (var sectionAssignmentsPair in sectionAssignmentsList)
            {
                List<string[]> sections = sectionAssignmentsPair.Split(',').Select(pair => pair.Split('-')).ToList();

                int firstStart = int.Parse(sectionAssignmentsPair.Split(',').ElementAt(0).Split('-').ElementAt(0));
                int firstEnd = int.Parse(sectionAssignmentsPair.Split(',').ElementAt(0).Split('-').ElementAt(1));
                int secondStart = int.Parse(sectionAssignmentsPair.Split(',').ElementAt(1).Split('-').ElementAt(0));
                int secondEnd = int.Parse(sectionAssignmentsPair.Split(',').ElementAt(1).Split('-').ElementAt(1));

                if (secondStart <= firstEnd && firstStart <= secondEnd)
                    overlappingPairs++;
            }

            sw.Stop();
            Console.WriteLine($"Pairs where one range overlaps with the other: {overlappingPairs}.\nTime elapsed: {sw.Elapsed.Milliseconds}ms.\n\n");
            Console.ReadKey();
        }

        public static void Part01and02()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var assignmentsList = File.ReadAllLines(@"..\..\..\..\AoC 2022 Inputs\Day04.txt");

            int containedPairs = 0;
            int overlappingPairs = 0;

            foreach (var assignmentsPair in assignmentsList)
            {
                IEnumerable<IEnumerable<int>> sections = assignmentsPair.Split(',').Select(pair => pair.Split('-').Select(int.Parse));
                int[] limits = { sections.ElementAt(0).ElementAt(0), sections.ElementAt(0).ElementAt(1), sections.ElementAt(1).ElementAt(0), sections.ElementAt(1).ElementAt(1) };

                if ((limits[0] <= limits[2] && limits[1] >= limits[3]) || (limits[2] <= limits[0] && limits[3] >= limits[1]))
                    containedPairs++;

                if ((limits[2] <= limits[1]) && (limits[0] <= limits[3]))
                    overlappingPairs++;
            }

            sw.Stop();
            Console.WriteLine($"Pairs where one range contains the other: {containedPairs}.\nPairs where one range overlaps with the other: {overlappingPairs}.\nTime elapsed: {sw.Elapsed.Milliseconds}ms.\n\n");
            Console.ReadKey();
        }
    }
}

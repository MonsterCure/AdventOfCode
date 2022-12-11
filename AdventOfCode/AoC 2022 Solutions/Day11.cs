using System.Diagnostics;

namespace AoC_2022_Solutions
{
    public class Day11
    {
        internal class Item
        {
            public double WorryLevel { get; set; }
            public void GetNewWorryLevel(long worryRelief, long givenWorryRelief)
            {
                WorryLevel = worryRelief == givenWorryRelief ? Math.Floor(WorryLevel / worryRelief) : WorryLevel % worryRelief;
            }

            public Item(double worryLevel)
            {
                WorryLevel = worryLevel;
            }
        }

        internal class Monkey
        {
            public int MonkeyID { get; set; }
            public List<Item> Items { get; set; }

            public Func<double, double> operationPredicate;

            public int OperationValue { get; set; }

            public Func<double, bool> testPredicate;

            public int TestValue { get; set; }

            public (int ifTrue, int ifFalse) ThrowTo;

            public int InspectedItems;

            public Monkey(int monkeyID, List<Item> items, Func<double, double> operation, int operationValue, Func<double, bool> test, int testValue, (int ifTrue, int ifFalse) throwTo)
            {
                MonkeyID = monkeyID;
                Items = items;
                operationPredicate = operation;
                OperationValue = operationValue;
                testPredicate = test;
                TestValue = testValue;
                ThrowTo = throwTo;
            }

            public double InspectItem(double worryLevel)
            {
                return operationPredicate(worryLevel);
            }

            public bool ThrowItem(double worryLevel)
            {
                return testPredicate(worryLevel);
            }
        }

        /*internal static long LeastCommonMultiple(long[] numbers)
        {
            return numbers.Aggregate((num1, num2) => num1 * num2 / GreatestCommonDivisor(num1, num2));
        }

        internal static long GreatestCommonDivisor(long num1, long num2)
        {
            return (num2 == 0) ? num1 : GreatestCommonDivisor(num2, num1 % num2);
        }*/

        internal static List<Monkey> GetMonkeys(string[] input)
        {
            List<Monkey> monkeys = new List<Monkey>();

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i].StartsWith("Monkey"))
                {
                    int monkeyID = int.Parse(input[i].Substring(7).Replace(':', ' ').Trim());
                    string[] operations = input[i + 2].Substring(23).Split(' ');
                    string operation = operations[0];
                    int operationValue = 0;

                    if (!(operations[1] == "old"))
                        operationValue = int.Parse(operations[1]);

                    string[] parsedItems = input[i + 1].Substring(18).Split(' ');
                    List<Item> items = new List<Item>();

                    foreach (var item in parsedItems)
                    {
                        double itemValue = double.Parse(item.Replace(',', ' ').Trim());
                        items.Add(new Item(itemValue));
                    }

                    Func<double, double> operationPredicate;

                    switch (operation)
                    {
                        case "+":
                            if (operations[1] == "old")
                                operationPredicate = e => e += e;
                            else
                                operationPredicate = e => e = e + operationValue;
                            break;
                        case "*":
                            if (operations[1] == "old")
                                operationPredicate = e => e *= e;
                            else
                                operationPredicate = e => e *= operationValue;
                            break;
                        /*case "-":
                            if (operations[1] == "old")
                                operationPredicate = e => e -= e;
                            else
                                operationPredicate = e => e = e - operationValue;
                            break;
                        case "/":
                            if (operations[1] == "old")
                                operationPredicate = e => e /= e;
                            else
                                operationPredicate = e => e = e / operationValue;
                            break;*/
                        default:
                            throw new Exception();
                    }

                    int testValue = int.Parse(input[i + 3].Substring(21));
                    Func<double, bool> testPredicate = e => e % testValue == 0;

                    int throwToIfTrue = int.Parse(input[i + 4].Substring(29));
                    int throwToIfFalse = int.Parse(input[i + 5].Substring(30));

                    Monkey monkey = new Monkey(monkeyID, items, operationPredicate, operationValue, testPredicate, testValue, (throwToIfTrue, throwToIfFalse));
                    monkeys.Add(monkey);
                }
                else
                    continue;
            }

            return monkeys;
        }

        internal static void PlayKeepAway(ref List<Monkey> monkeys, int rounds, long worryRelief, long givenWorryRelief)
        {
            for (int i = 0; i < rounds; i++)
            {
                foreach (var monkey in monkeys)
                {
                    List<Item> itemsHelper = new List<Item>();
                    itemsHelper.AddRange(monkey.Items);

                    foreach (var item in itemsHelper)
                    {
                        item.WorryLevel = monkey.InspectItem(item.WorryLevel);
                        monkey.InspectedItems++;
                        item.GetNewWorryLevel(worryRelief, givenWorryRelief);

                        if (monkey.ThrowItem(item.WorryLevel))
                        {
                            monkeys.Single(m => m.MonkeyID == monkey.ThrowTo.ifTrue).Items.Add(item);
                            monkey.Items.Remove(item);
                        }
                        else
                        {
                            monkeys.Single(m => m.MonkeyID == monkey.ThrowTo.ifFalse).Items.Add(item);
                            monkey.Items.Remove(item);
                        }
                    }
                }
            }
        }

        public static void Part01and02(int part, int rounds, long worryRelief)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var input = File.ReadAllLines(@"..\..\..\..\AoC 2022 Inputs\Day11.txt");

            List<Monkey> monkeys = GetMonkeys(input);

            //long[] testValues = monkeys.Select(m => (long)m.TestValue).ToArray();
            //long lcm = LeastCommonMultiple(testValues);
            long lcm = monkeys.Aggregate(1, (mod, monkey) => mod * monkey.TestValue);
            long givenWorryRelief = worryRelief;
            worryRelief = (part == 1) ? worryRelief : lcm;

            PlayKeepAway(ref monkeys, rounds, worryRelief, givenWorryRelief);

            var reorderedMonkeys = monkeys.OrderByDescending(m => m.InspectedItems);
            long firstMonkey = reorderedMonkeys.ElementAt(0).InspectedItems;
            long secondMonkey = reorderedMonkeys.ElementAt(1).InspectedItems;
            long monkeyBusiness = firstMonkey * secondMonkey;

            Console.WriteLine($"Monkey Business: {firstMonkey} * {secondMonkey} = {monkeyBusiness}\n");
            sw.Stop();
            Console.WriteLine($"Time elapsed: {sw.Elapsed.Milliseconds}ms.\n\n");
            Console.ReadKey();
        }
    }
}

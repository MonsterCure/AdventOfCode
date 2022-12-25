using System.Diagnostics;

namespace AoC_2022_Solutions
{
    public class Day25
    {
        public static void Part01and02()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var input = File.ReadAllLines(@"..\..\..\..\AoC 2022 Inputs\Day25.txt").ToList();
            var numbers = new List<Int64>();

            for (int i = 0; i < input.Count; i++)
            {
                var numberString = input[i].Reverse();
                Int64 number = 0;

                for (int j = numberString.Count() - 1; j >= 0; j--)
                {
                    var multiplier = (Int64)Math.Pow(5, j);

                    if (numberString.ElementAt(j) == '=')
                        number += multiplier * (-2);
                    else if (numberString.ElementAt(j) == '-')
                        number += multiplier * (-1);
                    else
                    {
                        var value = (Int64)Char.GetNumericValue(numberString.ElementAt(j));
                        number += multiplier * value;
                    }
                }

                numbers.Add(number);
            }

            var result = numbers.Sum();
            var resultToPrint = result;
            var SNAFUResult = "";

            while (result > 0)
            {
                var number = result % 5;
                result /= 5;

                switch (number)
                {
                    case 0:
                        SNAFUResult = "0" + SNAFUResult;
                        break;
                    case 1:
                        SNAFUResult = "1" + SNAFUResult;
                        break;
                    case 2:
                        SNAFUResult = "2" + SNAFUResult;
                        break;
                    case 3:
                        SNAFUResult = "=" + SNAFUResult;
                        result++;
                        break;
                    case 4:
                        SNAFUResult = "-" + SNAFUResult;
                        result++;
                        break;
                    default:
                        throw new Exception();
                }
            }

            sw.Stop();
            Console.WriteLine($"Fuel requirements sum: {resultToPrint} | SNAFU sum: {SNAFUResult}\nTime elapsed: {sw.Elapsed.Milliseconds}ms.\n\n");
            Console.ReadKey();
        }
    }
}

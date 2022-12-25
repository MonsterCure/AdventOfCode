using System.Diagnostics;

namespace AoC_2022_Solutions
{
    public class Day13  //--- Day 13: Distress Signal ---
    {
        public static void Part01and02()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var input = File.ReadAllLines(@"..\..\..\..\AoC 2022 Inputs\Day13.txt").ToList();

            var packets = new List<List<object>>();

            for (int i = 0; i < input.Count; i += 3)
            {
                var leftPacket = ParseInputLine(input[i]);
                var rightPacket = ParseInputLine(input[i + 1]);
                packets.Add(leftPacket);
                packets.Add(rightPacket);
            }

            int indicesInOrderSum = 0;

            for (int i = 0; i < packets.Count; i += 2)
            {
                int inOrder = CompareValues(packets[i], packets[i + 1]);

                if (inOrder == 1)
                    indicesInOrderSum += i / 2 + 1;
            }

            var dividerPacket1 = new List<object> { new List<object> { 2 } };
            var dividerPacket2 = new List<object> { new List<object> { 6 } };
            packets.Add(dividerPacket1);
            packets.Add(dividerPacket2);

            packets.Sort((leftPacket, rightPacket) => CompareValues(rightPacket, leftPacket));
            int divider1Index = packets.IndexOf(dividerPacket1) + 1;
            int divider2Index = packets.IndexOf(dividerPacket2) + 1;
            int decoderKey = divider1Index * divider2Index;

            sw.Stop();
            Console.WriteLine($"Sum of indices of pairs in order: {indicesInOrderSum}\nDecoder key: {decoderKey}\nTime elapsed: {sw.Elapsed.Milliseconds}ms.\n\n");
            Console.ReadKey();
        }

        private static List<object> ParseInputLine(string inputLine)
        {
            var integers = new List<object>();
            var packetData = new Stack<List<object>>();
            integers.Add(null);
            packetData.Push(integers);
            int parseChar = 0;

            for (int i = 1; i < inputLine.Length; i++)
            {
                char character = inputLine[i];

                if ('0' <= character && character <= '9')
                {
                    parseChar = parseChar * 10 + (character - '0');
                    integers[integers.Count - 1] = parseChar;
                }
                else if (character == '[')
                {
                    var lists = new List<object>();
                    lists.Add(null);
                    integers[integers.Count - 1] = lists;
                    packetData.Push(integers);
                    integers = lists;
                }
                else if (character == ']')
                {
                    integers.Remove(null);
                    integers = packetData.Pop();
                }
                else if (character == ',')
                {
                    parseChar = 0;
                    integers.Add(null);
                }
            }

            return integers;
        }

        private static int CompareValues(object leftPacket, object rightPacket)
        {
            if (leftPacket is int && rightPacket is int)
            {
                (int leftInteger, int rightInteger) = ((int)leftPacket, (int)rightPacket);

                if (leftInteger < rightInteger)
                    return 1;

                if (leftInteger > rightInteger)
                    return -1;
            }
            else if (leftPacket is List<object> && rightPacket is List<object>)
            {
                int index = 0;
                (var leftList, var rightList) = (leftPacket as List<object>, rightPacket as List<object>);

                while (index < leftList.Count)
                {
                    if (index >= rightList.Count) return -1;

                    int current = CompareValues(leftList[index], rightList[index]);

                    if (current != 0) return current;

                    index++;
                }

                if (leftList.Count < rightList.Count) return 1;
            }
            else
            {
                object leftItem, rightItem;

                if (leftPacket is int)
                    (leftItem, rightItem) = (new List<object>() { leftPacket }, rightPacket as List<object>);
                else
                    (leftItem, rightItem) = (leftPacket as List<object>, new List<object>() { rightPacket });

                return CompareValues(leftItem, rightItem);
            }

            return 0;
        }
    }
}

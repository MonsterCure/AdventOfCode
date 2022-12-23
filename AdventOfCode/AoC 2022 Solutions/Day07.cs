using System.Diagnostics;

namespace AoC_2022_Solutions
{
    public class Day07 //--- Day 7: No Space Left On Device ---
    {
        internal class FileObj
        {
            public string Name { get; set; }
            public int Size { get; set; }

            public FileObj(string fileName, int fileSize)
            {
                Name = fileName;
                Size = fileSize;
            }
        }

        internal class Directory
        {
            public string Name { get; set; }
            public Directory ParentDirectory { get; set; }
            public int Size { get; set; }
            public List<Directory> ChildDirectories { get; set; }
            public List<FileObj> Files { get; set; }

            public Directory(string directoryName, Directory parentDirectory)
            {
                Name = directoryName;
                ParentDirectory = parentDirectory;
                Size = 0;
                ChildDirectories = new List<Directory>();
                Files = new List<FileObj>();
            }
        }

        internal static int PopulateSizes(Directory directory, ref List<int> directoriesSizes)
        {
            if (directory.Files.Count > 0)
                foreach (var file in directory.Files)
                    directory.Size += file.Size;
            if (directory.ChildDirectories.Count > 0)
                foreach (var childDir in directory.ChildDirectories)
                    directory.Size += PopulateSizes(childDir, ref directoriesSizes);

            directoriesSizes.Add(directory.Size);

            return directory.Size;
        }
        public static void Part01and02()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var input = File.ReadAllLines(@"..\..\..\..\AoC 2022 Inputs\Day07.txt");

            Directory outermostDirectory = new Directory("/", null);
            List<int> directoriesSizes = new List<int>();
            Directory currentDir = null;

            for (int i = 0; i < input.Length; i++)
            {
                string directoryName = "";

                if (input[i].StartsWith("$ cd"))
                {
                    directoryName = input[i].Split(' ').ElementAt(2).Trim();

                    switch (directoryName)
                    {
                        case "/":
                            currentDir = outermostDirectory;
                            break;
                        case "..":
                            currentDir = currentDir.ParentDirectory;
                            break;
                        default:
                            currentDir = currentDir.ChildDirectories.Find(dir => dir.Name == directoryName);
                            break;
                    }
                }
                else if (input[i] == "$ ls")
                {
                    for (i = i + 1; i < input.Count() && !input[i].StartsWith("$"); i++)
                    {
                        if (input[i].StartsWith("dir"))
                        {
                            directoryName = input[i].Split(' ').ElementAt(1).Trim();
                            bool containsDir = currentDir.ChildDirectories.Contains(currentDir.ChildDirectories.Find(dir => dir.Name == directoryName));
                            if (!containsDir)
                                currentDir.ChildDirectories.Add(new Directory(directoryName, currentDir));
                        }
                        else
                        {
                            string fileName = input[i].Split(' ').ElementAt(1).Trim();
                            int fileSize = int.Parse(input[i].Split(' ').ElementAt(0));
                            currentDir.Files.Add(new FileObj(fileName, fileSize));
                        }
                    }

                    i--;
                }
            }

            PopulateSizes(outermostDirectory, ref directoriesSizes);

            int dirSizesBelow100000 = directoriesSizes.Where(size => size <= 100000).Sum();
            Console.WriteLine($"The total size of the directories with sizes below 100000 is {dirSizesBelow100000}.\n");

            int freeSpace = 70000000 - outermostDirectory.Size;
            int neededSpace = 30000000 - freeSpace;
            int smallestDir = directoriesSizes.Where(size => size >= neededSpace).Min();
            Console.WriteLine($"The smallest directory to be deleted for the update is {smallestDir}.\n");

            sw.Stop();
            Console.WriteLine($"Time elapsed: {sw.Elapsed.Milliseconds}ms.\n\n");
            Console.ReadKey();
        }
    }
}

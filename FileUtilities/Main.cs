
namespace FileUtilities
{
    public class Main(FileGenerator generator, FileSorter sorter)
    {
        public async Task Run(string[] args)
        {
            if (args.Length == 0)
            {
                PrintHelp();
                return;
            }

            if (args.Length > 0)
            {
                var argument = args.FirstOrDefault() ?? "";
                if (argument.StartsWith("--"))
                {
                    HandleSwitch(argument);
                }
                else 
                {
                    HandleOperation(args);
                }
            }
        }

        private void HandleOperation(string[] arguments)
        {
            var argumentsQueue = new Queue<string>(arguments);
            argumentsQueue.TryDequeue(out var operation);
            switch (operation)
            {
                case "generate":
                {
                    argumentsQueue.TryDequeue(out var pathSwitch);
                    if (pathSwitch != "--path")
                    {
                        PrintHelp();
                        break;
                    }

                    argumentsQueue.TryDequeue(out var pathValue);
                    if (string.IsNullOrEmpty(pathValue))
                    {
                        PrintHelp();
                        break;
                    }

                    RunGenerate(pathValue);
                    break;
                }

                case "sort":
                    {
                        argumentsQueue.TryDequeue(out var pathSwitch);
                        if (pathSwitch != "--path")
                        {
                            PrintHelp();
                            break;
                        }

                        argumentsQueue.TryDequeue(out var pathValue);
                        if (string.IsNullOrEmpty(pathValue))
                        {
                            PrintHelp();
                            break;
                        }

                        RunSort(pathValue);
                        break;
                    }

                case "combined":
                {
                    argumentsQueue.TryDequeue(out var pathSwitch);
                    if (pathSwitch != "--path")
                    {
                        PrintHelp();
                        break;
                    }

                    argumentsQueue.TryDequeue(out var pathValue);
                    if (string.IsNullOrEmpty(pathValue))
                    {
                        PrintHelp();
                        break;
                    }
                    RunGenerate(pathValue);
                    RunSort(pathValue);
                    break;
                }

                default:
                    {
                        PrintHelp();
                        break;
                    }
            }
        }

        private void RunGenerate(string pathValue)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var fileName = generator.Generate(pathValue);
            watch.Stop();
            Console.WriteLine($"File \"{fileName}\" generated in: {watch.ElapsedMilliseconds / 1000} seconds");
        }

        private void RunSort(string pathValue)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var outputFileName = sorter.Sort(pathValue);
            watch.Stop();
            Console.WriteLine($"File \"{outputFileName}\" sorted in: {watch.ElapsedMilliseconds / 1000} seconds");
        }

        private void HandleSwitch(string argument)
        {
            switch(argument)
            {
                case "--help":
                    {
                        PrintHelp();
                        break;
                    }
            }
        }

        static void PrintHelp()
        {
            Console.WriteLine("Usage: FileUtilities.exe operation --path <filename>");
            Console.WriteLine($"{Environment.NewLine}Operations:");
            Console.WriteLine("  generate     Generates a file with a size of 1 gigabyte (1GB) filled with random data. Use --path switch to provide input file.");
            Console.WriteLine("  sort         Sort file using external merge sort algorithm. Will create file with '_sorted' suffix. Use --path switch to provide input file.");
            Console.WriteLine("  combined     Generates a file and then sort it.");
            Console.WriteLine($"{Environment.NewLine}Example:");
            Console.WriteLine("  ./FileUtilities.exe combined --path \"./data.txt\"");
            Console.WriteLine($"{Environment.NewLine}Switches:");
            Console.WriteLine("  --help     Display this help message");
        }
    }
}

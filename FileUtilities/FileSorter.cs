using System.IO.MemoryMappedFiles;
using System.Text;

namespace FileUtilities
{
    public class FileSorter
    {
        private const long ChunkSizeInBytes = 104857600;

        public string Sort(string inputFilePath)
        {
            var chunks = SplitAndSort(inputFilePath, ChunkSizeInBytes);
            var file = new FileInfo(inputFilePath);
            var outputFileName = $"{file.Name.Replace(file.Extension, "")}_sorted{file.Extension}";
            MergeSortedChunks(chunks, outputFileName);
            return outputFileName;
        }

        private List<string> SplitAndSort(string inputFileName, long chunkSize)
        {
            var chunkFiles = new List<string>();
            var chunkNumber = 0;

            using var mmf = MemoryMappedFile.CreateFromFile(inputFileName, FileMode.Open);
            using var accessor = mmf.CreateViewAccessor();

            var fileSize = new FileInfo(inputFileName).Length;
            long position = 0;
            while (position < fileSize)
            {
                var remainingBytes = Math.Min(chunkSize, fileSize - position);
                var buffer = new byte[remainingBytes];
                accessor.ReadArray(position, buffer, 0, buffer.Length);

                var sourceLines = Encoding.ASCII.GetString(buffer)
                    .Split("\r\n", StringSplitOptions.RemoveEmptyEntries)
                    .Where(p => !string.IsNullOrEmpty(p))
                    .ToList();

                // Handle incomplete line
                var lastItem = sourceLines.Last();
                if (lastItem.Length > 0 && remainingBytes != fileSize - position)
                {
                    sourceLines.RemoveAt(sourceLines.Count() - 1);
                    remainingBytes -= Encoding.ASCII.GetByteCount(lastItem);
                }

                var lines = sourceLines
                    .Select(p => new Line(p))
                    .ToList();

                lines.Sort();

                Directory.CreateDirectory("./temp");

                var chunkFile = $"./temp/chunk_{chunkNumber}.txt";
                File.WriteAllLines(chunkFile, lines.Select(p => p.ToString() ?? ""));
                chunkFiles.Add(chunkFile);

                position += remainingBytes;
                chunkNumber++;
            }

            return chunkFiles;
        }

        static void MergeSortedChunks(List<string> tempFiles, string outputFile)
        {
            using var writer = new StreamWriter(outputFile);
            var readers = tempFiles.Select(p => new StreamReader(p));
            var chunks = readers.Select((f, i) => new Chunk(i, f)).ToList();
            var heap = new PriorityQueue<Chunk, Line>();

            foreach (var chunk in chunks)
            {
                var line = chunk.Next();
                if (!line.IsEmpty())
                {
                    heap.Enqueue(chunk, line);
                }
            }

            while (heap.Count > 0)
            {
                var chunk = heap.Dequeue();
                writer.WriteLine(chunk.CurrentLine);

                var nextLine = chunk.Next();
                if (!nextLine.IsEmpty())
                {
                    heap.Enqueue(chunk, nextLine);
                }
            }

            foreach (var reader in readers)
            {
                reader.Dispose();
            }
        }
    }
}

using System.Text;

namespace FileUtilities
{
    public class FileGenerator
    {
        private readonly string _delimiter = ". ";
        private readonly int _maxWordCountInLine = 100;
        private readonly int _maxWordLength = 100;
        private readonly long _sizeInBytes = 1073741824;

        public string Generate(string? outputFilePath)
        {
            outputFilePath ??= $"./generated-{DateTime.Now:s}.txt";
            var fileInfo = new FileInfo(outputFilePath);
            using var sw = new StreamWriter(outputFilePath);
            var currentSize = 0;
            while (currentSize < _sizeInBytes)
            {
                var random = new Random();

                var number = random.Next(int.MaxValue);
                var @string = GetRandomWords(_maxWordCountInLine, _maxWordLength);

                var line = number + _delimiter + @string;
                currentSize += Encoding.UTF8.GetByteCount(line);
                sw.WriteLine(line);
            }

            return fileInfo.Name;
        }

        static string GetRandomStringWithRandomLength(int maxLength)
        {
            var random = new Random();
            var stringLength = random.Next(1, maxLength);
            return GetRandomString(stringLength);
        }

        static string GetRandomWords(int maxWordCount, int maxWordLength)
        {
            var random = new Random();
            var wordsCount = random.Next(1, maxWordCount);
            var words = Enumerable
                .Range(0, wordsCount)
                .Select(p => GetRandomStringWithRandomLength(maxWordLength));
            return string.Join(" ", words);
        }

        static string GetRandomString(int stringLength)
        {
            var random = new Random();
            var allCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var randomCharacters = Enumerable
                .Range(0, stringLength)
                .Select(p => allCharacters[random.Next(allCharacters.Length)])
                .ToArray();
            return new string(randomCharacters);
        }
    }
}

namespace FileUtilities
{
    public class Chunk
    {
        public int Id { get; }
        public StreamReader StreamReader { get; }
        public Line CurrentLine { get; private set; }

        public Chunk(int id, StreamReader streamReader)
        {
            Id = id;
            StreamReader = streamReader;
        }

        public Line Next()
        {
            var lineValue = StreamReader.ReadLine();
            CurrentLine = new Line(lineValue);
            return CurrentLine;
        }
    }
}

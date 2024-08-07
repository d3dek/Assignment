namespace FileUtilities
{
    public class Line : IComparable<Line>
    {
        private readonly string? _value;

        protected int Number { get; }
        protected string String { get; }

        public Line(string? value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var parts = value.Split(". ");
                Number = int.Parse(parts[0]);
                String = parts[1];
                _value = value;
            }
        }

        public int CompareTo(Line? other)
        {
            if(other == null) return 1;

            var stringEquality = String.CompareTo(other.String);
            return stringEquality == 0 ? Number.CompareTo(other.Number) : stringEquality;
        }

        public override string? ToString()
        {
            return _value;
        }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(_value);
        }
    }
}

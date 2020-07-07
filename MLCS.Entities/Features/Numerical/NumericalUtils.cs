namespace MLCS.Entities.Features.Numerical
{
    public class NumericalUtils
    {
        private const string NUMERICAL_ATTRIBUTE_SEPARATOR = "..";

        public static (int from, int to) GetIntegerInterval(string line)
        {
            int from = int.MinValue;
            int to = int.MaxValue;

            if (!string.IsNullOrWhiteSpace(line) && line.Contains(NUMERICAL_ATTRIBUTE_SEPARATOR))
            {
                int posSeparator = line.IndexOf(NUMERICAL_ATTRIBUTE_SEPARATOR);
                string fromString = line.Substring(0, posSeparator).Trim();
                string toString = line.Substring(posSeparator + NUMERICAL_ATTRIBUTE_SEPARATOR.Length).Trim();

                bool isIntFrom = int.TryParse(fromString, out from);
                bool isIntTo = int.TryParse(toString, out to);

                if (!isIntFrom)
                    from = int.MinValue;
                if (!isIntTo)
                    to = int.MaxValue;
            }

            return (from, to);
        }

        public static (double from, double to) GetDoubleInterval(string line)
        {
            double from = double.MinValue;
            double to = double.MaxValue;

            if (!string.IsNullOrWhiteSpace(line) && line.Contains(NUMERICAL_ATTRIBUTE_SEPARATOR))
            {
                int posSeparator = line.IndexOf(NUMERICAL_ATTRIBUTE_SEPARATOR);
                string fromString = line.Substring(0, posSeparator).Trim();
                string toString = line.Substring(posSeparator + NUMERICAL_ATTRIBUTE_SEPARATOR.Length).Trim();

                bool isDoubleFrom = double.TryParse(fromString, out from);
                bool isDoubleTo = double.TryParse(toString, out to);

                if (!isDoubleFrom)
                    from = double.MinValue;
                if (!isDoubleTo)
                    to = double.MaxValue;
            }

            return (from, to);
        }
    }
}
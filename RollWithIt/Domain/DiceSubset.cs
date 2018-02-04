namespace RollWithIt.Controllers
{
    public class DiceSubset
    {
        public DiceSubset(string type, string value)
        {
        }

        private string Type { get; set; }
        private int Value { get; set; }

        public override string ToString()
        {
            return string.Empty;
        }

        private static class SubsetType
        {
            public const string RemoveHighest = "rh";
            public const string RemoveLowest = "rh";
            public const string KeepHighest = "rh";
            public const string KeepLowest = "rh";
        }
    }
}
namespace RollWithIt.Domain
{
    public class DiceRoll
    {
        public DiceRoll(int value)
        {
            Value = value;
        }

        public int Value { get; }

        public bool Dropped { get; set; }
    }
}
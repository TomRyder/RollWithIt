using System;
using System.Text.RegularExpressions;

namespace RollWithIt.Controllers
{
    public class Roll
    {
        public static Roll Create(string rollString)
        {
            string pattern = @"(?<rollOperator>-|\+)?(?:(?<numberOfDice>\d+)(?:d)(?<numberOfSides>\d+)(?:(?<modifierOperator>-|\+)(?<modifier>\d+(?!d(?!l|h))))?(?<diceSubset>(?:(?:dl|dh|kl|kh|rl|rh)(?:\d+))*))+";
            Regex.Matches(rollString, pattern);
            return new Roll();
        }

        public int NumberOfDice { get; set; }
        public int NumberOfSides { get; set; }
        public int Modifier { get; set; }
        public DiceSubset DiceSubset { get; set; }

        public string FormattedModifier
        {
            get
            {
                if (Modifier == 0)
                {
                    return string.Empty;
                }
                string modifierOperator = Modifier > 0 ? "+" : "-";
                return $"{modifierOperator} {Math.Abs((int) Modifier)}";
            }
        }

        public override string ToString()
        {
            return $"Rolling {NumberOfDice} x d{NumberOfSides} {FormattedModifier} {DiceSubset}";
        }
    }
}
using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace RollWithIt.Controllers
{
    [Route("api/[controller]")]
    public class RollController : Controller
    {
        [HttpGet("{rollString}")]
        public string Get(string rollString)
        {
            Roll roll = Roll.Create(rollString);
            return roll.ToString();
        }
    }

    public class Roll
    {
        public static Roll Create(string rollString)
        {
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
                return $"{modifierOperator} {Math.Abs(Modifier)}";
            }
        }

        public override string ToString()
        {
            return $"Rolling {NumberOfDice} x d{NumberOfSides} {FormattedModifier} {DiceSubset}";
        }
    }

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

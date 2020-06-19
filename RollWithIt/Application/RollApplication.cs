using System;
using System.Text.RegularExpressions;
using RollWithIt.Domain;

namespace RollWithIt.Application
{
    public class RollApplication
    {
        private const string Pattern = @"(-|\+)?(?:(\d+)(?:d)(\d+)(?:(-|\+\d+(?!d(?!l|h))))?((?:(?:dl|dh|kl|kh|rl|rh)(?:\d+))*))+";

        public Roll GetRoll(string rollString)
        {
            Match match = Regex.Match(rollString, Pattern);


            if (!int.TryParse(match.Groups[2].Value, out int numberOfDice))
            {
                throw new Exception("Number of Dice Invalid");
            }

            if (!int.TryParse(match.Groups[3].Value, out int numberOfSides))
            {
                throw new Exception("Number of Sides Invalid");
            }

            int? modifier = null;
            if (int.TryParse(match.Groups[4].Value, out int parsedModifier))
            {
                modifier = parsedModifier;
            }

            return Roll.Create(
                numberOfDice, 
                numberOfSides,
                modifier, 
                match.Groups[5].Value);
        }

        //        public IEnumerable<Roll> GetRolls(string rollString)
        //        {
        //
        //            MatchCollection matchCollection = Regex.Matches(rollString, Pattern);
        //            
        //            foreach (Match match in matchCollection)
        //            {
        //                if (!int.TryParse(match.Groups[2].Value, out int numberOfDice))
        //                {
        //                    throw new Exception("Number of Dice Invalid");
        //                }
        //
        //                if (!int.TryParse(match.Groups[3].Value, out int numberOfSides))
        //                {
        //                    throw new Exception("Number of Sides Invalid");
        //                }
        //
        //                if (!int.TryParse(match.Groups[4].Value, out int modifier))
        //                {
        //                    modifier = 0;
        //                }
        //
        //                rollResult.AddRoll(Roll.Create(match.Groups[1].Value, numberOfDice, numberOfSides, match.Groups[4].Value, modifier, DiceSubset.Create(match.Groups[6].Value)));
        //            }
        //
        //            return new RollResult();
        //        }
    }
}
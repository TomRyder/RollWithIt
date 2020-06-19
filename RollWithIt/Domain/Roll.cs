using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace RollWithIt.Domain
{
    public sealed class Roll
    {
        private static readonly Random Random = new Random();

        private Roll(IEnumerable<int> rolls, int maxSingleRoll, int? modifier, string subsetType, int? subsetAmount)
        {
            Rolls = ImmutableList.CreateRange(rolls.Select(r => new DiceRoll(r)));
            MaxSingleRoll = maxSingleRoll;
            Modifier = modifier;

            SubsetAmount = subsetAmount;
            SubsetType = subsetType;

            if (!subsetAmount.HasValue) return;

            switch (subsetType)
            {
                case "dl":
                    foreach (DiceRoll roll in Rolls.OrderBy(i => i.Value).Take(subsetAmount.Value))
                    {
                        roll.Dropped = true;
                    }
                    break;

                case "dh":
                    foreach (DiceRoll roll in Rolls.OrderByDescending(i => i.Value).Take(subsetAmount.Value))
                    {
                        roll.Dropped = true;
                    }
                    break;

                case "kl":
                    foreach (DiceRoll roll in Rolls.OrderByDescending(i => i.Value).Take(Rolls.Count - subsetAmount.Value))
                    {
                        roll.Dropped = true;
                    }
                    break;

                case "kh":
                    foreach (DiceRoll roll in Rolls.OrderBy(i => i.Value).Take(Rolls.Count - subsetAmount.Value))
                    {
                        roll.Dropped = true;
                    }
                    break;
            }
        }

        public ImmutableList<DiceRoll> Rolls { get; }

        public int MaxSingleRoll { get; }

        public int? Modifier { get; }
        
        public int NumberOfRolls => Rolls.Count;

        public int RollsTotal => Rolls.Sum(r => r.Value);

        public int KeptRollsTotal => Rolls.Where(r => !r.Dropped).Sum(r => r.Value);

        public int Total => Modifier.HasValue ? KeptRollsTotal + Modifier.Value : KeptRollsTotal;

        public int? SubsetAmount { get; set; }

        public string SubsetType { get; set; }

        public static Roll Create(
            int numberOfRolls, 
            int maxSingleRoll, 
            int? modifier = null, 
            string diceSubset = null)
        {
            if (maxSingleRoll < 1)
            {
                throw new InvalidMaxRollException();
            }

            List<int> rolls = new List<int>();
            for (int rollCount = 0; rollCount < numberOfRolls; rollCount++)
            {
                rolls.Add(Random.Next(maxSingleRoll) + 1);
            }
             
            if (string.IsNullOrWhiteSpace(diceSubset))
            {
                return new Roll(rolls, maxSingleRoll, modifier, null, null);
            }

            return new Roll(
                rolls, 
                maxSingleRoll, 
                modifier, 
                diceSubset.Substring(0, 2).ToLowerInvariant(), 
                int.Parse(diceSubset.Substring(2)));
        }

        public static Roll Create(int numberOfRolls, int maxSingleRoll, string subset)
        {
            return Create(numberOfRolls, maxSingleRoll, null, subset);
        }
    }

    public abstract class Possibility
    {
        public Guid Id { get; set; }

        public string Value { get; set; }

        public string Discriminator { get; set; }

        public int Weighting { get; set; }
    }
}
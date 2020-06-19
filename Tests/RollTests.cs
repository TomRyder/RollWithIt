using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using RollWithIt.Domain;

namespace Tests
{
    [TestFixture]
    public class RollTests
    {
        private const int Runs = 100;
        private const int MaxValue = 101;
        private const int ModifierMin = -10;
        private const int ModifierMax = 10;

        [Test]
        public void Create_10d20_KeepHighest2_2Kept8Dropped()
        {
            Roll roll = Roll.Create(10, 20, "kh2");

            Assert.That(roll.Rolls.Count, Is.EqualTo(10));
            Assert.That(roll.Rolls.Count(r => !r.Dropped), Is.EqualTo(2));
            Assert.That(roll.Rolls.Count(r => r.Dropped), Is.EqualTo(8));

            foreach (DiceRoll droppedRoll in roll.Rolls.Where(r => r.Dropped))
            {
                Assert.That(roll.Rolls.Where(r => !r.Dropped).All(r => r.Value >= droppedRoll.Value));
            }
        }

        [Test]
        public void Create_8d20_DropHighest5_3Kept5Dropped()
        {
            Roll roll = Roll.Create(8, 20, "dh5");

            Assert.That(roll.Rolls.Count, Is.EqualTo(8));
            Assert.That(roll.Rolls.Count(r => !r.Dropped), Is.EqualTo(3));
            Assert.That(roll.Rolls.Count(r => r.Dropped), Is.EqualTo(5));

            foreach (DiceRoll droppedRoll in roll.Rolls.Where(r => r.Dropped))
            {
                Assert.That(roll.Rolls.Where(r => !r.Dropped).All(r => r.Value <= droppedRoll.Value));
            }
        }

        [Test]
        public void Create_10d100_KeptLowest3_3Kept7Dropped()
        {
            Roll roll = Roll.Create(10, 100, "kl3");

            Assert.That(roll.Rolls.Count, Is.EqualTo(10));
            Assert.That(roll.Rolls.Count(r => !r.Dropped), Is.EqualTo(3));
            Assert.That(roll.Rolls.Count(r => r.Dropped), Is.EqualTo(7));

            foreach (DiceRoll droppedRoll in roll.Rolls.Where(r => r.Dropped))
            {
                Assert.That(roll.Rolls.Where(r => !r.Dropped).All(r => r.Value <= droppedRoll.Value));
            }
        }

        [Test]
        public void Create_10d100_DropLowest1_1Kept9Dropped()
        {
            Roll roll = Roll.Create(10, 100, "dl1");

            Assert.That(roll.Rolls.Count, Is.EqualTo(10));
            Assert.That(roll.Rolls.Count(r => !r.Dropped), Is.EqualTo(9));
            Assert.That(roll.Rolls.Count(r => r.Dropped), Is.EqualTo(1));

            foreach (DiceRoll droppedRoll in roll.Rolls.Where(r => r.Dropped))
            {
                 Assert.That(roll.Rolls.Where(r => !r.Dropped).All(r => r.Value >= droppedRoll.Value));
            }
        }

        [Test]
        public void Roll_Rolls_CannotRemoveRollsOnceCreated()
        {
            Roll roll = Roll.Create(5, 1, 1);
            roll.Rolls.RemoveAll(i => true);

            Assert.That(roll.Rolls.Count, Is.EqualTo(5));
        }

        [Test]
        public void Roll_Rolls_CannotAddRollsOnceCreated()
        {
            Roll roll = Roll.Create(5, 1, 1);
            roll.Rolls.Add(new DiceRoll(1));

            Assert.That(roll.Rolls.Count, Is.EqualTo(5));
        }

        [TestCase(1, 1, 1)]
        [TestCase(10, 10, 10)]
        [TestCase(10, 10, -10)]
        [TestCase(100, 100, 1000)]
        public void Create_XdY_ModifierZ_ReturnsValidResults(int numberOfRolls, int maxValue, int modifier)
        {
            Roll roll = Roll.Create(numberOfRolls, maxValue, modifier);

            Assert.That(roll.MaxSingleRoll, Is.EqualTo(maxValue));
            Assert.That(roll.NumberOfRolls, Is.EqualTo(numberOfRolls));
            Assert.That(roll.Modifier, Is.EqualTo(modifier));

            int expectedRollTotal = roll.Rolls.Sum(r => r.Value);
            Assert.That(roll.RollsTotal, Is.EqualTo(expectedRollTotal));
            Assert.That(roll.Total, Is.EqualTo(expectedRollTotal + modifier));
        }

        [Test]
        public void Create_Xd1_ModifierY_ReturnsRollTotalXPlusY()
        {
            for (int numberOfRolls = 0; numberOfRolls < Runs; numberOfRolls++)
            {
                for (int modifier = ModifierMin; modifier < ModifierMax; modifier++)
                {
                    Roll roll = Roll.Create(numberOfRolls, 1, modifier);

                    Assert.That(roll.MaxSingleRoll, Is.EqualTo(1));
                    Assert.That(roll.RollsTotal, Is.EqualTo(numberOfRolls));
                    Assert.That(roll.NumberOfRolls, Is.EqualTo(numberOfRolls));
                    Assert.That(roll.Modifier, Is.EqualTo(modifier));
                    Assert.That(roll.Total, Is.EqualTo(numberOfRolls + modifier));
                }
            }
        }

        [Test]
        public void Create_Xd1_ReturnsRollTotalX()
        {
            for (int numberOfRolls = 0; numberOfRolls < Runs; numberOfRolls++)
            {
                Roll roll = Roll.Create(numberOfRolls, 1);

                Assert.That(roll.MaxSingleRoll, Is.EqualTo(1));
                Assert.That(roll.RollsTotal, Is.EqualTo(numberOfRolls));
                Assert.That(roll.NumberOfRolls, Is.EqualTo(numberOfRolls));
            }
        }

        [Test]
        public void Create_1d1_ModifierX_ReturnsRollTotal1PlusX()
        {
            for (int modifier = ModifierMin; modifier < ModifierMax; modifier++)
            {
                Roll roll = Roll.Create(1, 1, modifier);

                Assert.That(roll.Rolls.Single().Value, Is.EqualTo(1));
                Assert.That(roll.Modifier, Is.EqualTo(modifier));
                Assert.That(roll.Total, Is.EqualTo(1 + modifier));
            }
        }

        [Test]
        public void Create_1d0_ThrowsException()
        {
            Assert.Throws<InvalidMaxRollException>(() => Roll.Create(1, 0));
        }

        [Test]
        public void Create_1dX_AllResultsBetween1AndX()
        {
            List<Roll> rolls = new List<Roll>();

            for (int i = 0; i < Runs * MaxValue; i++)
            {
                rolls.Add(Roll.Create(1, MaxValue));
            }

            for (int potentialValue = 1; potentialValue <= MaxValue; potentialValue++)
            {
                Assert.That(rolls.Any(roll => roll.Rolls.Single().Value == potentialValue && roll.MaxSingleRoll == MaxValue));
            }

            Assert.That(rolls.All(diceRoll => diceRoll.Rolls.Single().Value >= 1));
            Assert.That(rolls.All(diceRoll => diceRoll.Rolls.Single().Value <= MaxValue));
        }

        [Test]
        public void Create_1d1_DiceRoll1()
        {
            Roll roll = Roll.Create(1, 1);

            Assert.That(roll.Rolls.Single().Value, Is.EqualTo(1));
            Assert.That(roll.MaxSingleRoll, Is.EqualTo(1));
        }

        //        Can't test as this is a compile time error.
        //        [Test]
        //        public void Roll_Rolls_CannotSetRollsOnceCreated()
        //        {
        //            Roll roll = Roll.Create(5, 1, 1);
        //            roll.Rolls = roll.Rolls.Add(1);
        //            Assert.That(roll.Rolls.Count, Is.EqualTo(5));
        //        }
    }
}

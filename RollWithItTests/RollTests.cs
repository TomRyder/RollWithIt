using NUnit.Framework;
using RollWithIt.Domain;

namespace RollWithItTests
{
    [TestFixture]
    public class RollWithItTests
    {
        [TestCase(1, 1)]
        [TestCase(2, 1)]
        [TestCase(100, 1)]
        [TestCase(1, 5)]
        [TestCase(2, 10)]
        [TestCase(100, 80)]
        public void Create_ValidDiceAndSides_ReturnsRoll(string numberOfDice, string numberOfSides)
        {
            Roll roll = Roll.Create($"{numberOfDice}d{numberOfSides}");
            Assert.That(roll.NumberOfDice, Is.EqualTo(numberOfDice));
            Assert.That(roll.NumberOfSides, Is.EqualTo(numberOfSides));
        }
    }
}

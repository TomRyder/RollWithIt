using NUnit.Framework;
using RollWithIt.Domain;

namespace Tests
{
    [TestFixture]
    public class RollTests
    {
        [Test]
        public void Crete_ValidRollString_CreatesCorrectRoll()
        {
            Roll roll = Roll.Create("1d2+3");
            Assert.That(roll.NumberOfDice, Is.EqualTo(1));
            Assert.That(roll.NumberOfSides, Is.EqualTo(2));
            Assert.That(roll.Modifier, Is.EqualTo(3));
        }
    }
}

using FluentAssertions;
using System;
using Xunit;

namespace Game.Model.xUnitTesting
{
    public class DeveloperxUnitTesting
    {
        [Fact] 
        public void TwoDevelopersAreEqualIfTheirFullnamesAndBirthdaysAreEqual()
        {
            var dev1 = new Developer("Peter Graham", new DateTime(1985, 12, 5), 10000, 3000);
            var dev2 = new Developer("Peter Graham", new DateTime(1985, 12, 5), 10000, 3000);
            var dev3 = new Developer("Peter Grahams", new DateTime(1985, 12, 5), 10000, 3000);
            var dev4 = new Developer("Peter Graham", new DateTime(1985, 12, 4), 10000, 3000);

            Assert.True(dev1.Equals(dev2));
            Assert.False(dev1.Equals(dev3));
            Assert.False(dev1.Equals(dev4));
        }

        [Fact]
        public void WhenDeveleoperIsCreated_IsLeavingStatusIsFalseAndFireDateIsNull()
        {
            var dev = new Developer("Niko Ko", new DateTime(1985, 12, 5), 10000, 3000);
            
            dev.IsLeaving.Should().Be(false);
            dev.FireDate.Should().BeNull();
        }

        [Fact]
        public void WhenDeveleoperIsResigning_IsLeavingStatusIsTrueAndFireDateIsNotNull()
        {
            // arrange
            var dev = new Developer("Niko Ko", new DateTime(1985, 12, 5), 10000, 3000);
            
            // act
            dev.Resign(DateTime.Now);

            // assert
            dev.IsLeaving.Should().Be(true);
            dev.FireDate.Should().NotBeNull();
        }

        [Fact]
        public void ResigningFirstTime_ReturnsTrue_ResigningSecondTime_ReturnsFalse()
        {
            // arrange
            var dev = new Developer("Niko Ko", new DateTime(1985, 12, 5), 10000, 3000);
            
            // act
            bool result1 = dev.Resign(DateTime.Now);
            bool result2 = dev.Resign(DateTime.Now);

            // assert
            result1.Should().Be(true);
            result2.Should().Be(false);
        }
    }
}

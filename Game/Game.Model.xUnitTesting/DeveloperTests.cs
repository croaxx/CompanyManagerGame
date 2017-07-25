using FluentAssertions;
using System;
using Xunit;

namespace Game.Model.xUnitTesting
{
    public class DeveloperTests
    {
        private IDeveloper testee;

        [Theory]
        [InlineData("Nikolay Komarevskiy", 1986, 10, 2, true)]
        [InlineData("Nikolay Komarevsky", 1986, 10, 2, false)]
        [InlineData("Nikolay Komarevskiy", 1986, 10, 3, false)]
        public void DevelopersAreEqual_If_TheirFullnamesAndBirthdaysAreEqual(string name,
                                                                             int birthYear,
                                                                             int birthMonth,
                                                                             int birthDay,
                                                                             bool areEqual)
        {
            testee = new Developer("Nikolay Komarevskiy", new DateTime(1986, 10, 02), 0 , 0);
            var comparedDev = new Developer(name, new DateTime(birthYear, birthMonth, birthDay), 0 , 0);

            bool result = testee.Equals(comparedDev);

            result.Should().Be(areEqual);
        }

        [Fact]
        public void WhenDeveleoperIsCreated_FireDateIsNull()
        {
            Action act = () => {testee = new Developer("Niko Ko", new DateTime(1985, 12, 5), 0, 0);};
            
            act();

            testee.FireDate.Should().BeNull();
        }

        [Fact]
        public void WhenDeveleoperIsResigning_FireDateIsNotNull()
        {
            var dev = new Developer("Niko Ko", new DateTime(1985, 12, 5), 0, 0);
            
            dev.Resign(DateTime.Now);

            dev.FireDate.Should().NotBeNull();
        }

        [Fact]
        public void CallingResignFirstTime_ReturnsTrue_CallingSecondTime_ReturnsFalse()
        {
            var dev = new Developer("Niko Ko", new DateTime(1985, 12, 5), 0, 0);
            bool resignFirstCall;
            bool resignSecondCall;;

            resignFirstCall = dev.Resign(DateTime.Now);
            resignSecondCall = dev.Resign(DateTime.Now);

            resignFirstCall.Should().Be(true);
            resignSecondCall.Should().Be(false);
        }
    }
}

using FluentAssertions;
using System;
using Xunit;

namespace Game.Model.xUnitTesting
{
    public class ExtensionMethodsxUnitTesting
    {
        [Fact]
        public void IncrementMonths_Test()
        {
            //arrange
            var time = new DateTime(2011, 5, 2);

            //act
            var newTime = time.IncrementMonths(2);
            var expectedNewTime = new DateTime(2011, 7, 2);

            // assert
            newTime.Should().Be(expectedNewTime);
        }

        [Fact]
        public void IncrementMonthsYearSwitch_Test()
        {
            //arrange
            var time = new DateTime(2011, 8, 2);

            //act
            var newTime = time.IncrementMonths(6);
            var expectedNewTime = new DateTime(2012, 2, 2);

            // assert
            newTime.Should().Be(expectedNewTime);
        }
    }
}

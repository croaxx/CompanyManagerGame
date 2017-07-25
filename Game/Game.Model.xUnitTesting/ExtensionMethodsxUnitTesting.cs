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
            var time = new DateTime(2011, 5, 2);
            var expectedNewTime = new DateTime(2011, 7, 2);

            var newTime = time.TryIncrementMonths(2);

            newTime.Should().Be(expectedNewTime);
        }

        [Fact]
        public void IncrementMonthsYearSwitch_Test()
        {
            var time = new DateTime(2011, 8, 2);
            var expectedNewTime = new DateTime(2012, 2, 2);

            var newTime = time.TryIncrementMonths(6);

            newTime.Should().Be(expectedNewTime);
        }
    }
}

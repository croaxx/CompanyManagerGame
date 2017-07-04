using FluentAssertions;
using Game.Model;
using System;
using Xunit;

namespace Game.ModelTests
{
    public class ProjectTest
    {
        private Project testee;

        [Fact]
        public void WhenExpiryDateIsBeforeCurrentDate_ThenProjectIsExpired()
        {
            var expiryTime = new DateTime(2017, 10, 2);
            var currentTime = new DateTime(2017, 10, 3);

            this.testee = new Project(expiryTime);

            this.testee.IsExpired(currentTime).Should().Be(true);
        }

        [Fact]
        public void WhenExpiryDateExactlyEqualsCurrentDate_ThenProjectIsNotExpired()
        {
            var expiryTime = new DateTime(2017, 10, 2);
            var currentTime = new DateTime(2017, 10, 2);

            this.testee = new Project(expiryTime);

            this.testee.IsExpired(currentTime).Should().Be(false);
        }
    }
}

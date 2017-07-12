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

        [Fact]
        public void ProjectSetStartTimeFailsAfterFirstSet()
        {
            var startTime = new DateTime(2017, 12, 10);
            var expiry = startTime.AddDays(5);
            this.testee = new Project(expiry);

            bool result = this.testee.TrySetStartTime(startTime);

            result.Should().Be(true);

            result = this.testee.TrySetStartTime(expiry.AddDays(10));

            result.Should().Be(false);
        }

        [Fact]
        public void ProjectSetStartTimeFails_ReturnsFalse_IfStartTimeExceedsExpiry()
        {
            // set
            var expiryTime = new DateTime(2017, 10, 5); 
            var startTime = expiryTime.AddMilliseconds(1);
            this.testee = new Project(expiryTime);

            // act
            bool result = this.testee.TrySetStartTime(startTime);
            
            //assert
            result.Should().Be(false);
        }
    }
}

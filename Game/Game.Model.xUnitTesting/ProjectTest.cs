using FluentAssertions;
using Game.Model;
using System;
using Xunit;

namespace Game.ModelTests
{
    public class ProjectTest
    {
        private IProject testee;

        [Fact]
        public void WhenExpiryDateIsBeforeCurrentDate_ThenProjectIsExpired()
        {
            var expiryTime = new DateTime(2017, 10, 2);
            var currentTime = new DateTime(2017, 10, 3);
            testee = new Project("", expiryTime, 0, 0);

            bool status = testee.IsExpired(currentTime);

            status.Should().Be(true);
        }

        [Fact]
        public void WhenExpiryDateExactlyMatchesCurrentDate_ThenProjectIsNotExpired()
        {
            var expiryTime = new DateTime(2017, 10, 2);
            var currentTime = new DateTime(2017, 10, 2);
            testee = new Project("", expiryTime, 0 , 0);

            bool status = testee.IsExpired(currentTime);

            status.Should().Be(false);
        }

        [Fact]
        public void ProjectSetStartTimeFailsAtSecondSetAttempt()
        {
            var startTime = new DateTime(2017, 12, 10);
            var expiryDate = startTime.AddDays(5);
            testee = new Project("", expiryDate, 0, 0);

            bool resultSet1 = testee.TrySetStartTime(startTime);
            bool resultSet2 = testee.TrySetStartTime(expiryDate.AddDays(10));

            resultSet1.Should().Be(true);
            resultSet2.Should().Be(false);
        }

        [Theory]
        [InlineData(2017, 10, 5, false)]
        [InlineData(2017, 10, 6, false)]
        [InlineData(2017, 10, 4, true)]
        public void ProjectSetStartTime_Returns_Status(int year, int month, int day, bool isTimeSetSucceded)
        {
            var expiryTime = new DateTime(2017, 10, 5); 
            var startTime = new DateTime(year, month, day); 
            testee = new Project("", expiryTime, 0, 0);

            bool timeSetSuccedStatus = testee.TrySetStartTime(startTime);
            
            timeSetSuccedStatus.Should().Be(isTimeSetSucceded);
        }

        [Theory]
        [InlineData(1000, 500, 500, 0, false)]
        [InlineData(2300, 2300, 0, 0, true)]
        [InlineData(3000, 4000, 0, 1000, true)]
        public void DoWorkOnProject_Returns_UnusedWorkAmount_And_Sets_CompletionStatus(long workAmountAssigned,
                                                                                       long workAmountToDo,
                                                                                       long workAmountRemaining,
                                                                                       long unusedWorkExpected,
                                                                                       bool isWorkCompleted)
        {
            testee = new Project("", DateTime.Now, 0, workAmountAssigned);

            var unusedWork = testee.DoWorkOnProject(workAmountToDo);

            testee.WorkAmountRemaining.Should().Be(workAmountRemaining);
            unusedWork.Should().Be(unusedWorkExpected);
            testee.IsWorkCompleted.Should().Be(isWorkCompleted);
        }

    }
}

using System.Collections.Generic;
using Xunit;
using System;
using FluentAssertions;
using System.Linq;
using FakeItEasy;

namespace Game.Model.xUnitTesting
{
    public class CompanyLogicxUnitTesting
    {
        private ICompanyLogic logic;

        private EntityRepository<IDeveloper> GenerateRepositoryOfDevelopers(int devs, 
                                                                           IList<int> salaries, 
                                                                           IList<DateTime> resignTimes,
                                                                           IList<int> productivities)
        {
            var devRepo = new EntityRepository<IDeveloper>();

            for (int i = 0; i < devs; ++i)
            {
                string name = Guid.NewGuid().ToString();
                var birthDate  = DateTime.MinValue;

                int salary = salaries == null ? 0 : salaries[i];
                int productivity = productivities == null ? 0 : productivities[i];

                var dev = new Developer(name, birthDate, salary, productivity);

                if (resignTimes != null)
                    dev.Resign(resignTimes[i]);

                devRepo.TryAdd(dev);
            }

            return devRepo;
        }

        [Theory]
        [InlineData(2019, 3, 14, 0)]
        [InlineData(2018, 12, 5, 1)]
        [InlineData(2017, 2, 7, 2)]
        [InlineData(2017, 2, 6, 3)]
        public void RemoveResignedDevelopers_Test(int currentYear, int currentMonth,
                                                  int currentDay, int expectedNumOfDevsLeft)
        {
            logic = new CompanyLogic();
            var resignTimes = new List<DateTime> { { new DateTime(2017, 2, 7) }, { new DateTime(2018, 12, 5) }, { new DateTime(2019, 3, 14) } };
            var repo = GenerateRepositoryOfDevelopers(resignTimes.Count, null, resignTimes, null);
            var currentTime = new DateTime(currentYear, currentMonth, currentDay);

            logic.RemoveResignedDevelopers(repo, currentTime);

            repo.Count.Should().Be(expectedNumOfDevsLeft);
        }

        [Fact]
        public void PaySalariesAndRemoveUnpaidDevs_ReducesBudgetBySalariesSum_And_DoesNotChangeDevsCount()
        {
            logic = new CompanyLogic();
            int devs = 4;
            var salaries = new List<int> { 15500, 6500, 17500, 7000 };
            var repo = GenerateRepositoryOfDevelopers(devs, salaries, null, null);
            long budget = 50000;
            long expectedRemainBudget = budget - salaries.Sum(x => x);

            logic.PaySalariesAndRemoveUnpaidDevs(repo, ref budget);

            budget.Should().Be(expectedRemainBudget);
            repo.Count.Should().Be(devs); // nobody leaves
        }

        [Fact]
        public void PaySalariesAndRemoveUnpaidDevs_TwoDevelopersLeaveDueToInsufficientBudget()
        {
            logic = new CompanyLogic();
            var salaries = new List<int> { 15500, 6500, 17500, 7000 };
            var repo = GenerateRepositoryOfDevelopers(salaries.Count, salaries, null, null);
            long budget = 13500;
            int expectedDevsAfterPayment = 2;

            logic.PaySalariesAndRemoveUnpaidDevs(repo, ref budget);

            repo.Count.Should().Be(expectedDevsAfterPayment);
        }

        private EntityRepository<IProject> GenerateRepositoryOfProjects(int projCount,
                                                           IList<int> workAmountAssigned,
                                                           IList<DateTime> expiryTimes,
                                                           IList<long> rewards)
        {
            var projRepo = new EntityRepository<IProject>();

            for (int i = 0; i < projCount; ++i)
            {
                string name = Guid.NewGuid().ToString();

                int workAssigned = workAmountAssigned == null ? 0 : workAmountAssigned[i];
                DateTime expiry = expiryTimes == null ? DateTime.MinValue : expiryTimes[i];
                long reward = rewards == null ? 0 : rewards[i];

                var p = new Project(name, expiry, reward, workAssigned);

                projRepo.TryAdd(p);
            }

            return projRepo;
        }

        [Fact]
        public void PerformOneWorkDayOnProjects_Test()
        {
            logic = new CompanyLogic();
            int devsCount = 3;
            var salaries = new List<int> { 15500, 6500, 17500 };
            var productivities = new List<int> { 1053, 45007, 11011 };
            var devsRepo = GenerateRepositoryOfDevelopers(devsCount, salaries, null, productivities);
            int projCount = 3;
            var workAmounts = new List<int> { 3700, 45000, 11000 };
            var projectsRepo = GenerateRepositoryOfProjects(projCount, workAmounts, null, null);
            int productivitySum = productivities.Sum(x => x);
            int expectedRoundOffWorkDone = projectsRepo.Count * (productivitySum / projectsRepo.Count);

            logic.PerformOneWorkDayOnProjects(devsRepo, projectsRepo);

            projectsRepo.Sum(x => x.WorkAmountAssigned - x.WorkAmountRemaining).Should().Be(expectedRoundOffWorkDone);
        }

        [Fact]
        public void PerformOneWorkDayOnProjects_SetsIsDoneStatusOfTwoProjects_ToTrue()
        {
            logic = new CompanyLogic();
            int devsCount = 4;
            var productivities = new List<int> { 2002, 1301, 1205, 4125 };
            var devsRepo = GenerateRepositoryOfDevelopers(devsCount, null, null, productivities);
            int projCount = 4;
            var workAmounts = new List<int> { 1053, 45000, 11000, 1600 };
            var projectsRepo = GenerateRepositoryOfProjects(projCount, workAmounts, null, null);
            int expectedFinishedProjects = 2;

            logic.PerformOneWorkDayOnProjects(devsRepo, projectsRepo); // projects with 1053 and 1600 should be finished

            projectsRepo.Where(x => x.IsWorkCompleted == true).ToList().Count.Should().Be(expectedFinishedProjects);
        }

        [Fact]
        public void RemoveFinishedProjectAndGetTheRestReward_Test()
        {
            var logic = new CompanyLogic();
            var projectsRepo = new EntityRepository<IProject>();
            var project = A.Fake<IProject>();
            A.CallTo(() => project.StartTime).Returns(new DateTime(2011, 5, 13));
            A.CallTo(() => project.ExpiryTime).Returns(new DateTime(2017, 7, 22));
            var currentTime = new DateTime(2015, 5, 28);
            A.CallTo(() => project.IsWorkCompleted).Returns(true);
            long projectReward = 15257;
            A.CallTo(() => project.Reward).Returns(projectReward);
            projectsRepo.TryAdd(project);
            long rewardReceived = logic.GetRewardReceivedFromProjectAtTime(project, currentTime);

            logic.RemoveFinishedProjectAndGetTheRestReward(projectsRepo, currentTime, ref rewardReceived);

            projectsRepo.Count.Should().Be(0);
            rewardReceived.Should().Be(projectReward);
        }
    }
}

using FakeItEasy;
using System.Collections.Generic;
using Xunit;
using System;
using System.Collections.Concurrent;
using FluentAssertions;
using System.Linq;

namespace Game.Model.xUnitTesting
{
    public class CompanyLogicxUnitTesting
    {
        private ConcurrentDictionary<Developer, Developer> GenerateConcurrentDictionaryOfThreeDevelopers(IList<DateTime> resignTimes)
        {
            var d1 = new Developer("Bryan", DateTime.Now, 5500, 100);
            d1.Resign(resignTimes[0]);
            var d2 = new Developer("Thomas", DateTime.Now, 6500, 100);
            d2.Resign(resignTimes[1]);
            var d3 = new Developer("Luke", DateTime.Now, 7300, 100);
            d3.Resign(resignTimes[2]);

            var dict = new ConcurrentDictionary<Developer, Developer>();
            dict.TryAdd(d1, d1);
            dict.TryAdd(d2, d2);
            dict.TryAdd(d3, d3);

            return dict;
        }

        private ConcurrentDictionary<Developer, Developer> GenerateConcurrentDictionaryOfFourDevelopers()
        {
            var d1 = new Developer("Bryan", DateTime.Now, 15500, 100);
            var d2 = new Developer("Thomas", DateTime.Now, 6500, 100);
            var d3 = new Developer("Luke", DateTime.Now, 17500, 100);
            var d4 = new Developer("Robert", DateTime.Now, 7000, 100);

            var dict = new ConcurrentDictionary<Developer, Developer>();
            dict.TryAdd(d1, d1);
            dict.TryAdd(d2, d2);
            dict.TryAdd(d3, d3);
            dict.TryAdd(d4, d4);

            return dict;
        }

        [Fact]
        public void RemoveResignedDevelopers_RemovesTwoOutOfTreeEmployees()
        {
            // arrange
            var logic = new CompanyLogic();
            var resignTimes = new List<DateTime>
            { 
                { new DateTime(2017, 2, 5) },
                { new DateTime(2018, 12, 5) },
                { new DateTime(2019, 3, 5) }
            };
            var dict = GenerateConcurrentDictionaryOfThreeDevelopers(resignTimes);
            int expectedNumberOfDevs = 1;

            // act
            logic.RemoveResignedDevelopers(dict, new DateTime(2018, 12, 7));

            //assert
            dict.Count.Should().Be(expectedNumberOfDevs);
        }

        [Fact]
        public void PaySalariesAndRemoveUnpaidDevs_ReducesTheBudgetBySalariesSum()
        {
            // arrange
            var logic = new CompanyLogic();
            var dict = GenerateConcurrentDictionaryOfFourDevelopers();
            long salarySum = dict.Sum(x => x.Value.MonthlySalary);
            long budget = 50000;
            long expectedRemainBudget = budget - salarySum;
            
            // act
            logic.PaySalariesAndRemoveUnpaidDevs(dict, ref budget);

            // assert
            budget.Should().Be(expectedRemainBudget);
            dict.Count.Should().Be(4); // nobody leaves
        }

        [Fact]
        public void PaySalariesAndRemoveUnpaidDevs_TwoDevelopersLeaveDue()
        {
            // arrange
            var logic = new CompanyLogic();
            var dict = GenerateConcurrentDictionaryOfFourDevelopers();
            long budget = 13500;
            
            // act
            logic.PaySalariesAndRemoveUnpaidDevs(dict, ref budget);

            // assert
            dict.Count.Should().Be(2);
        }

        private ConcurrentDictionary<Developer, Developer> GenerateFourDevelopersWithProductivities(IList<int> productivities)
        {
            var d1 = new Developer("Bryan", DateTime.Now, 15500, productivities[0]);
            var d2 = new Developer("Thomas", DateTime.Now, 6500, productivities[1]);
            var d3 = new Developer("Luke", DateTime.Now, 17500, productivities[2]);
            var d4 = new Developer("Robert", DateTime.Now, 7000, productivities[3]);

            var dict = new ConcurrentDictionary<Developer, Developer>();
            dict.TryAdd(d1, d1);
            dict.TryAdd(d2, d2);
            dict.TryAdd(d3, d3);
            dict.TryAdd(d4, d4);

            return dict;
        }
        
        private ConcurrentDictionary<string, Project> GenerateThreeProjectsWithAssignedWorkAmount(List<int> workAmount)
        {
            var d1 = new Project("Youtube Services", DateTime.MinValue, 1000, workAmount[0]);
            var d2 = new Project("BWM database", DateTime.MinValue, 1000, workAmount[1]);
            var d3 = new Project("Bank security challange", DateTime.MinValue, 1000, workAmount[1]);


            var dict = new ConcurrentDictionary<string, Project>();
            dict.TryAdd(d1.Title, d1);
            dict.TryAdd(d2.Title, d2);
            dict.TryAdd(d3.Title, d3);

            return dict;
        }

        [Fact] public void DoWorkOnTheProjects_Test()
        {
            // arrange
            var developers = GenerateFourDevelopersWithProductivities(new List<int> {2001, 1303, 1200, 4125 });
            var projects = GenerateThreeProjectsWithAssignedWorkAmount(new List<int> {3700, 45000, 11000});
            var logic = new CompanyLogic();
            int productivitySum = developers.Sum(x => x.Value.CodeLinesPerDay);
            
            int expectedRoundOffWorkDone = projects.Count*(productivitySum/projects.Count);
            // act
            logic.PerformOneWorkDayOnProjects(developers, projects);

            // assert
            projects.Sum( x => x.Value.WorkAmountAssigned - x.Value.WorkAmountRest).Should().Be(expectedRoundOffWorkDone);
        }

        [Fact] public void DoWorkOnTheProjects_SetsIsDoneStatusOfFirstProject_ToTrue()
        {
            // arrange
            var developers = GenerateFourDevelopersWithProductivities(new List<int> {2002, 1301, 1205, 4125 });
            var projects = GenerateThreeProjectsWithAssignedWorkAmount(new List<int> {1053, 45000, 11000});
            var logic = new CompanyLogic();
            int productivitySum = developers.Sum(x => x.Value.CodeLinesPerDay);
            
            int expectedRoundOffWorkDone = projects.Count*(productivitySum/projects.Count);
            // act
            logic.PerformOneWorkDayOnProjects(developers, projects);

            // assert
            var project1 = projects.GetEnumerator();
            project1.MoveNext();
            project1.Current.Value.IsDone.Should().Be(true);
            projects.Sum( x => x.Value.WorkAmountAssigned - x.Value.WorkAmountRest).Should().Be(expectedRoundOffWorkDone);
        }

        [Fact] public void DoWorkOnTheProjects_SetsIsDoneStatusOfAllProjects_ToTrue_AndCompetionStatusToOne()
        {
            // arrange
            var developers = GenerateFourDevelopersWithProductivities(new List<int> {2002, 1301, 1205, 41250 });
            var projects = GenerateThreeProjectsWithAssignedWorkAmount(new List<int> {1053, 45000, 11000});
            var logic = new CompanyLogic();
            int productivitySum = developers.Sum(x => x.Value.CodeLinesPerDay);
            
            int expectedRoundOffWorkDone = projects.Count*(productivitySum/projects.Count);
            // act
            logic.PerformOneWorkDayOnProjects(developers, projects);

            // assert
            projects.Values.Select(x => x.IsDone == true).ToList().Count.Should().Be(3);
            projects.Values.Select(x => x.CompletionPercent == 100).ToList().Count.Should().Be(3);
        }

    }
}

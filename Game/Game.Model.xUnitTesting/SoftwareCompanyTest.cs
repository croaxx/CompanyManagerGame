using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Game.Model.xUnitTesting
{
    public class SoftwareCompanyTest
    {
        public SoftwareCompanyTest()
        {
        }

        private void SetCompanyWithThreeProjects(ICompany company)
        {
            company.TryAcceptNewProject(new Project("Project1", new DateTime(2017, 10, 10)), new DateTime(2017, 10, 5));
            company.TryAcceptNewProject(new Project("Project2", new DateTime(2017, 10, 10)), new DateTime(2017, 10, 5));
            company.TryAcceptNewProject(new Project("Project3", new DateTime(2017, 10, 10)), new DateTime(2017, 10, 5));
        }

        private void SetCompanyWithThreeEmployees(SoftwareCompany company)
        {
            company.TryHireDeveloper(new Developer("Glenn", new DateTime(1990, 12, 2), 6000, 1000));
            company.TryHireDeveloper(new Developer("Niko", new DateTime(1989, 12, 2), 6000, 1000));
            company.TryHireDeveloper(new Developer("Rolf", new DateTime(1992, 12, 2), 6000, 5000));
        }

        [Fact]
        public void CompanyReturns_IEnumerableOfProjects()
        {
            var bookingLogicFake = A.Fake<IBookingLogic>();
            var company = new SoftwareCompany(bookingLogicFake);
            SetCompanyWithThreeProjects(company);

            int expectedNumberOfProjects = 3;

            var projects = company.GetProjects();
            projects.Count.Should().Be(expectedNumberOfProjects);
        }

        [Fact]
        public void CompanyReturns_IEnumerableOfDevelopers()
        {
            var bookingLogicFake = A.Fake<IBookingLogic>();
            var company = new SoftwareCompany(bookingLogicFake);
            SetCompanyWithThreeEmployees(company);

            int expectedNumberOfDevelopers = 3;

            var projects = company.GetDevelopers();
            projects.Count.Should().Be(expectedNumberOfDevelopers);
        }

        [Fact]
        void CompanyCantAcceptNewProject_IfTheProjectWithTheSameNameIsAlreadyPresent()
        {
            // arrange
            var bookingLogicFake = A.Fake<IBookingLogic>();
            var company = new SoftwareCompany(bookingLogicFake);
            bool isAccepted = company.TryAcceptNewProject(new Project("Project1", DateTime.Now), DateTime.Now);

            // act
            isAccepted = company.TryAcceptNewProject(new Project("Project1", DateTime.Now), DateTime.Now);

            isAccepted.Should().Be(false);
        }

        [Fact]
        public void DayJump_WhenTimerReachesNextDay()
        {

        }

        [Fact]
        public void FirstTicIsSavedToLastBookedDay()
        {
            //arrange
            var bookingLogicFake = A.Fake<IBookingLogic>();
            var company = new SoftwareCompany(bookingLogicFake);

            var expectedValue = DateTime.Now;
            //act
            company.UpdateProjectsStatus(expectedValue);
            //assert
            company.LastBookedTime.Should().Be(expectedValue);
        }

        [Fact]
        public void FirstTicIsNotSaved_WhenLastBookedDayHasValue()
        {
            //arrange
            var bookingLogicFake = A.Fake<IBookingLogic>();
            var company = new SoftwareCompany(bookingLogicFake);
            DateTime dateTime = DateTime.Now.AddHours(-1);

            company.LastBookedTime = dateTime;

            var nextTicValue = DateTime.Now;
            //act
            company.UpdateProjectsStatus(nextTicValue);
            //assert
            company.LastBookedTime.Should().Be(dateTime);
        }

        [Fact]
        public void TimerReachesNextDay_ThenBookingFunctionIsCalled()
        {
            //arrange
            var bookingLogicFake = A.Fake<IBookingLogic>();
            var company = new SoftwareCompany(bookingLogicFake);
            DateTime today = DateTime.Now;
            company.LastBookedTime = today;

            var nextTicValue = today.AddDays(1);
            //act
            company.UpdateProjectsStatus(nextTicValue);
            //assert
            A.CallTo(() => bookingLogicFake.BookTime(A<IEnumerable<Project>>.Ignored, A<IEnumerable<Developer>>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void TimerDoesNotReachNextDay_ThenBookingFunctionIsNotCalled()
        {
            //arrange
            var bookingLogicFake = A.Fake<IBookingLogic>();
            var company = new SoftwareCompany(bookingLogicFake);
            DateTime startDate = DateTime.Parse("1.1.2000");
            company.LastBookedTime = startDate;

            var nextTicValue = startDate.AddMilliseconds(1);
            //act
            company.UpdateProjectsStatus(nextTicValue);
            //assert
            A.CallTo(() => bookingLogicFake.BookTime(A<IEnumerable<Project>>.Ignored, A<IEnumerable<Developer>>.Ignored)).MustNotHaveHappened();
        }
        [Fact]
        public void TimerEdgeCaseEndOfDay_ThenBookingFunctionIsCalled()
        {
            //arrange
            var bookingLogicFake = A.Fake<IBookingLogic>();
            var company = new SoftwareCompany(bookingLogicFake);
            DateTime startDate = DateTime.Parse("1.1.2000 23:59:59");
            company.LastBookedTime = startDate;

            var nextTicValue = startDate.AddSeconds(1);
            //act
            company.UpdateProjectsStatus(nextTicValue);
            //assert
            A.CallTo(() => bookingLogicFake.BookTime(A<IEnumerable<Project>>.Ignored, A<IEnumerable<Developer>>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void TimerEdgeCaseEndOfMonth_ThenBookingFunctionIsCalled()
        {
            //arrange
            var bookingLogicFake = A.Fake<IBookingLogic>();
            var company = new SoftwareCompany(bookingLogicFake);
            DateTime startDate = DateTime.Parse("31.1.2000 23:59:59");
            company.LastBookedTime = startDate;

            var nextTicValue = startDate.AddSeconds(1);
            //act
            company.UpdateProjectsStatus(nextTicValue);
            //assert
            A.CallTo(() => bookingLogicFake.BookTime(A<IEnumerable<Project>>.Ignored, A<IEnumerable<Developer>>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void TimerEdgeCaseEndOfYear_ThenBookingFunctionIsCalled()
        {
            //arrange
            var bookingLogicFake = A.Fake<IBookingLogic>();
            var company = new SoftwareCompany(bookingLogicFake);
            DateTime startDate = DateTime.Parse("31.12.2000 23:59:59");
            company.LastBookedTime = startDate;

            var nextTicValue = startDate.AddSeconds(1);
            //act
            company.UpdateProjectsStatus(nextTicValue);
            //assert
            A.CallTo(() => bookingLogicFake.BookTime(A<IEnumerable<Project>>.Ignored, A<IEnumerable<Developer>>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void TimerJumpsMoreThanADay_ThenBookingFunctionIsCalled()
        {
            //arrange
            var bookingLogicFake = A.Fake<IBookingLogic>();
            var company = new SoftwareCompany(bookingLogicFake);
            DateTime startDate = DateTime.Parse("31.12.2000 23:59:59");
            company.LastBookedTime = startDate;

            var nextTicValue = startDate.AddDays(10);
            //act
            company.UpdateProjectsStatus(nextTicValue);
            //assert
            A.CallTo(() => bookingLogicFake.BookTime(A<IEnumerable<Project>>.Ignored, A<IEnumerable<Developer>>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void HiringDeveloperReturnsTrueAndIncreasesCountByOne()
        {
            // arrange 
            var bookingLogicFake = A.Fake<IBookingLogic>();
            var company = new SoftwareCompany(bookingLogicFake);
            int countDevelopers = company.GetNumberOfDevelopers();

            //act
            bool isAccepted = company.TryHireDeveloper(new Developer("Peter Graham", new DateTime(2000, 5, 3), 10000, 3000));

            isAccepted.Should().Be(true);
            company.GetNumberOfDevelopers().Should().Be(countDevelopers + 1);
        }

        [Fact]
        public void HiringDeveloperWithSameNameAndBirthdayReturnsFalse()
        {
            // arrange 
            var bookingLogicFake = A.Fake<IBookingLogic>();
            var company = new SoftwareCompany(bookingLogicFake);
            int countDevelopers = company.GetNumberOfDevelopers();

            //act
            bool isAccepted = company.TryHireDeveloper(new Developer("Peter Graham", new DateTime(2000, 5, 3), 10000, 3000));
            isAccepted = company.TryHireDeveloper(new Developer("Peter Graham", new DateTime(2000, 5, 3), 10000, 3000));

            isAccepted.Should().Be(false);
            company.GetNumberOfDevelopers().Should().Be(countDevelopers + 1);
        }

        public void HiringDeveloperWithSameNameButDifferentBirthdayReturnsTrue()
        {
            // arrange 
            var bookingLogicFake = A.Fake<IBookingLogic>();
            var company = new SoftwareCompany(bookingLogicFake);
            int countDevelopers = company.GetNumberOfDevelopers();

            //act
            bool isAccepted = company.TryHireDeveloper(new Developer("Peter Graham", new DateTime(2000, 5, 3), 10000, 3000));
            isAccepted = company.TryHireDeveloper(new Developer("Peter Graham", new DateTime(2000, 5, 2), 10000, 3000));

            isAccepted.Should().Be(true);
            company.GetNumberOfDevelopers().Should().Be(countDevelopers + 1);
        }

        public void HiringDeveloperWithDifferentNamesButSameBirthdayReturnsTrue()
        {
            // arrange 
            var bookingLogicFake = A.Fake<IBookingLogic>();
            var company = new SoftwareCompany(bookingLogicFake);
            int countDevelopers = company.GetNumberOfDevelopers();

            //act
            bool isAccepted = company.TryHireDeveloper(new Developer("Peter Graham", new DateTime(2000, 5, 3), 10000, 3000));
            isAccepted = company.TryHireDeveloper(new Developer("Niko Komarevskiy", new DateTime(2000, 5, 3), 10000, 3000));

            isAccepted.Should().Be(true);
            company.GetNumberOfDevelopers().Should().Be(countDevelopers + 1);
        }
    }

}

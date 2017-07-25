using FakeItEasy;
using FluentAssertions;
using System;
using Xunit;

namespace Game.Model.xUnitTesting
{
    public class SoftwareCompanyTest
    {
        private void SetupCompanyWithThreeProjects(ICompany company)
        {
            company.TryAcceptNewProject(new Project("Project1", new DateTime(2017, 10, 10)), new DateTime(2017, 10, 5));
            company.TryAcceptNewProject(new Project("Project2", new DateTime(2017, 10, 10)), new DateTime(2017, 10, 5));
            company.TryAcceptNewProject(new Project("Project3", new DateTime(2017, 10, 10)), new DateTime(2017, 10, 5));
        }

        private void SetupCompanyWithThreeEmployees(SoftwareCompany company)
        {
            company.TryHireDeveloper(new Developer("Glenn", new DateTime(1990, 12, 2), 6000, 1000));
            company.TryHireDeveloper(new Developer("Niko", new DateTime(1989, 12, 2), 6000, 1000));
            company.TryHireDeveloper(new Developer("Rolf", new DateTime(1992, 12, 2), 6000, 5000));
        }

        [Fact]
        void CompanyCantAcceptNewProject_IfTheProjectWithTheSameNameIsAlreadyPresent()
        {
            // arrange
            var company = new SoftwareCompany();
            bool isAccepted = company.TryAcceptNewProject(new Project("Project1", DateTime.Now), DateTime.Now);

            // act
            isAccepted = company.TryAcceptNewProject(new Project("Project1", DateTime.Now), DateTime.Now);

            isAccepted.Should().Be(false);
        }

        [Fact]
        public void HiringDeveloperReturnsTrueAndIncreasesCountByOne()
        {
            // arrange 
            var company = new SoftwareCompany();
            int countDevelopers = company.GetNumberOfDevelopers();

            //act
            bool isAccepted = company.TryHireDeveloper(new Developer("Peter Graham", new DateTime(2000, 5, 3), 10000, 3000));

            isAccepted.Should().Be(true);
            company.GetNumberOfDevelopers().Should().Be(countDevelopers + 1);
        }

        [Fact]
        public void HiringDeveloperWithSameNameAndBirthdayReturnsFalse()
        {
            var company = new SoftwareCompany();
            int countDevelopers = company.GetNumberOfDevelopers();

            bool isAccepted = company.TryHireDeveloper(new Developer("Peter Graham", new DateTime(2000, 5, 3), 10000, 3000));
            isAccepted = company.TryHireDeveloper(new Developer("Peter Graham", new DateTime(2000, 5, 3), 10000, 3000));

            isAccepted.Should().Be(false);
            company.GetNumberOfDevelopers().Should().Be(countDevelopers + 1);
        }

        [Fact]
        public void HiringDeveloperWithSameNameButDifferentBirthdayReturnsTrue()
        {
            // arrange 
            var company = new SoftwareCompany();
            int countDevelopers = company.GetNumberOfDevelopers();

            //act
            bool isAccepted = company.TryHireDeveloper(new Developer("Peter Graham", new DateTime(2000, 5, 3), 10000, 3000));
            isAccepted = company.TryHireDeveloper(new Developer("Peter Graham", new DateTime(2000, 5, 2), 10000, 3000));

            isAccepted.Should().Be(true);
            company.GetNumberOfDevelopers().Should().Be(countDevelopers + 2);
        }

        [Fact]
        public void HiringDeveloperWithDifferentNamesButSameBirthdayReturnsTrue()
        {
            // arrange 
            var company = new SoftwareCompany();
            int countDevelopers = company.GetNumberOfDevelopers();

            // act
            bool isAccepted = company.TryHireDeveloper(new Developer("Peter Graham", new DateTime(2000, 5, 3), 10000, 3000));
            isAccepted = company.TryHireDeveloper(new Developer("Niko Komarevskiy", new DateTime(2000, 5, 3), 10000, 3000));
            
            // assert
            isAccepted.Should().Be(true);
            company.GetNumberOfDevelopers().Should().Be(countDevelopers + 2);
        }

        [Fact]
        public void SoftwareCompanyConstructorSetsNextSalaryDateTo25thOfNextYear_()
        {
            // arrange
            var currentTime = new DateTime(2015, 12, 11);

            //act
            var company = new SoftwareCompany(currentTime);
            var expectedNextSalaryDate = new DateTime(2016, 1, 25); 

            // assert
            company.GetNextSalaryPaymentDate().Should().Be(expectedNextSalaryDate);
        }

        [Fact]
        public void SoftwareCompanyConstructorSetsNextSalaryDateTo25thOfNextMonth()
        {
            // arrange
            var currentTime = new DateTime(2015, 11, 11);

            //act
            var company = new SoftwareCompany(currentTime);
            var expectedNextSalaryDate = new DateTime(2015, 12, 25); 

            // assert
            company.GetNextSalaryPaymentDate().Should().Be(expectedNextSalaryDate);
        }

        [Fact]
        public void UpdateCompanyStatus_SetsLastBookedDateToDateOfCurrentTime()
        {
            // arrange
            var logic = A.Fake<ICompanyLogic>();
            var companyFoundationTime = new DateTime(2018, 10, 2, 14, 20, 33);
            var company = new SoftwareCompany(companyFoundationTime);
            var currentTime = new DateTime(2020, 11, 4, 19, 30, 53);
            var expectedNewTime = new DateTime(2020, 11, 4, 14, 20, 33);

            // act
            company.UpdateCompanyStatus(currentTime, logic);

            // assert
            company.LastBookedTime.Should().Be(expectedNewTime);
        }

        [Fact]
        public void UpdateCompanyStatus_Executes_PaySalariesAndRemoveUnpaidDevs_ThreeTimes()
        {
            // arrange
            var logic = A.Fake<ICompanyLogic>();
            var companyFoundationTime = new DateTime(2018, 11, 2, 14, 20, 33);
            var company = new SoftwareCompany(companyFoundationTime);
            var currentTime = new DateTime(2019, 2, 26, 14, 20, 33);
            var expectedNextSalaryTime = new DateTime(2019, 3, 25, 14, 20, 33);

            // act
            company.UpdateCompanyStatus(currentTime, logic);

            // assert
            company.GetNextSalaryPaymentDate().Should().Be(expectedNextSalaryTime);
         }
    }

}

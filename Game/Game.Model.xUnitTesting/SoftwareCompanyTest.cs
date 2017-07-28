using FakeItEasy;
using FluentAssertions;
using System;
using Xunit;

namespace Game.Model.xUnitTesting
{
    public class SoftwareCompanyTest
    {
        private void SetupCompanyWithThreeDifferentProjects(ICompany company)
        {
            var p = new Project("Project1", new DateTime(2017, 10, 10), 0, 0);
            company.TryAcceptNewProject(p, new DateTime(2017, 10, 5));
            p = new Project("Project2", new DateTime(2017, 10, 10), 0, 0);
            company.TryAcceptNewProject(p, new DateTime(2017, 10, 5));
            p = new Project("Project3", new DateTime(2017, 10, 10), 0, 0);
            company.TryAcceptNewProject(p, new DateTime(2017, 10, 5));
        }

        private void SetupCompanyWithThreeDifferentEmployees(SoftwareCompany company)
        {
            company.TryHireDeveloper(new Developer("Glenn", new DateTime(1990, 12, 2), 6000, 1000));
            company.TryHireDeveloper(new Developer("Niko", new DateTime(1989, 12, 2), 6000, 1000));
            company.TryHireDeveloper(new Developer("Rolf", new DateTime(1992, 12, 2), 6000, 5000));
        }

        [Fact]
        void CompanyCantAcceptNewProject_IfTheProjectWithTheSameNameIsAlreadyPresent()
        {
            var company = new SoftwareCompany();
            var project = new Project("Project1", DateTime.Now, 0 , 0);
            
            bool isAccepted = company.TryAcceptNewProject(project, DateTime.Now);

            isAccepted.Should().Be(false);
        }

        [Fact]
        public void HiringDeveloperReturnsTrueAndIncreasesCountByOne()
        {
            var company = new SoftwareCompany();
            var dev = new Developer("Peter Graham", new DateTime(2000, 5, 3), 10000, 3000);
            int expectedNumberOfDevs = 1;

            bool isAccepted = company.TryHireDeveloper(dev);

            isAccepted.Should().Be(true);
            company.GetNumberOfDevelopers().Should().Be(expectedNumberOfDevs);
        }

        [Fact]
        public void HiringDeveloperWithSameNameAndBirthdayReturnsFalse()
        {
            var company = new SoftwareCompany();
            var dev = new Developer("Peter Graham", new DateTime(2000, 5, 3), 0, 0);
            company.TryHireDeveloper(dev);
            int expectedNumberOfDevs = 1;

            bool isAccepted = company.TryHireDeveloper(dev);

            isAccepted.Should().Be(false);
            company.GetNumberOfDevelopers().Should().Be(expectedNumberOfDevs);
        }

        [Fact]
        public void HiringDeveloperWithSameNameButDifferentBirthdayReturnsTrue()
        {
            var company = new SoftwareCompany();
            var dev = new Developer("Peter Graham", new DateTime(2000, 5, 3), 10000, 3000);
            company.TryHireDeveloper(dev);
            dev = new Developer("Peter Graham", new DateTime(2000, 5, 2), 0, 0);
            int expectedNumberOfDevs = 2;
 
            bool isAccepted = company.TryHireDeveloper(dev);

            isAccepted.Should().Be(true);
            company.GetNumberOfDevelopers().Should().Be(expectedNumberOfDevs);
        }

        [Fact]
        public void HiringDeveloperWithDifferentNamesButSameBirthdayReturnsTrue()
        {
            var company = new SoftwareCompany();
            var dev = new Developer("Peter Graham", new DateTime(2000, 5, 3), 0, 0);
            company.TryHireDeveloper(dev);
            dev = new Developer("Nikolay Komarevskiy", new DateTime(2000, 5, 3), 0, 0);
            int expectedNumberOfDevs = 2;
 
            bool isAccepted = company.TryHireDeveloper(dev);

            isAccepted.Should().Be(true);
            company.GetNumberOfDevelopers().Should().Be(expectedNumberOfDevs);
        }

        [Fact]
        public void SoftwareCompanyConstructorSetsNextSalaryDateTo25thOfNextYear()
        {
            var currentTime = new DateTime(2015, 12, 11);
            var expectedNextSalaryDate = new DateTime(2016, 1, 25); 

            var company = new SoftwareCompany(currentTime);

            company.GetNextSalaryPaymentDate().Should().Be(expectedNextSalaryDate);
        }

        [Fact]
        public void SoftwareCompanyConstructorSetsNextSalaryDateTo25thOfNextMonth()
        {
            var currentTime = new DateTime(2015, 11, 11);
            var expectedNextSalaryDate = new DateTime(2015, 12, 25); 

            var company = new SoftwareCompany(currentTime);

            company.GetNextSalaryPaymentDate().Should().Be(expectedNextSalaryDate);
        }

        [Fact]
        public void UpdateCompanyStatus_SetsLastBookedDateToDateOfCurrentTime()
        {
            var logic = A.Fake<ICompanyLogic>();
            var companyFoundationTime = new DateTime(2018, 10, 2, 14, 20, 33);
            var company = new SoftwareCompany(companyFoundationTime);
            var currentTime = new DateTime(2020, 11, 4, 19, 30, 53);
            var expectedNewTime = new DateTime(2020, 11, 4, 14, 20, 33);

            company.UpdateCompanyStatus(currentTime, logic);

            company.LastBookedTime.Should().Be(expectedNewTime);
        }

        [Fact]
        public void UpdateCompanyStatus_Executes_PaySalariesAndRemoveUnpaidDevs_ThreeTimes()
        {
            var logic = A.Fake<ICompanyLogic>();
            var companyFoundationTime = new DateTime(2018, 11, 2, 14, 20, 33);
            var company = new SoftwareCompany(companyFoundationTime);
            var currentTime = new DateTime(2019, 2, 26, 14, 20, 33);
            var expectedNextSalaryTime = new DateTime(2019, 3, 25, 14, 20, 33);

            company.UpdateCompanyStatus(currentTime, logic);

            company.GetNextSalaryPaymentDate().Should().Be(expectedNextSalaryTime);
         }
    }

}

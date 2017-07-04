using FluentAssertions;
using Xunit;

namespace Game.Model.xUnitTesting
{
    public class SoftwareCompanyxUnitTesting
    {
        private SoftwareCompany testee;

        public SoftwareCompanyxUnitTesting()
        {
            this.testee = new SoftwareCompany();
        }

        [Fact]
        public void WhenNewDeveloperHired_DevelopersCountIncreases()
        {
            var devs = this.testee.GetNumberOfEmployees();

            this.testee.HireDeveloper(new Developer("Thomas", 7000, 10000));

            this.testee.GetNumberOfEmployees().Should().Be(devs + 1);
        }
    }
}

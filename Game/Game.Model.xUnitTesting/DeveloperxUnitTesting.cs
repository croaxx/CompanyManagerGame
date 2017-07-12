using System;
using Xunit;

namespace Game.Model.xUnitTesting
{
    public class DeveloperxUnitTesting
    {
        [Fact] 
        public void TwoDevelopersAreEqualIfTheirFullnamesAndBirthdaysAreEqual()
        {
            var dev1 = new Developer("Peter Graham", new DateTime(1985, 12, 5), 10000, 3000);
            var dev2 = new Developer("Peter Graham", new DateTime(1985, 12, 5), 10000, 3000);
            var dev3 = new Developer("Peter Grahams", new DateTime(1985, 12, 5), 10000, 3000);
            var dev4 = new Developer("Peter Graham", new DateTime(1985, 12, 4), 10000, 3000);

            Assert.True(dev1.Equals(dev2));
            Assert.False(dev1.Equals(dev3));
            Assert.False(dev1.Equals(dev4));
        }
    }
}

using Game.Model;
using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Game.DataServices
{
    public class DevelopersDataService
    {
        private IList<Developer> developers;

        private int currentDeveloperIdx;

        public DevelopersDataService()
        {
            string path = @"C:\Users\nikolaykomarevskiy\Documents\IdleProject\CompanyManagerGame\CompanyManagerGame\Game\Game.DataServices\DevelopersFotos\";
            this.developers = new List<Developer>
            {
                { new Developer("Gregory Bleiker", new DateTime(1980, 12, 5), 10000, 200, new BitmapImage(new System.Uri(path + "Bleiker.jpg",UriKind.Relative)))},
                { new Developer("Glenn Gruenberg", new DateTime(1990, 12, 5), 9000, 150, new BitmapImage(new System.Uri(path + "Gruenberg.jpg",UriKind.Relative)))},
                { new Developer("Nikolay Komarevskiy", new DateTime(1986, 2, 10), 8000, 125, new BitmapImage(new System.Uri(path + "Komarevskiy.jpg",UriKind.Relative)))},
                { new Developer("Oliver Christen", new DateTime(1982, 12, 8), 7000, 165, new BitmapImage(new System.Uri(path + "Christen.jpg",UriKind.Relative)))},
                { new Developer("Thomas Britschgi", new DateTime(1989, 3, 12), 6000, 185, new BitmapImage(new System.Uri(path + "Britschgi.jpg",UriKind.Relative)))},
                { new Developer("Nikolay Komarevskiy", new DateTime(1986, 2, 10), 5000, 215, new BitmapImage(new System.Uri(path + "Komarevskiy.jpg",UriKind.Relative)))},
                { new Developer("Jan Bosshard", new DateTime(1990, 10, 15), 10000, 285, new BitmapImage(new System.Uri(path + "Bosshard.jpg",UriKind.Relative)))},
            };
            this.currentDeveloperIdx = 0;
        }

        public Developer GetNextDeveloper()
        {
            return  developers[currentDeveloperIdx++];
        }

        public bool IsNextDeveloperAvailable()
        {
            return currentDeveloperIdx < developers.Count;
        }
    }
}

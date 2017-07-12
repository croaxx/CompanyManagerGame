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
                { new Developer("Gregory Bleiker", new DateTime(1980, 12, 5), 10000, 4000, new BitmapImage(new System.Uri(path + "Bleiker.jpg",UriKind.Relative)))},
                { new Developer("Glenn Gruenberg", new DateTime(1990, 12, 5), 10000, 3500, new BitmapImage(new System.Uri(path + "Gruenberg.jpg",UriKind.Relative)))},
                { new Developer("Nikolay Komarevskiy", new DateTime(1986, 2, 10), 10000, 3000, new BitmapImage(new System.Uri(path + "Komarevskiy.jpg",UriKind.Relative)))},
                { new Developer("Oliver Christen", new DateTime(1982, 12, 8), 10000, 4000, new BitmapImage(new System.Uri(path + "Christen.jpg",UriKind.Relative)))},
                { new Developer("Thomas Britschgi", new DateTime(1989, 3, 12), 10000, 2000, new BitmapImage(new System.Uri(path + "Britschgi.jpg",UriKind.Relative)))},
                { new Developer("Jan Bosshard", new DateTime(1990, 10, 15), 10000, 5000, new BitmapImage(new System.Uri(path + "Bosshard.jpg",UriKind.Relative)))}
            };
            this.currentDeveloperIdx = 0;
        }

        public Developer GetNextDeveloper()
        {
            return  developers[currentDeveloperIdx++];
        }

        public bool IsNextProjectAvailable()
        {
            return currentDeveloperIdx < developers.Count;
        }
    }
}

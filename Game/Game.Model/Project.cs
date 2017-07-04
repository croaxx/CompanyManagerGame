using System;

namespace Game.Model
{
    public class Project
    {
        public Project(string title, DateTime expiry)
        {
            this.Title = title;
            this.Expiry = expiry;
        }
        public Project(DateTime expiry)
        {
            this.Expiry = expiry;
        }
        public string Title { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime Expiry { get; private set; }
        public long WorkAmountAssigned { get; private set; }
        public long WorkAmountDone { get; private set; }
        public bool IsExpired(DateTime current)
        {
            return DateTime.Compare(this.Expiry, current) < 0 ? true : false;
        }
    }
}

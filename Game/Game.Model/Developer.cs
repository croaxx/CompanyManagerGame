using System;
using System.Windows.Media;

namespace Game.Model
{
    public class Developer
    {
        public Developer(string fullname, DateTime birth, int monthsalary, int productivity)
        {
            this.FullName = fullname;
            this.Birth = birth;
            this.MonthlySalary = monthsalary;
            this.CodeLinesPerDay = productivity;
        }

        public Developer(string fullname, DateTime birth, int monthsalary, int productivity, ImageSource image) : this(fullname, birth, monthsalary, productivity)
        {
            this.Picture = image;
        }

        public Developer(string fullname, DateTime birth, DateTime entryDate, int monthsalary, int productivity, ImageSource image) : this(fullname, birth, monthsalary, productivity)
        {
            this.Picture = image;
            this.JobStartDate = entryDate;
        }

        public string FullName { get; private set; }
        public DateTime Birth { get; private set; }
        public int MonthlySalary { get; private set; }
        public int CodeLinesPerDay { get; private set; }
        public bool IsLeaving { get; private set; } = false; 
        public DateTime? FireDate { get; private set; } = null;
        public DateTime JobStartDate { get; private set; }
        public bool Resign(DateTime time)
        {
            if (FireDate == null)
            {
                FireDate = time;
                this.IsLeaving = true;
                return true;
            }
            return false;
        }
        public override bool Equals(object obj)
        {
            Developer d = obj as Developer;
            if (d == null)
                return false;
            return d.FullName == this.FullName && d.Birth == this.Birth;
        }
        public override int GetHashCode()
        {
            int hash = 17;
            hash = (hash * 23) + FullName.GetHashCode();
            hash = (hash * 23) + Birth.GetHashCode();
            return hash;
        }
        public ImageSource Picture { get; private set; }
    }
}

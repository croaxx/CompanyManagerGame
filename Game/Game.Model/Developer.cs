using System;
using System.Windows.Media;

namespace Game.Model
{
    public class Developer : IDeveloper
    {
        public string FullName { get; }
        public DateTime Birth { get; }
        public int MonthlySalary { get; }
        public ImageSource Picture { get; }
        public int CodeLinesPerDay { get; }
        public DateTime? FireDate { get; private set; } = null;
        public Developer(string fullname, DateTime birth, int monthsalary, int productivity)
        {
            FullName = fullname;
            Birth = birth;
            MonthlySalary = monthsalary;
            CodeLinesPerDay = productivity;
        }
        public Developer(string fullname, DateTime birth, int monthsalary, int productivity, ImageSource image) : this(fullname, birth, monthsalary, productivity)
        {
            Picture = image;
        }
        public bool Resign(DateTime time)
        {
            if (FireDate == null)
            {
                FireDate = time;
                return true;
            }
            return false;
        }
        public override bool Equals(object obj)
        {
            Developer d = obj as Developer;

            if (d == null) return false;

            return d.FullName == FullName && d.Birth == Birth;
        }
        public override int GetHashCode()
        {
            // joint hash of name and birthdate
            int hash = 17;
            hash = (hash * 23) + FullName.GetHashCode();
            hash = (hash * 23) + Birth.GetHashCode();
            return hash;
        }
    }
}

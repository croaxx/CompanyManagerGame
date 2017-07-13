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
            this.MonthSalary = monthsalary;
            this.CodeLinesPerMonth = productivity;
        }

        public Developer(string fullname, DateTime birth, int monthsalary, int productivity, ImageSource image) : this(fullname, birth, monthsalary, productivity)
        {
            this.Picture = image;
        }

        public string FullName { get; private set; }
        public DateTime Birth { get; private set; }
        public int MonthSalary { get; private set; }
        public int CodeLinesPerMonth { get; private set; }

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

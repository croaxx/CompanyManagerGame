using System;
using System.Windows.Media;

namespace Game.Model
{
    public class Developer : IEquatable<Developer>
    {
        public Developer(string fullname, DateTime birth, int monthsalary, int productivity)
        {
            this.FullName = fullname;
            this.Birth = birth;
            this.MonthSalary = monthsalary;
            this.CodeLinesPerDay = productivity;
        }

        public Developer(string fullname, DateTime birth, int monthsalary, int productivity, ImageSource image) : this(fullname, birth, monthsalary, productivity)
        {
            this.Picture = image;
        }

        public string FullName { get; private set; }
        public DateTime Birth { get; private set; }
        public int MonthSalary { get; private set; }
        public long CodeLinesPerDay { get; private set; }

        public bool Equals(Developer other)
        {
            return FullName == other.FullName && Birth == other.Birth;
        }

        public ImageSource Picture { get; private set; }
    }
}

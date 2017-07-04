using System;

namespace Game.Model
{
    public class Developer : IEquatable<Developer>
    {
        public Developer(string name, int salary, int productivity) 
        {
        }

        public string FullName { get; private set; }
        public int Salary { get; private set; }
        public long Productivity { get; private set; }

        public bool Equals(Developer other)
        {
            return FullName == other.FullName;
        }
    }
}

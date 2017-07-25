using System;

namespace Game.Model
{
    public interface IDeveloper
    {
        string FullName { get; }
        DateTime Birth { get; }
        int MonthlySalary { get; }
        int CodeLinesPerDay { get; }
        DateTime? FireDate { get; }
        bool Resign(DateTime time);
    }
}

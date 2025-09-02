using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxOfficeGuesser.Model;

public struct Movie
{
    public string name;
    public int year;
    public long boxOfficeIncome;

    public static bool operator ==(Movie a, Movie b)
    {
        return a.name == b.name && a.year == b.year && a.boxOfficeIncome == b.boxOfficeIncome;
    }

    public static bool operator !=(Movie a, Movie b)
    {
        return a.name != b.name || a.year != b.year || a.boxOfficeIncome != b.boxOfficeIncome;
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Movie movie && movie == this;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(name, year, boxOfficeIncome);
    }
}
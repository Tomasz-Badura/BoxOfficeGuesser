using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace BoxOfficeGuesser.Stores;

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

    public override bool Equals([NotNullWhen(true)]object? obj)
    {
        if(obj is Movie other)
        {
            return this == other;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return name.GetHashCode() ^ year.GetHashCode() ^ boxOfficeIncome.GetHashCode();
    }
}

public class MovieStore
{
    public int MoviesCount => movies.Count; 
    public Movie GetMovie(int index) => movies.ElementAt(index);
    public Movie[]? GetRandomMovies(int count, int? seed = null)
    {
        if(count > movies.Count / 2)
        {
            return null;
        }

        Random random;
        if(seed is not null)
        {
            random = new(seed.Value);
        }
        else
        {
            random = new Random();
        }

        HashSet<Movie> result = new();

        for(int i = 0; i < count; i++)
        {
            int movieIndex = random.Next(0, movies.Count);

            while(result.Add(movies.ElementAt(movieIndex)) == false)
            {
                movieIndex = random.Next(0, movies.Count);
            }
        }

        return result.ToArray();
    }

    private readonly List<Movie> movies = new();

    public MovieStore(string moviesCsvFilePath)
    {
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, moviesCsvFilePath);
        using(TextFieldParser parser = new(filePath))
        {
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");

            string[]? fields = parser.ReadFields();

            if(fields is null || fields.Length != 3)
            {
                throw new Exception($"Error while parsing file {filePath}");
            }

            string[] expectedFields = ["Year", "Movie", "Income"];
            int[] order = new int[3];
            Array.Fill(order, -1);
            for(int i = 0; i < fields.Length; i++)
            {
                int index = Array.IndexOf(expectedFields, fields[i]);
                if(index == -1 || order[index] != -1)
                {
                    throw new Exception($"Error while parsing file {filePath}");
                }

                order[index] = i;
            }

            if(order.Contains(-1))
            {
                throw new Exception($"Error while parsing file {filePath}");
            }

            while(parser.EndOfData == false)
            {
                fields = parser.ReadFields();

                if(fields is null || fields.Length != 3)
                {
                    return;
                }

                movies.Add(new()
                {
                    year = int.Parse(fields[order[0]]),
                    name = fields[order[1]],
                    boxOfficeIncome = long.Parse(fields[order[2]]),
                });
            }
        }
    }
}

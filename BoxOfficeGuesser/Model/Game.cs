using BoxOfficeGuesser.Stores;

namespace BoxOfficeGuesser.Model;

public enum Guess
{
    GreaterThan,
    LessThan
}

public struct Player
{
    public string username;
    public int lifes;
    public int points;
}

public class Game
{
    public int CurrentPlayer { get; private set; }
    public int PlayerCount => players.Length;
    public bool GameEnded => players.All(x => x.lifes <= 0);
    public Movie CurrentMovie { get; private set; }
    public Movie ComparedToMovie { get; private set; }

    private readonly Player[] players;
    private readonly Movie[] movies;
    private readonly Random random = new();

    public Game(int gameSeed, int playerLifes, string[] playerNames, Movie[] movies) : this(playerLifes, playerNames, movies)
    {
        random = new(gameSeed + movies.Length);
    }

    public Game(int playerLifes, string[] playerNames, Movie[] movies)
    {
        if(movies.Length <= 1)
        {
            throw new ArgumentException("There must be more than 2 movies", nameof(movies));
        }

        if(playerNames.Length <= 0)
        {
            throw new ArgumentException("There must be at least one player", nameof(playerNames));
        }

        if(playerLifes <= 0)
        {
            throw new ArgumentException("Player needs atleast 1 life", nameof(playerLifes));
        }

        CurrentMovie = movies[0];
        ComparedToMovie = movies[1];
        this.movies = movies;
        players = new Player[playerNames.Length];
        for(int i = 0; i < players.Length; i++)
        {
            players[i] = new Player()
            {
                username = playerNames[i],
                lifes = playerLifes,
                points = 0
            };
        }
    }

    public Movie GetNextRandomMovie()
    {
        int rand = random.Next(0, movies.Length);

        while(movies[rand] == CurrentMovie || movies[rand] == ComparedToMovie)
        {
            rand = random.Next(0, movies.Length);
        }

        return movies[rand];
    }

    public Player GetCurrentPlayer()
    {
        return players[CurrentPlayer];
    }

    public Player[] GetPlayers()
    {
        return players;
    }

    public bool GuessNextNumber(Guess guess)
    {
        if(GameEnded == true)
        {
            return false;
        }

        Player player = players[CurrentPlayer];
        bool result = guess switch
        {
            Guess.LessThan => CurrentMovie.boxOfficeIncome < ComparedToMovie.boxOfficeIncome,
            Guess.GreaterThan => CurrentMovie.boxOfficeIncome > ComparedToMovie.boxOfficeIncome,
            _ => throw new NotImplementedException()
        };
        
        players[CurrentPlayer] = new()
        {
            username = player.username,
            lifes = player.lifes - (result ? 0 : 1),
            points = player.points + (result ? 1 : 0)
        };

        CurrentMovie = ComparedToMovie;
        ComparedToMovie = GetNextRandomMovie();

        do
        {
            CurrentPlayer = (CurrentPlayer + 1) % players.Length;
            player = players[CurrentPlayer];
        }
        while(player.lifes <= 0 && GameEnded == false);

        return result;
    }
}

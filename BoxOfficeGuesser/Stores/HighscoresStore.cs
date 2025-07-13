using BoxOfficeGuesser.EntityModels;
using BoxOfficeGuesser.Model;

namespace BoxOfficeGuesser.Stores;

public class HighscoresStore
{
    private readonly AppDbContext context;

    public HighscoresStore(AppDbContext dbContext)
    {
        context = dbContext;
    }

    public void InsertHighscore(Score newScore)
    {
        try
        {
            _ = context.Scores.Add(newScore);
            _ = context.SaveChanges();
        }
        catch(Exception ex)
        {
            throw new Exception("An error occurred while inserting the highscore", ex);
        }
    }

    public List<Score> GetHighscores()
    {
        try
        {
            return context.Scores
                .OrderByDescending(s => s.Points)
                .ToList();
        }
        catch(Exception ex)
        {
            throw new Exception("An error occurred while retrieving highscores", ex);
        }
    }
}
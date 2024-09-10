using BoxOfficeGuesser.ViewModel;
namespace BoxOfficeGuesser.Model;

public class Score
{
    public int ID { get; set; }
    public DateTime Date { get; set; }
    public string Username { get; set; } = default!;
    public int Points { get; set; }
    public int Difficulty { get; set; }
    public GameDifficulty GameDifficulty
    {
        get => (GameDifficulty) Difficulty;
        set => Difficulty = (int) value;
    }
}
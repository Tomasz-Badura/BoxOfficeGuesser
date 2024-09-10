using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

using BoxOfficeGuesser.Factories;
using BoxOfficeGuesser.Model;
using BoxOfficeGuesser.Stores;
using BoxOfficeGuesser.ViewModel.Commands;

namespace BoxOfficeGuesser.ViewModel;

public class GameViewModel : ViewModelBase
{
    public GuessCommand LessThanButtonClick { get; }
    public GuessCommand GreaterThanButtonClick { get; }
    public string CurrentPlayerName => game.GetCurrentPlayer().username;
    public string CurrentPlayerLifes => game.GetCurrentPlayer().lifes.ToString();
    public string CurrentPlayerPoints => game.GetCurrentPlayer().points.ToString();
    public string MovieLeftName => game.CurrentMovie.name;
    public string MovieLeftYear => "(" + game.CurrentMovie.year.ToString() + ")";
    public string MovieLeftIncome => $"{game.CurrentMovie.boxOfficeIncome:N0}$";
    public string MovieRightName => game.ComparedToMovie.name;
    public string MovieRightYear => "(" + game.ComparedToMovie.year.ToString() + ")";

    private readonly Game game;
    private readonly NavigationStore navigationStore;
    private readonly HighscoresStore highscoresStore;
    private readonly GameDifficulty gameDifficulty;

    public GameViewModel(Game game,
        GameDifficulty gameDifficulty,
        NavigationStore navigationStore, 
        HighscoresStore highscoresStore)
    {
        this.game = game;
        this.navigationStore = navigationStore;
        this.highscoresStore = highscoresStore;
        this.gameDifficulty = gameDifficulty;

        LessThanButtonClick = new(game, Guess.LessThan);
        GreaterThanButtonClick = new(game, Guess.GreaterThan);
        LessThanButtonClick.Executed += OnGuess;
        GreaterThanButtonClick.Executed += OnGuess;
    }

	public override WindowOptions WindowOptions { get; protected set; } = new()
	{
		minWidth = 1000,
		minHeight = 600,
		defaultWidth = 1000,
		defaultHeight = 600,
		resizeMode = System.Windows.ResizeMode.CanResizeWithGrip
	};

	public void OnGuess()
    {
        if(game.GameEnded == true)
        {
            Player[] players = game.GetPlayers();
            StringBuilder sb = new();

            int highestPoints = players.Max(p => p.points);
            List<Player> winners = players.Where(p => p.points == highestPoints).ToList();

            foreach(Player player in players)
            {
                string winnerStr = " ";
                if(winners.Contains(player))
                {
                    winnerStr = " won and ";
                    highscoresStore.InsertHighscore(new()
                    {
                        Date = DateTime.Now,
                        Points = player.points,
                        Username = player.username,
                        GameDifficulty = gameDifficulty,
                    });
                }

                _ = sb.AppendLine($"Player {player.username}{winnerStr}achieved: {player.points} point" + (player.points > 1 ? "s" : ""));
            }

            _ = MessageBox.Show(sb.ToString(), "Game has ended!", MessageBoxButton.OK, MessageBoxImage.Information);
            navigationStore.NavigateTo<GameCreationViewModel>();
            return;
        }

        OnPropertyChanged(nameof(CurrentPlayerName));
        OnPropertyChanged(nameof(CurrentPlayerLifes));
        OnPropertyChanged(nameof(CurrentPlayerPoints));
        OnPropertyChanged(nameof(MovieLeftName));
        OnPropertyChanged(nameof(MovieLeftYear));
        OnPropertyChanged(nameof(MovieLeftIncome));
        OnPropertyChanged(nameof(MovieRightName));
        OnPropertyChanged(nameof(MovieRightYear));
    }
}

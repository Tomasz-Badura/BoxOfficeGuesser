using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

using BoxOfficeGuesser.Model;
using BoxOfficeGuesser.Stores;
using BoxOfficeGuesser.ViewModel.Commands;

namespace BoxOfficeGuesser.ViewModel;

public enum GameDifficultyDisplay
{
    Easy,
    Medium,
    Hard,
    All
}

public class HighscoresViewModel : ViewModelBase
{
    public ICollectionView Scores => displayedScores.View;

    public GameDifficultyDisplay GameDifficultyFilter
    {
        get => gameDifficultyFilter;
        set
        {
            gameDifficultyFilter = value;
            displayedScores.View.Refresh();
        }
    }

    public override WindowOptions WindowOptions { get; protected set; } = new()
    {
        minWidth = 700,
        minHeight = 300,
        maxHeight = int.MaxValue,
        maxWidth = int.MaxValue,
        defaultWidth = 700,
        defaultHeight = 300,
        resizeMode = System.Windows.ResizeMode.CanResizeWithGrip
    };

    public SwitchHighscoreViewCommand EasyScoresButtonClick { get; }
    public SwitchHighscoreViewCommand MediumScoresButtonClick { get; }
    public SwitchHighscoreViewCommand HardScoresButtonClick { get; }
    public SwitchHighscoreViewCommand AllScoresButtonClick { get; }
    public NavigateCommand<GameCreationViewModel> BackButtonClick { get; }

    private GameDifficultyDisplay gameDifficultyFilter;
    private readonly ReadOnlyCollection<Score> scores;
    private readonly CollectionViewSource displayedScores;
    private readonly HighscoresStore highscoresStore;

    public HighscoresViewModel(HighscoresStore highscoresStore, NavigationStore navigationStore)
    {
        this.highscoresStore = highscoresStore;

        gameDifficultyFilter = GameDifficultyDisplay.All;
        scores = highscoresStore.GetHighscores().AsReadOnly();
        displayedScores = new CollectionViewSource
        {
            Source = scores
        };
        displayedScores.Filter += ApplyFilter;

        EasyScoresButtonClick = new(this, GameDifficultyDisplay.Easy);
        MediumScoresButtonClick = new(this, GameDifficultyDisplay.Medium);
        HardScoresButtonClick = new(this, GameDifficultyDisplay.Hard);
        AllScoresButtonClick = new(this, GameDifficultyDisplay.All);
        BackButtonClick = new(navigationStore);
    }

    private void ApplyFilter(object sender, FilterEventArgs e)
    {
        Score score = e.Item as Score ?? throw new ArgumentException($"Passed to filter object not of type {nameof(Score)}", nameof(e));

        e.Accepted = score.GameDifficulty == gameDifficultyFilter switch
        {
            GameDifficultyDisplay.Easy => GameDifficulty.Easy,
            GameDifficultyDisplay.Medium => GameDifficulty.Medium,
            GameDifficultyDisplay.Hard => GameDifficulty.Hard,
            GameDifficultyDisplay.All => score.GameDifficulty,
            _ => throw new NotImplementedException()
        };
    }
}

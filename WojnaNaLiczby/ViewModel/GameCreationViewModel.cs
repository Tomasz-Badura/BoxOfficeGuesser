using System.Globalization;
using System.Windows.Data;

using BoxOfficeGuesser.Factories;
using BoxOfficeGuesser.Stores;
using BoxOfficeGuesser.ViewModel.Commands;

namespace BoxOfficeGuesser.ViewModel;

public enum GameDifficulty
{
    Easy = 3,
    Medium = 2,
    Hard = 1
}

public enum GamePlayerCount
{
    One = 1,
    Two = 2
}

public class ComparisonConverter : IValueConverter
{
    public object? Convert(object value, Type targetType,  object parameter, CultureInfo culture)
    {
        return value?.Equals(parameter);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value?.Equals(true) == true ? parameter : Binding.DoNothing;
    }
}

public partial class GameCreationViewModel : ViewModelBase
{
    public override WindowOptions WindowOptions { get; protected set; } = new()
    {
        minWidth = 400,
        minHeight = 230,
        defaultWidth = 400,
        defaultHeight = 230,
        maxWidth = 400,
        maxHeight = 230,
        resizeMode = System.Windows.ResizeMode.NoResize
    };

    public GameCreationViewModel(NavigationStore navigationStore, StartGameCommandFactory startGameCommandFactory)
    {
        NavigateToHighscoresViewButtonClick = new(navigationStore);
        StartButtonClick = startGameCommandFactory.Create(this);
    }

    public GamePlayerCount PlayerCount
    {
        get
        {
            return playerCount;
        }
        set
        {
            playerCount = value;
            OnPropertyChanged(nameof(PlayerCount));
        }
    }

    public GameDifficulty GameDifficulty
    {
        get
        {
            return gameDifficulty;
        }
        set
        {
            gameDifficulty = value;
            OnPropertyChanged(nameof(GameDifficulty));
        }
    }

    public NavigateCommand<HighscoresViewModel> NavigateToHighscoresViewButtonClick { get; }
    public ShowTutorialCommand ShowTutorialButtonClick { get; } = new();
    public StartGameCommand StartButtonClick { get; }
    public ExitGameCommand ExitButtonClick { get; } = new();

    private GameDifficulty gameDifficulty;
    private GamePlayerCount playerCount;
}

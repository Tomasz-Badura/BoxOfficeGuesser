namespace BoxOfficeGuesser.ViewModel.Commands;
public class SwitchHighscoreViewCommand : CommandBase
{
    private readonly HighscoresViewModel highscoresViewModel;
    private readonly GameDifficultyDisplay gameDifficulty;

    public SwitchHighscoreViewCommand(HighscoresViewModel viewModel, GameDifficultyDisplay difficulty)
    {
        highscoresViewModel = viewModel;
        gameDifficulty = difficulty;
    }

    protected override void OnExecute(object? parameter)
    {
        highscoresViewModel.GameDifficultyFilter = gameDifficulty;
    }
}

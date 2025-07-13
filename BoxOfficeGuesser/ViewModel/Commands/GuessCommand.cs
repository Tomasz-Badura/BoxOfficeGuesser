using BoxOfficeGuesser.Model;

namespace BoxOfficeGuesser.ViewModel.Commands;
public class GuessCommand : CommandBase
{
    private readonly Game game;
    private readonly Guess guess;

    public GuessCommand(Game game, Guess guess)
    {
        this.game = game;
        this.guess = guess;
    }

    protected override void OnExecute(object? parameter)
    {
        _ = game.GuessNextNumber(guess);
    }
}
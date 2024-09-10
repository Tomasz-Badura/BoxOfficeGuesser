namespace BoxOfficeGuesser.ViewModel.Commands;
public class ExitGameCommand : CommandBase
{
    protected override void OnExecute(object? parameter)
    {
        Environment.Exit(0);
    }
}

using System.ComponentModel;
using System.Windows;

using BoxOfficeGuesser.Factories;
using BoxOfficeGuesser.Model;
using BoxOfficeGuesser.Stores;
using BoxOfficeGuesser.Windows;

namespace BoxOfficeGuesser.ViewModel.Commands;
public class StartGameCommand : CommandBase
{
    private readonly GameCreationViewModel viewModel;
    private readonly NavigationStore navigationStore;
    private readonly MovieStore movieStore;
    private readonly GameViewModelFactory gameViewModelFactory;

    public StartGameCommand(GameCreationViewModel viewModel, 
        GameViewModelFactory gameViewModelFactory,
        NavigationStore navigationStore, 
        MovieStore movieStore)
    {
        this.viewModel = viewModel;
        this.navigationStore = navigationStore;
        this.movieStore = movieStore;
        this.gameViewModelFactory = gameViewModelFactory;

        viewModel.PropertyChanged += PropertyChanged;
    }

    private void PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        RaiseCanExecuteChanged();
    }

    public override bool CanExecute(object? parameter)
    {
        int playerLifes = (int) viewModel.GameDifficulty;
        int playerCount = (int) viewModel.PlayerCount;

        return playerLifes > 0
            && playerLifes <= 3
            && playerCount > 0
            && playerCount <= 2;
    }

    protected async override void OnExecute(object? parameter)
    {
        int playerLifes = (int) viewModel.GameDifficulty;
        int playerCount = (int) viewModel.PlayerCount;

        string[] playerNames = new string[playerCount];

        for(int i = 0; i < playerCount; i++)
        {
            PromptWindowEventArgs promptResult = await PromptWindow.OpenPromptWindow(
                Application.Current.MainWindow,
                $"Wprowadź nazwę gracza {i}:",
                (x) => !string.IsNullOrEmpty(x) && !playerNames.Contains(x) && x.Length < 30, 
                PromptValidatorType.Result);

            if(promptResult.result == PromptWindowResult.Cancel)
            {
                return;
            }

            playerNames[i] = promptResult.value!;
        }

        Movie[] movies = movieStore.GetRandomMovies(movieStore.MoviesCount / 3) ?? throw new Exception("Failed to retrieve movies from MovieStore");
        Game game = new(playerLifes, playerNames, movies);  
        navigationStore.CurrentViewModel = gameViewModelFactory.Create(game, viewModel.GameDifficulty);
    }
}
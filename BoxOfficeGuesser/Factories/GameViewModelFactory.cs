
using Microsoft.Extensions.DependencyInjection;

using BoxOfficeGuesser.Model;
using BoxOfficeGuesser.Stores;
using BoxOfficeGuesser.ViewModel;

namespace BoxOfficeGuesser.Factories;

public class GameViewModelFactory : AbstractFactory<GameViewModel>
{
    public GameViewModelFactory(IServiceProvider provider) : base(provider)
    {
    }

    public override GameViewModel Create(params object[] parameters)
    {
        ThrowParams<Game, GameDifficulty>(parameters);

        return new((Game) parameters[0], (GameDifficulty) parameters[1],
            provider.GetRequiredService<NavigationStore>(), 
            provider.GetRequiredService<HighscoresStore>());
    }
}

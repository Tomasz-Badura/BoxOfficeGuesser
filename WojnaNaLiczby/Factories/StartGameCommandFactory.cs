
using Microsoft.Extensions.DependencyInjection;

using BoxOfficeGuesser.Model;
using BoxOfficeGuesser.Stores;
using BoxOfficeGuesser.ViewModel;
using BoxOfficeGuesser.ViewModel.Commands;

namespace BoxOfficeGuesser.Factories;

public class StartGameCommandFactory : AbstractFactory<StartGameCommand>
{
    public StartGameCommandFactory(IServiceProvider provider) : base(provider)
    {
    }

    public override StartGameCommand Create(params object[] parameters)
    {
        ThrowParams<GameCreationViewModel>(parameters);

        return new StartGameCommand(
            (GameCreationViewModel) parameters[0], 
            provider.GetRequiredService<GameViewModelFactory>(),
            provider.GetRequiredService<NavigationStore>(), 
            provider.GetRequiredService<MovieStore>());
    }
}

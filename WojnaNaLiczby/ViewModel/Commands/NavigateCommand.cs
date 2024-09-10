using BoxOfficeGuesser.Stores;

namespace BoxOfficeGuesser.ViewModel.Commands;

public class NavigateCommand<T> : CommandBase where T : ViewModelBase
{
    private readonly NavigationStore navigationStore;

    public NavigateCommand(NavigationStore navigationStore)
    {
        this.navigationStore = navigationStore;
    }

    protected override void OnExecute(object? parameter)
    {
        navigationStore.NavigateTo<T>();
    }
}

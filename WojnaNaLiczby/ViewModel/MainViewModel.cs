
using BoxOfficeGuesser.Stores;

namespace BoxOfficeGuesser.ViewModel;

public class MainViewModel : ViewModelBase
{
    public ViewModelBase CurrentViewModel => navigationStore.CurrentViewModel!;
    
    private readonly NavigationStore navigationStore;

    public MainViewModel(NavigationStore navigationStore)
    {
        this.navigationStore = navigationStore;
        navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
    }

    private void OnCurrentViewModelChanged() => OnPropertyChanged(nameof(CurrentViewModel));
}

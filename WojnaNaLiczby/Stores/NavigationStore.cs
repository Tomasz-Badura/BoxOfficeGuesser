using BoxOfficeGuesser.ViewModel;

namespace BoxOfficeGuesser.Stores;

public class NavigationStore
{
    public event Action? CurrentViewModelChanged;

    private readonly Func<Type, ViewModelBase> vmFactory;

    private ViewModelBase? currentViewModel;
    public ViewModelBase? CurrentViewModel
    {
        get => currentViewModel;
        set
        {
            currentViewModel = value;
            CurrentViewModelChanged?.Invoke();
        }
    }

    public NavigationStore(Func<Type, ViewModelBase> vmFactory)
    {
        this.vmFactory = vmFactory;
    }

    public void NavigateTo<T>() where T : ViewModelBase
    {
        ViewModelBase vm = vmFactory.Invoke(typeof(T));
        CurrentViewModel = vm;
    }

}

using System.Windows;

using BoxOfficeGuesser.ViewModel;
using BoxOfficeGuesser.Windows;

namespace BoxOfficeGuesser.Stores;

public class NavigationStore
{
    public event Action? CurrentViewModelChanged;
    public ViewModelBase? CurrentViewModel
    {
        get => currentViewModel;
        set
        {
            currentViewModel = value;
            if(value is not null && WindowTarget is not null)
            {
                WindowTarget.MinWidth = value.WindowOptions.minWidth;
                WindowTarget.MinHeight = value.WindowOptions.minHeight;
                WindowTarget.Width = value.WindowOptions.defaultWidth;
                WindowTarget.Height = value.WindowOptions.defaultHeight;
                WindowTarget.ResizeMode = value.WindowOptions.resizeMode;
                WindowTarget.Focus();
            }

            CurrentViewModelChanged?.Invoke();
        }
    }

    public Window? WindowTarget { get; set; }

    private readonly Func<Type, ViewModelBase> vmFactory;
    private ViewModelBase? currentViewModel;

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

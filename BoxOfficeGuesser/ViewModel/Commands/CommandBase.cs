using System.Windows.Input;

namespace BoxOfficeGuesser.ViewModel.Commands;
public abstract class CommandBase : ICommand
{
    public event EventHandler? CanExecuteChanged;
    public event Action? Executed;

    public void Execute(object? parameter)
    {
        OnExecute(parameter);
        Executed?.Invoke();
    }

    public virtual bool CanExecute(object? parameter)
    {
        return true;
    }

    public virtual void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnExecute(object? parameter)
    {

    }
}

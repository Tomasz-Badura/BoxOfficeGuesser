using BoxOfficeGuesser.Model;

using System.ComponentModel;
using System.Windows;

namespace BoxOfficeGuesser.ViewModel;

public class ViewModelBase : INotifyPropertyChanged
{
    public virtual WindowOptions WindowOptions { get; protected set; } = new()
    {
        defaultWidth = 400,
        defaultHeight = 300,
        minWidth = 300,
        minHeight = 200,
        maxWidth = int.MaxValue,
        maxHeight = int.MaxValue,
        resizeMode = ResizeMode.CanResizeWithGrip,
    };

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

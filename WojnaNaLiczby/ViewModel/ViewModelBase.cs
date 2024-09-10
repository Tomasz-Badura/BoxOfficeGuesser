using System.ComponentModel;
using System.Windows;

namespace BoxOfficeGuesser.ViewModel;
public struct WindowOptions
{
    public int defaultWidth;
    public int defaultHeight;
    public int minWidth;
    public int minHeight;
	public int maxWidth;
	public int maxHeight;
	public ResizeMode resizeMode;
}

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

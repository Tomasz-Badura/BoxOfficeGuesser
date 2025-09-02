using System.Windows;

namespace BoxOfficeGuesser.Model;
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
using System.Windows;

namespace BoxOfficeGuesser.ViewModel.Commands;

public class ShowTutorialCommand : CommandBase
{
    protected override void OnExecute(object? parameter)
    {
        _ = MessageBox.Show("When you start the game, you're presented with 2 buttons with movie titles. The left button also shows the worldwide box office income of that movie. Your job is to select which one of the two movies had a greater box office income.", "Tutorial", MessageBoxButton.OK, MessageBoxImage.Question);
    }
}

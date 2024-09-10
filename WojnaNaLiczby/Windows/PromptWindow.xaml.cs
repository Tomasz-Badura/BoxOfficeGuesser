using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BoxOfficeGuesser.Windows;

public enum PromptWindowResult
{
    Cancel,
    Ok,
}

public enum PromptValidatorType
{
    Result,
    Override
}

public struct PromptWindowEventArgs
{
    public PromptWindowResult result;
    public string? value;
}

public partial class PromptWindow : Window
{
    public event EventHandler<PromptWindowEventArgs>? WindowClosed;
    private readonly Func<string, bool>? validator;
    private readonly PromptValidatorType validatorType;
    private string previousTextBoxValue = "";

    public PromptWindow(string? promptMessage = null, Func<string, bool>? validator = null, PromptValidatorType validatorType = PromptValidatorType.Result)
    {
        this.validator = validator;
        this.validatorType = validatorType;
        InitializeComponent();

        if(promptMessage is not null)
        {
            PromptText.Content = promptMessage;
        }

        TextChangedHandler(InputTextBox, null);
    }

    public async static Task<PromptWindowEventArgs> OpenPromptWindow(Window? parent = null, string? promptMessage = null, Func<string, bool>? validator = null, PromptValidatorType validatorType = PromptValidatorType.Result)
    {
        PromptWindow promptWindow = new(promptMessage, validator, validatorType);
        TaskCompletionSource<PromptWindowEventArgs> tcs = new();
        EventHandler<PromptWindowEventArgs> onWindowClosed = (_, result) => tcs.SetResult(result);
        
        if(parent is not null)
        {
            parent.Closed += (_, _) =>
            {
                promptWindow.WindowClosed -= onWindowClosed;
                promptWindow.CloseWindow(PromptWindowResult.Cancel);
            };
        }

        promptWindow.WindowClosed += onWindowClosed;
        promptWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        promptWindow.Show();
        promptWindow.FocusTextBox();
        return await tcs.Task;
    }

    public void FocusTextBox()
    {
        InputTextBox.Focus();
    }

    public void CloseWindow(PromptWindowResult result)
    {
        WindowClosed?.Invoke(this, new PromptWindowEventArgs() { result = result, value = InputTextBox.Text });
        Close();
    }

    private void OkButtonClick(object sender, RoutedEventArgs e)
    {
        WindowClosed?.Invoke(this, new PromptWindowEventArgs() { result = PromptWindowResult.Ok, value = InputTextBox.Text });
        Close();
    }

    private void CancelButtonClick(object sender, RoutedEventArgs e)
    {
        WindowClosed?.Invoke(this, new PromptWindowEventArgs() { result = PromptWindowResult.Cancel, value = InputTextBox.Text });
        Close();
    }

    private void TextChangedHandler(object sender, TextChangedEventArgs? e)
    {
        if(validator is null)
        {
            return;
        }

        TextBox textBox = sender as TextBox ?? throw new ArgumentException("TextChangedHandler recieved a sender that is not a TextBox", nameof(sender));
        string currentTextBoxValue = textBox.Text;

        switch(validatorType)
        {
            case PromptValidatorType.Result:
            {
                AcceptButton.IsEnabled = validator.Invoke(currentTextBoxValue);
                break;
            }
            case PromptValidatorType.Override:
            {
                if(validator.Invoke(currentTextBoxValue) == true)
                {
                    AcceptButton.IsEnabled = true;
                    previousTextBoxValue = currentTextBoxValue;
                    break;
                }

                if(string.IsNullOrEmpty(currentTextBoxValue) == false)
                {
                    textBox.Text = previousTextBoxValue;
                    return;
                }

                textBox.Text = "";
                AcceptButton.IsEnabled = false;
                return;   
            }
            default:
            {
                throw new NotImplementedException();
            }
        }
    }

    private void TextBoxEnterHandler(object sender, KeyEventArgs e)
    {
        if(e.Key == Key.Enter)
        {
            WindowClosed?.Invoke(this, new PromptWindowEventArgs() { result = PromptWindowResult.Ok, value = InputTextBox.Text });
            Close();
        }
    }
}

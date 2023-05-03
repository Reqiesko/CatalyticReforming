using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using Wpf.Ui.Controls;

using Button = Wpf.Ui.Controls.Button;



namespace CatalyticReforming.Services;

public class MessageBoxService
{

    public Task<MessageBoxResult> Show(string messageBoxText)
    {
        return Show(window: null, messageBoxText: messageBoxText);
    }

    public Task<MessageBoxResult> Show(string messageBoxText, string caption)
    {
        return Show(null, messageBoxText, caption);
    }

    public Task<MessageBoxResult> Show(string messageBoxText, string caption, MessageBoxButton button)
    {
        return Show(null, messageBoxText, caption, button);
    }

    public Task<MessageBoxResult> Show(string messageBoxText, MessageBoxButton button)
    {
        return Show(messageBoxText, "", button);
    }

    public Task<MessageBoxResult> Show(Window window, string messageBoxText)
    {
        return Show(window, messageBoxText, null);
    }

    public Task<MessageBoxResult> Show(Window window, string messageBoxText, string caption)
    {
        return Show(null, messageBoxText, caption, MessageBoxButton.OK);
    }

    public Task<MessageBoxResult> Show(Window window, string messageBoxText, string caption, MessageBoxButton button)
    {
        var tcs = new TaskCompletionSource<MessageBoxResult>();
        var mb = new Wpf.Ui.Controls.MessageBox();
        mb.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        mb.Owner = window;
        mb.Title = caption;
        mb.Content = messageBoxText;
        var result = MessageBoxResult.None;

        var footer = new StackPanel
            {HorizontalAlignment = HorizontalAlignment.Right, Orientation = Orientation.Horizontal};

        void OnButtonClicked(MessageBoxResult buttonResult)
        {
            result = buttonResult;
            mb.Close();
            tcs.SetResult(result);
        }

        var OkButton = new Button
            {Content = "ОК", Appearance = ControlAppearance.Primary, Margin = new Thickness(5, 5, 5, 5),};

        var CancelButton = new Button
            {Content = "Отмена", Margin = new Thickness(5, 5, 5, 5),};

        var YesButton = new Button
            {Content = "Да", Margin = new Thickness(5, 5, 5, 5),};

        var NoButton = new Button
            {Content = "Нет", Margin = new Thickness(5, 5, 5, 5),};

        OkButton.Click += (sender, args) => OnButtonClicked(MessageBoxResult.OK);
        CancelButton.Click += (sender, args) => OnButtonClicked(MessageBoxResult.Cancel);
        YesButton.Click += (sender, args) => OnButtonClicked(MessageBoxResult.Yes);
        NoButton.Click += (sender, args) => OnButtonClicked(MessageBoxResult.No);

        switch (button)
        {
            case MessageBoxButton.OK:
                footer.Children.Add(OkButton);

                break;
            case MessageBoxButton.OKCancel:
                footer.Children.Add(CancelButton);
                footer.Children.Add(OkButton);

                break;
            case MessageBoxButton.YesNoCancel:
                footer.Children.Add(CancelButton);
                footer.Children.Add(NoButton);
                footer.Children.Add(YesButton);

                break;
            case MessageBoxButton.YesNo:
                footer.Children.Add(NoButton);
                footer.Children.Add(YesButton);

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(button), button, null);
        }

        mb.Footer = footer;
        mb.Show();

        return tcs.Task;
    }
}

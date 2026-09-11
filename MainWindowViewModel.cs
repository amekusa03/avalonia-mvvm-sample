using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SampleApp;

// Base class implementing the mechanism to notify the View of changes from the ViewModel
public abstract class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

// Simple command class to handle actions like button clicks
public class DelegateCommand : ICommand
{
    private readonly Action _execute;
    private readonly Func<bool>? _canExecute;

    public event EventHandler? CanExecuteChanged;

    public DelegateCommand(Action execute, Func<bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;
    public void Execute(object? parameter) => _execute();
    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}

public class MainWindowViewModel : ViewModelBase
{
    private string _userName = string.Empty;
    private string _greeting = "名前を入力してボタンを押してください。";

    // Property bound to the TextBox in the View
    public string UserName
    {
        get => _userName;
        set
        {
            _userName = value;
            OnPropertyChanged();
            // Notify that the command's executable state may have changed when UserName changes
            (GreetCommand as DelegateCommand)?.RaiseCanExecuteChanged();
        }
    }

    // Property bound to the TextBlock in the View
    public string Greeting
    {
        get => _greeting;
        private set // Use private set to prevent modification directly from the View
        {
            _greeting = value;
            OnPropertyChanged();
        }
    }

    // Command bound to the Button in the View
    public ICommand GreetCommand { get; }

    public MainWindowViewModel()
    {
        GreetCommand = new DelegateCommand(
            execute: () => Greeting = $"こんにちは、{UserName} さん！",
            canExecute: () => !string.IsNullOrWhiteSpace(UserName)
        );
    }
}

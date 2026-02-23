using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SampleApp;

// ViewModelからViewへ変更を通知するための仕組みを実装したベースクラス
public abstract class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

// ボタンのクリックなどを処理するシンプルなコマンドクラス
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

    // ViewのTextBoxにバインドするプロパティ
    public string UserName
    {
        get => _userName;
        set
        {
            _userName = value;
            OnPropertyChanged();
            // UserNameが変更されたらコマンドの実行可否も変わる可能性があるため通知する
            (GreetCommand as DelegateCommand)?.RaiseCanExecuteChanged();
        }
    }

    // ViewのTextBlockにバインドするプロパティ
    public string Greeting
    {
        get => _greeting;
        private set // Viewから変更されないようにprivate setにする
        {
            _greeting = value;
            OnPropertyChanged();
        }
    }

    // ViewのButtonにバインドするコマンド
    public ICommand GreetCommand { get; }

    public MainWindowViewModel()
    {
        GreetCommand = new DelegateCommand(
            execute: () => Greeting = $"こんにちは、{UserName} さん！",
            canExecute: () => !string.IsNullOrWhiteSpace(UserName)
        );
    }
}
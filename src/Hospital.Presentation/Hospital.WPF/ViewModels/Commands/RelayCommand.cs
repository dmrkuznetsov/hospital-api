using System.Windows.Input;

namespace Hospital.WPF.ViewModels.Commands;

internal class RelayCommand : ICommand
{
    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }
    private Action<object> _execute;
    private Func<object, bool> _canExecute;
    public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
    {
        _execute = execute;
        _canExecute = canExecute;
    }
    public bool CanExecute(object parameter)
    {
        return _canExecute is null || _canExecute(parameter);
    }

    public void Execute(object parameter)
    {
        _execute(parameter);
    }
}
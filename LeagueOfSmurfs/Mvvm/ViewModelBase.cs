using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace LeagueOfSmurfs.Mvvm
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void Raise([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected bool Set<T>(ref T field, T value, [CallerMemberName] string name = null)
        {
            if (Equals(field, value))
                return false;
            field = value;
            Raise(name);
            return true;
        }
    }

    public sealed class RelayCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;

        public RelayCommand(Action execute)
            : this(_ => execute(), null)
        {
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }

    public sealed class AsyncRelayCommand : ICommand
    {
        private readonly Func<object, System.Threading.Tasks.Task> execute;
        private readonly Func<object, bool> canExecute;
        private bool isRunning;

        public AsyncRelayCommand(Func<System.Threading.Tasks.Task> execute)
            : this(_ => execute(), null)
        {
        }

        public AsyncRelayCommand(Func<object, System.Threading.Tasks.Task> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return !isRunning && (canExecute == null || canExecute(parameter));
        }

        public async void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;
            try
            {
                isRunning = true;
                CommandManager.InvalidateRequerySuggested();
                await execute(parameter);
            }
            finally
            {
                isRunning = false;
                CommandManager.InvalidateRequerySuggested();
            }
        }
    }
}

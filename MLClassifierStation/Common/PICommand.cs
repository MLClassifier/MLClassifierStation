using System;
using System.Windows.Input;

namespace MLClassifierStation.Common
{
    // https://stackoverflow.com/a/5599770
    public class FLCommand : ICommand
    {
        private readonly Action<object> execute = null;
        private readonly Predicate<object> canExecute = null;

        #region Constructors

        public FLCommand(Action<object> execute)
            : this(execute, null) { }

        public FLCommand(Action<object> execute, Predicate<object> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        #endregion Constructors

        #region ICommand Members

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return canExecute != null ? canExecute(parameter) : true;
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }

        public void OnCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        #endregion ICommand Members
    }
}
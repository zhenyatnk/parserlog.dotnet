using System;
using System.Windows.Input;
using System.Threading;

namespace parserlog.dotnet.ui.view_model.command
{
	public class CancelCommand
		: ICommand
	{
        public CancelCommand(CancellationTokenSource _cancelCommand)
        {
            cancelCommand = _cancelCommand;
        }

        public void Execute(object parameter)
		{
            cancelCommand.Cancel();
		}

		public bool CanExecute(object parameter)
		{
			return cancelCommand != null;
		}
		
		public event EventHandler CanExecuteChanged;

        private CancellationTokenSource cancelCommand;
    }
}

using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace parserlog.dotnet.ui.view_model.command
{
	public class OpenFileCommand
		: ICommand
	{
        public OpenFileCommand(ViewModel _model_view)
        {
            model_view = _model_view;
        }

        public void Execute(object parameter)
		{
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Log name|*.log";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (var operation in model_view.Operations)
                    operation.CancelCommand.Execute(null);
                model_view.LogName = dialog.FileName;
                model_view.ParseLogCommand.Execute(null);                
            }
        }
		public bool CanExecute(object parameter)
		{
			return true;
		}

        public event EventHandler CanExecuteChanged;
        private ViewModel model_view;
    }
}

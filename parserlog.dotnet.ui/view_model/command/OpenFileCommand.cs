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
                model_view.LogName = dialog.FileName;

                var cancel = new CancellationTokenSource();
                core.utilities.Wrapped<long> count = new core.utilities.Wrapped<long>(0);
                parserlog.dotnet.core.interfaces.IOperation count_lines = new parserlog.dotnet.core.operations.OperationCountLines(model_view.LogName, cancel.Token, count);

                OperationView operation_view = new OperationView(model_view.Operations, cancel);
                count_lines.OnStart += (sender, e) => { operation_view.Title = "Starting calculate count";  };
                count_lines.OnProgress += (sender, e) => { operation_view.Progress = e.Progress; };
                count_lines.OnError += (sender, e) => { operation_view.Complete = true; };
                count_lines.OnComplete += (sender, e) => 
                {
                    operation_view.Complete = true;
                    model_view.DataChart.Add(new ChartElement() { Name = model_view.LogName, Count = (int)count.value});
                };

                count_lines.ExecuteAsync();
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

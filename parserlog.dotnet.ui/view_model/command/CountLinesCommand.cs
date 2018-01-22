using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace parserlog.dotnet.ui.view_model.command
{
    public class CountLinesCommand
        : ICommand
    {
        public CountLinesCommand(ViewModel _model_view)
        {
            model_view = _model_view;
        }

        public void Execute(object parameter)
        {
            var cancel = new CancellationTokenSource();
            core.utilities.Wrapped<long> count = new core.utilities.Wrapped<long>(0);
            parserlog.dotnet.core.interfaces.IOperation count_lines = new parserlog.dotnet.core.operations.OperationCountLines(model_view.LogName, cancel.Token, count);

            OperationView operation_view = new OperationView(model_view.Operations, cancel);
            count_lines.OnStart += (sender, e) => { operation_view.Title = "Starting calculate count"; };
            count_lines.OnProgress += (sender, e) => { operation_view.Progress = e.Progress; };
            count_lines.OnError += (sender, e) => { operation_view.Complete = true; };
            count_lines.OnComplete += (sender, e) =>
            {
                operation_view.Complete = true;
            };

            count_lines.ExecuteAsync();
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
        private ViewModel model_view;
    }
}

using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace parserlog.dotnet.ui.view_model.command
{
    public class AnalyzeThreadCommand
        : ICommand
    {
        public AnalyzeThreadCommand(ViewModel _model_view)
        {
            model_view = _model_view;
        }

        public void Execute(object parameter)
        {
            var threadId = ulong.Parse(model_view.ThreadID);

            if (model_view.Lines.ContainsKey(threadId))
            {
                var cancel = new CancellationTokenSource();
                var internal_chart = new ObservableCollectionDisp<ChartElement>();
                var next_time = new TimeSpan();
                var empty_time = new TimeSpan();
                var chart_element = new ChartElement();
                System.Timers.Timer timer = new System.Timers.Timer();
                if (timer != null)
                    timer.Stop();

                parserlog.dotnet.core.interfaces.IOperationParse analyze_thread = new parserlog.dotnet.core.operations.OperationGetThreadLines(model_view.Lines[ulong.Parse(model_view.ThreadID)], cancel.Token);

                OperationView operation_view = new OperationView(model_view.Operations, cancel);
                analyze_thread.OnStart += (sender, e) => { operation_view.Title = "Analyzing thread[" + threadId + "].."; };
                analyze_thread.OnProgress += (sender, e) => { operation_view.Progress = e.Progress; };
                analyze_thread.OnParsedLine += (sender, e) =>
                {
                    if(next_time == empty_time)
                    {
                        next_time = e.Info.m_TimeStamp.Add(new TimeSpan(0, 0, 10));
                        chart_element.Name = e.Info.m_TimeStamp.ToString(@"hh\:mm\:ss");
                    }
                    if (e.Info.m_TimeStamp > next_time)
                    {
                        next_time = e.Info.m_TimeStamp.Add(new TimeSpan(0, 0, 10));
                        internal_chart.Add(chart_element);
                        chart_element = new ChartElement();
                        chart_element.Name = e.Info.m_TimeStamp.ToString(@"hh\:mm\:ss");
                    }
                    ++chart_element.Count;
                };

                analyze_thread.OnError += (sender, e) => { operation_view.Complete = true; };
                analyze_thread.OnComplete += (sender, e) =>
                {
                    if (chart_element.Count > 0)
                        internal_chart.Add(chart_element);
                    model_view.ThreadChart = internal_chart;
                    operation_view.Complete = true;
                };
                analyze_thread.ExecuteAsync();
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

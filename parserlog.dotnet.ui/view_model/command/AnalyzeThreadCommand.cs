using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using LiveCharts.Wpf;
using LiveCharts;

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
            ExecuteAsync(parameter);
        }

        public async Task ExecuteAsync(object parameter)
        {
            var threadId = ulong.Parse(model_view.ThreadID);

            if (model_view.LinesByThread.ContainsKey(threadId))
            {
                var cancel = new CancellationTokenSource();
                var values = new ChartValues<double>();
                var next_time = new TimeSpan();
                var empty_time = new TimeSpan();
                var chart_element = new double();
                var end_time = new TimeSpan();
                if (model_view.Lines.Count > 0)
                {
                    next_time = model_view.Lines[0].m_TimeStamp;
                    end_time = model_view.Lines[model_view.Lines.Count - 1].m_TimeStamp;
                }
                int demesion = 1;

                parserlog.dotnet.core.interfaces.IOperationParse analyze_thread = new parserlog.dotnet.core.operations.OperationGetThreadLines(model_view.LinesByThread[ulong.Parse(model_view.ThreadID)], cancel.Token);

                OperationView operation_view = new OperationView(model_view.Operations, cancel);
                analyze_thread.OnStart += (sender, e) => 
                {
                    operation_view.Title = "Analyzing thread[" + threadId + "]...";
                    model_view.Series.Clear();
                };
                analyze_thread.OnProgress += (sender, e) => { operation_view.Progress = e.Progress; };
                analyze_thread.OnParsedLine += (sender, e) =>
                {
                    var info = e.Info;

                    if (info.m_TimeStamp > next_time)
                    {
                        var expl = info.m_TimeStamp - next_time;
                        if (demesion < expl.TotalSeconds && next_time != empty_time)
                        {
                            var count = expl.TotalSeconds / demesion;
                            while (count-- > 0)
                                values.Add(0);
                        }
                        next_time = info.m_TimeStamp.Add(new TimeSpan(0, 0, demesion));
                        values.Add(chart_element);
                        chart_element = 0;
                    }
                    ++chart_element;
                };

                analyze_thread.OnError += (sender, e) => { operation_view.Complete = true; };
                analyze_thread.OnComplete += (sender, e) =>
                {
                    if (chart_element > 0)
                        values.Add(chart_element);

                    if(next_time < end_time)
                    {
                        var expl = end_time - next_time;
                        if (demesion < expl.TotalSeconds)
                        {
                            var count = expl.TotalSeconds / demesion;
                            while (count-- > 0)
                                values.Add(0);
                        }
                    }

                    var series = new LineSeries
                    {
                        Values = values,
                        StrokeThickness = .5,
                        PointGeometry = null //use a null geometry when you have many series
                    };

                    model_view.Series.Add(series);
                    operation_view.Complete = true;
                };
                await analyze_thread.ExecuteAsync();
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

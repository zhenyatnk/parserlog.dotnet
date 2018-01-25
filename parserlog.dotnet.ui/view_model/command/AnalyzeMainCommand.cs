using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Input;
using System.Windows.Media;
using LiveCharts.Wpf;
using LiveCharts;

namespace parserlog.dotnet.ui.view_model.command
{
    public class AnalyzeMainCommand
        : ICommand
    {
        public AnalyzeMainCommand(ViewModel _model_view)
        {
            model_view = _model_view;
        }

        public void Execute(object parameter)
        {
            var cancel = new CancellationTokenSource();
            var values = new ChartValues<double>();
            var next_time = new TimeSpan();
            var empty_time = new TimeSpan();
            var chart_element = new double();
            int demesion = 1;

            parserlog.dotnet.core.interfaces.IOperationParse analyze_thread = new parserlog.dotnet.core.operations.OperationGetThreadLines(model_view.Lines, cancel.Token);

            OperationView operation_view = new OperationView(model_view.Operations, cancel);
            analyze_thread.OnStart += (sender, e) =>
            {
                operation_view.Title = "Analyzing file log...";
                model_view.SeriesMainPage.Clear();
            };
            analyze_thread.OnProgress += (sender, e) => { operation_view.Progress = e.Progress; };
            analyze_thread.OnParsedLine += (sender, e) =>
            {
                var info = e.Info;

                if (next_time == empty_time)
                {
                    next_time = info.m_TimeStamp.Add(new TimeSpan(0, 0, demesion));
                }
                if (info.m_TimeStamp > next_time)
                {
                    var expl = info.m_TimeStamp - next_time;
                    if(demesion < expl.TotalSeconds)
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

                var series = new LineSeries
                {
                    Values = values,
                    StrokeThickness = .5,
                    PointGeometry = null //use a null geometry when you have many series
                    };

                model_view.SeriesMainPage.Add(series);
                operation_view.Complete = true;
            };
            analyze_thread.ExecuteAsync();
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
        private ViewModel model_view;
    }
}

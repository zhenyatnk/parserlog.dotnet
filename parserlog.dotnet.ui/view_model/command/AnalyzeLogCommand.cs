using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace parserlog.dotnet.ui.view_model.command
{
    public class AnalyzeLogCommand
        : ICommand
    {
        public AnalyzeLogCommand(ViewModel _model_view)
        {
            model_view = _model_view;
        }

        public void Execute(object parameter)
        {
            var cancel = new CancellationTokenSource();
            parserlog.dotnet.core.interfaces.IOperationParse analyze_log = new parserlog.dotnet.core.operations.OperationParserLines(model_view.LogName, cancel.Token);

            System.Timers.Timer timer = new System.Timers.Timer();
            if (timer != null)
                timer.Stop();

            timer = new System.Timers.Timer(1500);
            timer.Elapsed += (source, e) =>
            {
                var i = 0;
                MultiSortedList<ulong, ulong> byCount = new MultiSortedList<ulong, ulong>(new CompareReverse<ulong>());
                foreach (var thread in model_view.ThreadsInfo)
                    byCount.Add(thread.Value.m_Count, thread.Key);

                model_view.DataChart.dispatcher.Invoke(new Action(() => { model_view.DataChart.Clear(); }));

                foreach (var thread in byCount)
                {
                    model_view.DataChart.dispatcher.Invoke(new Action(() => { model_view.DataChart.Add(new ChartElement() { Name = thread.Value.ToString(), Count = (int)thread.Key }); }));
                    if (i++ > 20)
                        break;
                }

            };
            timer.AutoReset = true;
            timer.Enabled = true;

            OperationView operation_view = new OperationView(model_view.Operations, cancel);
            analyze_log.OnStart += (sender, e) => { operation_view.Title = "Parsing log file.."; model_view.Lines = new MultiSortedList<ulong, core.model.LineInfo>(); };
            analyze_log.OnProgress += (sender, e) => { operation_view.Progress = e.Progress; };
            analyze_log.OnParsedLine += (sender, e) =>
            {
                model_view.Lines.Add(e.Info.m_ThreadId, e.Info);
            };
            analyze_log.OnError += (sender, e) => { operation_view.Complete = true; };
            analyze_log.OnComplete += (sender, e) =>
            {
                timer.Stop();
                var i = 0;
                operation_view.Complete = true;
                SortedList<ulong, ulong> sumThread = new SortedList<ulong, ulong>();
                foreach (var thread in model_view.Lines)
                {
                    if(sumThread.ContainsKey(thread.Key))
                        ++sumThread[thread.Key];
                    else
                        sumThread.Add(thread.Key, 1);
                }

                MultiSortedList<ulong, ulong> byCount = new MultiSortedList<ulong, ulong>(new CompareReverse<ulong>());
                model_view.Threads.Clear();
                foreach (var thread in sumThread)
                {
                    byCount.Add(thread.Value, thread.Key);
                }

                foreach (var thread in byCount)
                {
                    if (i == 0)
                        model_view.ThreadID = thread.Value.ToString();
                    if (i++ < 20)
                        model_view.DataChart.Add(new ChartElement() { Name = thread.Value.ToString(), Count = (int)thread.Key });

                    model_view.Threads.Add(thread.Value);
                }

            };

            analyze_log.ExecuteAsync();
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
        private ViewModel model_view;
    }
}

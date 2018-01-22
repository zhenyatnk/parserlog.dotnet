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
                foreach (var thread in model_view.Threads)
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
            analyze_log.OnStart += (sender, e) => { operation_view.Title = "Analyzing oog file"; };
            analyze_log.OnProgress += (sender, e) => { operation_view.Progress = e.Progress; };
            analyze_log.OnParsedLine += (sender, e) => 
            {
                if (!model_view.Threads.ContainsKey(e.Info.m_ThreadId))
                {
                    core.model.ThreadInfo thread = new core.model.ThreadInfo()
                    {
                        m_Id = e.Info.m_ThreadId,
                        m_TimeStart = e.Info.m_TimeStamp,
                        m_TimeEnd = e.Info.m_TimeStamp,
                        m_Count = 1,
                        m_Component = new SortedList<string, ulong>()
                    };
                    thread.m_Component.Add(e.Info.m_Component, 1);

                    model_view.Threads.Add(e.Info.m_ThreadId, thread);
                }
                else
                {
                    var thread = model_view.Threads[e.Info.m_ThreadId];
                    ++thread.m_Count;
                    thread.m_TimeEnd = e.Info.m_TimeStamp;
                    if(!thread.m_Component.ContainsKey(e.Info.m_Component))
                        thread.m_Component.Add(e.Info.m_Component, 1);
                    else
                        ++thread.m_Component[e.Info.m_Component];
                }
            };
            analyze_log.OnError += (sender, e) => { operation_view.Complete = true; };
            analyze_log.OnComplete += (sender, e) =>
            {
                timer.Stop();
                var i = 0;
                operation_view.Complete = true;
                MultiSortedList<ulong, ulong> byCount = new MultiSortedList<ulong, ulong>(new CompareReverse<ulong>());
                foreach (var thread in model_view.Threads)
                    byCount.Add(thread.Value.m_Count, thread.Key);

                    foreach (var thread in byCount)
                {
                    model_view.DataChart.Add(new ChartElement() { Name = thread.Value.ToString(), Count = (int)thread.Key });
                    if (i++ > 20)
                        break;
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

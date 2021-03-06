﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace parserlog.dotnet.ui.view_model.command
{
    public class ParseLogCommand
        : ICommand
    {
        public ParseLogCommand(ViewModel _model_view)
        {
            model_view = _model_view;
        }

        public void Execute(object parameter)
        {
            ExecuteAsync(parameter);
        }

        public async Task ExecuteAsync(object parameter)
        {
            var cancel = new CancellationTokenSource();
            parserlog.dotnet.core.interfaces.IOperationParse analyze_log = new parserlog.dotnet.core.operations.OperationParserLines(model_view.LogName, cancel.Token);

            OperationView operation_view = new OperationView(model_view.Operations, cancel);
            analyze_log.OnStart += (sender, e) =>
            {
                operation_view.Title = "Parsing log file...";
                model_view.LinesByThread = new MultiSortedList<ulong, core.model.LineInfo>();
                model_view.Lines.Clear();
                model_view.Threads.Clear();
                model_view.Series.Clear();
                model_view.SeriesMainPage.Clear();
            };
            analyze_log.OnProgress += (sender, e) => { operation_view.Progress = e.Progress; };
            analyze_log.OnParsedLine += (sender, e) =>
            {
                model_view.LinesByThread.Add(e.Info.m_ThreadId, e.Info);
                model_view.Lines.Add(e.Info);
            };
            analyze_log.OnError += (sender, e) => { operation_view.Complete = true; };
            analyze_log.OnComplete += (sender, e) =>
            {
                operation_view.Complete = true;
                SortedList<ulong, ulong> sumThread = new SortedList<ulong, ulong>();
                foreach (var thread in model_view.LinesByThread)
                {
                    if(sumThread.ContainsKey(thread.Key))
                        ++sumThread[thread.Key];
                    else
                        sumThread.Add(thread.Key, 1);
                }

                MultiSortedList<ulong, ulong> byCount = new MultiSortedList<ulong, ulong>(new CompareReverse<ulong>());
                model_view.Threads.Clear();
                foreach (var thread in sumThread)
                    byCount.Add(thread.Value, thread.Key);

                foreach (var thread in byCount)
                    model_view.Threads.Add(thread.Value);
            };

            await analyze_log.ExecuteAsync();
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
        private ViewModel model_view;
    }
}

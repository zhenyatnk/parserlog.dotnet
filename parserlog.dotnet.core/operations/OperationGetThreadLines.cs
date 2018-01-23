using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using System;
using System.IO;

namespace parserlog.dotnet.core.operations
{
    public class OperationGetThreadLines
        : interfaces.IOperationParse
    {

        public OperationGetThreadLines(List<model.LineInfo> aLinesThread, CancellationToken aCancel)
        {
            linesThread = aLinesThread;
            cancel = aCancel;
        }

        public async Task ExecuteAsync()
        {
            OnStart(this, new EventArgs());
            await Execute();
            OnComplete(this, new EventArgs());
        }

        private Task Execute()
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var count_lines = linesThread.Count;
                    var index = 0;
                    foreach (var line in linesThread)
                    {
                        ++index;
                        OnParsedLine(this, new interfaces.OnParsedLineEventArgs() { Info = line });
                        OnProgress(this, new interfaces.OnProgressEventArgs() { Progress = (100.0 * index) / count_lines });
                    }
                }
                catch (SystemException e)
                {
                    OnError(this, new interfaces.OnErrorEventArgs() { Message = e.Message });
                }
            });
        }

        public event EventHandler OnStart;
        public event EventHandler<interfaces.OnProgressEventArgs> OnProgress;
        public event EventHandler<interfaces.OnErrorEventArgs> OnError;
        public event EventHandler<interfaces.OnParsedLineEventArgs> OnParsedLine;
        public event EventHandler OnComplete;

        private CancellationToken cancel;
        private List<model.LineInfo> linesThread;
    }
}

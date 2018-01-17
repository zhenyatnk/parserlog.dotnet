using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.IO;

namespace parserlog.dotnet.core.operations
{
    public class OperationCountLines
        : interfaces.IOperation
    {

        public OperationCountLines(String aFilename, CancellationToken aCancel, utilities.Wrapped<long> aCountlines)

        {
            cancel = aCancel;
            fileinfo = new System.IO.FileInfo(aFilename);
            countlines = aCountlines;
        }

        public async Task ExecuteAsync()
        {
            countlines.value = 0;
            OnStart(this, new EventArgs());
            await Execute();
            OnComplete(this, new EventArgs());
        }

        private Task Execute()
        {
            countlines.value = 0;
            return Task.Factory.StartNew(() =>
            {
                if (!fileinfo.Exists)
                    OnError(this, new interfaces.OnErrorEventArgs() { Message = "File not exist. Filename='" + fileinfo.FullName + "'" });
                else
                {
                    using (StreamReader reader = new StreamReader(fileinfo.FullName))
                    {
                        long size_file = fileinfo.Length;
                        long count_bytes = 0;
                        string line = "";
                        while ((line = reader.ReadLine()) != null && !cancel.IsCancellationRequested)
                        {
                            count_bytes += line.Length;
                            ++countlines.value;
                            OnProgress(this, new interfaces.OnProgressEventArgs() { Progress = (100.0 * count_bytes) / size_file });
                        }
                    }
                }
            });
        }

        public event EventHandler OnStart;
        public event EventHandler<interfaces.OnProgressEventArgs> OnProgress;
        public event EventHandler<interfaces.OnErrorEventArgs> OnError;
        public event EventHandler OnComplete;

        private CancellationToken cancel;
        private System.IO.FileInfo fileinfo;
        private utilities.Wrapped<long> countlines;
    }
}

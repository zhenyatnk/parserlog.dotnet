using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using System;
using System.IO;

namespace parserlog.dotnet.core.operations
{
    public class OperationParserLines
        : interfaces.IOperationParse
    {

        public OperationParserLines(String aFilename, CancellationToken aCancel)
        {
            filename = aFilename;
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
                    var fileinfo = new System.IO.FileInfo(filename);
                    if (!fileinfo.Exists)
                        OnError(this, new interfaces.OnErrorEventArgs() { Message = "File not exist. Filename='" + fileinfo.FullName + "'" });
                    else
                    {
                        using (FileStream file = new FileStream(fileinfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
                        using (StreamReader reader = new StreamReader(file))
                        {
                            long size_file = fileinfo.Length;
                            long count_bytes = 0;
                            string line = "";
                            while ((line = reader.ReadLine()) != null && !cancel.IsCancellationRequested)
                            {
                                count_bytes += line.Length;
                                IList<string> items = SplitToMax(line, '\t', 4);
                                if (IsLineInfo(items))
                                    OnParsedLine(this, new interfaces.OnParsedLineEventArgs() { Info = ParseToLineInfo(items) });
                                OnProgress(this, new interfaces.OnProgressEventArgs() { Progress = (100.0 * count_bytes) / size_file });
                            }
                        }
                    }
                }
                catch (SystemException e)
                {
                    OnError(this, new interfaces.OnErrorEventArgs() { Message = e.Message});
                }
            });
        }

        private model.LineInfo ParseToLineInfo(IList<string> items)
        {
            model.LineInfo info = new model.LineInfo();
            var lenght = items.Count;
            if (lenght > 0)
                info.m_TimeStamp = TimeSpan.Parse(items[0]);
            if (lenght > 1)
                info.m_ThreadId = UInt64.Parse(items[1].Substring(2), System.Globalization.NumberStyles.HexNumber);
            if (lenght > 2)
                info.m_Type = items[2];
            if (lenght > 3)
                info.m_Component = items[3];
            return info;
        }

        private bool IsLineInfo(IList<string> items)
        {
            return items.Count > 2 && items[1].StartsWith("0x");
        }

        private IList<string> SplitToMax(string line, char delimeter, int count)
        {
            int index = 0;
            IList<string> container = new List<string>();
            StringBuilder builder = new StringBuilder();
            foreach (var element in line)
            {
                if (element == delimeter)
                {
                    container.Add(builder.ToString());
                    if (++index >= count)
                        break;
                    builder.Clear();
                }
                else
                    builder.Append(element);
            }
            return container;
        }

        public event EventHandler OnStart;
        public event EventHandler<interfaces.OnProgressEventArgs> OnProgress;
        public event EventHandler<interfaces.OnErrorEventArgs> OnError;
        public event EventHandler<interfaces.OnParsedLineEventArgs> OnParsedLine;
        public event EventHandler OnComplete;

        private CancellationToken cancel;
        private String filename;
    }
}

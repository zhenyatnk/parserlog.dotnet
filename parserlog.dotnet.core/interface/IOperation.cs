using System;
using System.Threading.Tasks;

namespace parserlog.dotnet.core.interfaces
{
    public class OnErrorEventArgs : EventArgs
    {
        public String Message { get; set; }
    }

    public class OnProgressEventArgs : EventArgs
    {
        public double Progress { get; set; }
    }

    public interface IOperation
    {
        Task ExecuteAsync();

        //events
        event EventHandler OnStart;
        event EventHandler<OnProgressEventArgs> OnProgress;
        event EventHandler<OnErrorEventArgs> OnError;
        event EventHandler OnComplete;
    }

    public class OnParsedLineEventArgs : EventArgs
    {
        public double Progress { get; set; }
    }
    public interface IOperationParse
        : IOperation
    {
        //events
        event EventHandler<OnParsedLineEventArgs> OnParsedLine;
    }
}

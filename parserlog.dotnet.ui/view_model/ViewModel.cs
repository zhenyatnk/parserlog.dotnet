using System.Threading;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using LiveCharts;

namespace parserlog.dotnet.ui.view_model
{
    public class ViewModel
        :Notifier
    {
        public ViewModel()
        {
            log_name = "";
            OpenFileCommand = new command.OpenFileCommand(this);
            ParseLogCommand = new command.ParseLogCommand(this);
            AnalyzeThreadCommand = new command.AnalyzeThreadCommand(this);
            AnalyzeMainCommand = new command.AnalyzeMainCommand(this);
            CountLinesCommand = new command.CountLinesCommand(this);

            threads = new List<ulong>();
            Operations = new ObservableCollectionDisp<OperationView>();

            Series = new SeriesCollection();
            SeriesMainPage = new SeriesCollection();
            linesByThread = new MultiSortedList<ulong, core.model.LineInfo>();
            lines = new List<core.model.LineInfo>();
        }

        public string LogName
        {
            get
            {
                return log_name;
            }
            set
            {
                log_name = value;
                NotifyPropertyChanged("LogName");
            }
        }
        public string ThreadID
        {
            get
            {
                return thread_id;
            }
            set
            {
                thread_id = value;
                NotifyPropertyChanged("ThreadID");
                AnalyzeThreadCommand.Execute(null);
            }
        }

        public command.OpenFileCommand OpenFileCommand { get; set; }
        public command.CountLinesCommand CountLinesCommand { get; set; }
        public command.ParseLogCommand ParseLogCommand { get; set; }
        public command.AnalyzeThreadCommand AnalyzeThreadCommand { get; set; }
        public command.AnalyzeMainCommand AnalyzeMainCommand { get; set; }

        public ObservableCollectionDisp<OperationView> Operations
        {
            get
            {
                return operations;
            }
            set
            {
                operations = value;
                NotifyPropertyChanged("Operations");
            }
        }

        public MultiSortedList<ulong, core.model.LineInfo> LinesByThread
        {
            get
            {
                return linesByThread;
            }
            set
            {
                linesByThread = value;
                NotifyPropertyChanged("LinesByThread");
            }
        }

        public List<core.model.LineInfo> Lines
        {
            get
            {
                return lines;
            }
            set
            {
                lines = value;
                NotifyPropertyChanged("Lines");
            }
        }

        public List<ulong> Threads
        {
            get
            {
                return threads;
            }
            set
            {
                threads = value;
                NotifyPropertyChanged("Threads");
            }
        }

        public SeriesCollection Series { get; set; }
        public SeriesCollection SeriesMainPage { get; set; }

        private ObservableCollectionDisp<OperationView> operations;
        private MultiSortedList<ulong, core.model.LineInfo> linesByThread;
        private List<core.model.LineInfo> lines;
        private List<ulong> threads;
        private string log_name;
        private string thread_id;

    }
}

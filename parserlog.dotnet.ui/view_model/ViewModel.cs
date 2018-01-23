using System.Threading;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace parserlog.dotnet.ui.view_model
{
    public class ViewModel
        :Notifier
    {
        public ViewModel()
        {
            log_name = "";
            OpenFileCommand = new command.OpenFileCommand(this);
            AnalyzeLogCommand = new command.AnalyzeLogCommand(this);
            AnalyzeThreadCommand = new command.AnalyzeThreadCommand(this);
            CountLinesCommand = new command.CountLinesCommand(this);

            data_chart = new ObservableCollectionDisp<ChartElement>();
            thread_chart = new ObservableCollectionDisp<ChartElement>();
            Operations = new ObservableCollectionDisp<OperationView>();

            lines = new MultiSortedList<ulong, core.model.LineInfo>();
            threads = new SortedList<ulong, core.model.ThreadInfo>();
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
        public command.AnalyzeLogCommand AnalyzeLogCommand { get; set; }
        public command.AnalyzeThreadCommand AnalyzeThreadCommand { get; set; }
        public ObservableCollectionDisp<ChartElement> DataChart
        {
            get
            {
                return data_chart;
            }
        }

        public ObservableCollectionDisp<ChartElement> ThreadChart
        {
            get
            {
                return thread_chart;
            }
            set
            {
                thread_chart = value;
                NotifyPropertyChanged("ThreadChart");
            }
        }

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

        public MultiSortedList<ulong, core.model.LineInfo> Lines
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

        public SortedList<ulong, core.model.ThreadInfo> Threads
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

        private ObservableCollectionDisp<OperationView> operations;
        private MultiSortedList<ulong, core.model.LineInfo> lines;
        private SortedList<ulong, core.model.ThreadInfo> threads;
        private readonly ObservableCollectionDisp<ChartElement> data_chart;
        private ObservableCollectionDisp<ChartElement> thread_chart;
        private string log_name;
        private string thread_id;

    }
}

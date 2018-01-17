using System.Threading;
using System.Collections.ObjectModel;

namespace parserlog.dotnet.ui.view_model
{
    public class ViewModel
        :Notifier
    {
        public ViewModel()
        {
            log_name = "";
            OpenFileCommand = new command.OpenFileCommand(this);
            data_chart = new ObservableCollection<ChartElement>();
            Operations = new ObservableCollectionDisp<OperationView>();
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

        public command.OpenFileCommand OpenFileCommand { get; set; }
        public ObservableCollection<ChartElement> DataChart
        {
            get
            {
                return data_chart;
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

        private ObservableCollectionDisp<OperationView> operations;
        private readonly ObservableCollection<ChartElement> data_chart;
        private string log_name;
    }
}

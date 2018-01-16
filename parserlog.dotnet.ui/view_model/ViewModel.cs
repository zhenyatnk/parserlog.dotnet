using System.Threading;

namespace parserlog.dotnet.ui.view_model
{
    public class ViewModel
        :Notifier
    {
        public ViewModel()
        {
            OpenFileCommand = new command.OpenFileCommand(this);
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

        private string log_name;
    }
}

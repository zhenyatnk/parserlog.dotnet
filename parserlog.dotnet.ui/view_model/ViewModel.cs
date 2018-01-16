﻿using System.Threading;
using System.Collections.ObjectModel;

namespace parserlog.dotnet.ui.view_model
{
    public class ViewModel
        :Notifier
    {
        public ViewModel()
        {
            OpenFileCommand = new command.OpenFileCommand(this);
            data_chart = new ObservableCollection<ChartElement>();
            data_chart.Add(new ChartElement() { Name = "China", Count = 1340 });
            data_chart.Add(new ChartElement() { Name = "India", Count = 1220 });
            data_chart.Add(new ChartElement() { Name = "United States", Count = 309 });
            data_chart.Add(new ChartElement() { Name = "Indonesia", Count = 240 });

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


        private readonly ObservableCollection<ChartElement> data_chart;
        private string log_name;
    }
}

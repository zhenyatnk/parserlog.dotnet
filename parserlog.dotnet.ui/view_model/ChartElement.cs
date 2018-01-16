using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace parserlog.dotnet.ui.view_model
{
    public class ChartElement
        : Notifier
    {

        private string _name = string.Empty;
        private int _count = 0;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        public int Count
        {
            get
            {
                return _count;
            }
            set
            {
                _count = value;
                NotifyPropertyChanged("Count");
            }
        }
    }
}

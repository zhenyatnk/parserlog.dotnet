using System;
using System.Threading;

namespace parserlog.dotnet.ui.view_model
{
    public class OperationView
        : Notifier
    {
        public OperationView(ObservableCollectionDisp<OperationView> _items, CancellationTokenSource _cancel)
        {
            progress = 0.0;
            title = "";
            items = _items;
            complete = false;
            CancelCommand = new command.CancelCommand(_cancel);
            AddToItems();
        }

        public double Progress
        {
            get { return progress; }
            set
            {
                progress = value;
                NotifyPropertyChanged("Progress");
            }
        }
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                NotifyPropertyChanged("Title");
            }
        }

        public bool Complete
        {
            get { return complete; }
            set
            {
                complete = value;
                if (complete)
                    RemoveFromItems();
                NotifyPropertyChanged("Complete");
            }
        }

        private void AddToItems()
        {
            items.dispatcher.Invoke(new Action(() => { items.Add(this); }));
        }

        private void RemoveFromItems()
        {
            items.dispatcher.Invoke(new Action(() => { items.Remove(this); }));
        }

        public command.CancelCommand CancelCommand { get; set; }

        private double progress;
        private string title;
        private bool complete;
        private ObservableCollectionDisp<OperationView> items;
    }
}

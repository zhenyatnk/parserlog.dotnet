using FirstFloor.ModernUI.Windows.Controls;
using System;

namespace parserlog.dotnet.ui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            view_model = new ui.view_model.ViewModel();
            DataContext = view_model;
        }

        protected override void OnClosed(EventArgs e)
        {
        }

        private void Subscride(OperationFunction function, double interval)
        {
            var timer = new System.Timers.Timer(interval);

            timer.Elapsed += (source, e) => { function(); };
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        delegate void OperationFunction();

        private view_model.ViewModel view_model;
    }
}

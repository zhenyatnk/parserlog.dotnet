using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace parserlog.dotnet.ui.view_model
{
	public class ObservableCollectionDisp<T> : ObservableCollection<T>
	{
		public ObservableCollectionDisp()
		{
			current_dispacter = Dispatcher.CurrentDispatcher;
		}

		public Dispatcher dispatcher
		{
			get
			{
				return current_dispacter;
			}
		}

		private Dispatcher current_dispacter;
	}
}

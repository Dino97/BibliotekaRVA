using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Client
{
	public class ViewModelBase : INotifyPropertyChanged
	{
		public ViewModelBase CurrentViewModel { get; set; }

		public ViewModelBase()
		{
		}

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged = delegate { };
	}
}

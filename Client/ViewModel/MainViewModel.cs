using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModel
{
	class MainViewModel : ViewModelBase
	{
		public List<Book> Books { get; set; }



		public MainViewModel()
		{
			using (var c = new WaitCursor())
			{
				Books = Session.Current.LibraryProxy.GetBooks();
			}
		}
	}
}

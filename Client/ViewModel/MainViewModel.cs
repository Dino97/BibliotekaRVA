using Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Client.ViewModel
{
	class MainViewModel : ViewModelBase
	{
		public ObservableCollection<Book> Books { get; set; }

		public Command<Book> DuplicateCommand { get; set; }
		public Command RefreshCommand { get; set; }



		public MainViewModel()
		{
			DuplicateCommand = new Command<Book>(DuplicateBook);
			RefreshCommand = new Command(RefreshList);

			using (var c = new WaitCursor())
			{
				Books = new ObservableCollection<Book>(Session.Current.LibraryProxy.GetBooks());
			}
		}

		private void DuplicateBook(Book b)
		{
			Session.Current.LibraryProxy.DuplicateBook(b);
			RefreshList();
		}
		
		private void RefreshList()
		{
			foreach (Book b in Session.Current.LibraryProxy.GetBooks())
			{
				if (Books.ToList().Find(p => p.BookId == b.BookId) == null)
					Books.Add(b);
			}
		}
	}
}

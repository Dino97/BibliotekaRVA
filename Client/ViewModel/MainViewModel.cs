using Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Client.ViewModel
{
	class MainViewModel : ViewModelBase
	{
		public ObservableCollection<Book> Books { get; set; }
		public string BookNameTextBox { get; set; }
		public string AuthorTextBox { get; set; }
		private Book selectedBook;

		// Commands
		public Command NewBookCommand { get; set; }
		public Command<Book> DuplicateCommand { get; set; }
		public Command RefreshCommand { get; set; }
		public Command LeaseCommand { get; set; }



		public MainViewModel()
		{
			NewBookCommand = new Command(NewBook);
			DuplicateCommand = new Command<Book>(DuplicateBook, CanDuplicate);
			RefreshCommand = new Command(RefreshList);
			LeaseCommand = new Command(LeaseBook, CanLease);

			using (var c = new WaitCursor())
			{
				Books = new ObservableCollection<Book>(Session.Current.LibraryProxy.GetBooks());
			}
		}

		#region Bindings
		public Book SelectedBook
		{
			get { return selectedBook; }
			set
			{
				selectedBook = value;
				DuplicateCommand.RaiseCanExecuteChanged();
				LeaseCommand.RaiseCanExecuteChanged();
				OnPropertyChanged("SelectedBook");
			}
		}
		#endregion

		private void NewBook()
		{
		}

		private void DuplicateBook(Book b)
		{
			Session.Current.LibraryProxy.DuplicateBook(b);
			RefreshList();
		}

		private bool CanDuplicate()
		{
			return SelectedBook != null;
		}
		
		private void RefreshList()
		{
			foreach (Book b in Session.Current.LibraryProxy.GetBooks())
			{
				if (Books.ToList().Find(p => p.BookId == b.BookId) == null)
					Books.Add(b);
			}
		}

		private void LeaseBook()
		{
			SelectedBook.LeasedTo = new Member() { Username = "Test" };
			LeaseCommand.RaiseCanExecuteChanged();
			OnPropertyChanged("SelectedBook");
		}

		private bool CanLease()
		{
			return selectedBook != null && selectedBook.LeasedTo == null;
		}
	}
}

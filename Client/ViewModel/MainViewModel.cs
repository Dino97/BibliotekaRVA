using Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Client.ViewModel
{
	class MainViewModel : ViewModelBase
	{
		public ICollectionView BookList { get; set; }
		public string BookNameTextBox { get; set; }
		public string AuthorTextBox { get; set; }

		// Commands
		public Command NewBookCommand { get; set; }
		public Command EditBookCommand { get; set; }
		public Command<Book> DuplicateCommand { get; set; }
		public Command DeleteCommand { get; set; }
		public Command RefreshCommand { get; set; }
		public Command LeaseCommand { get; set; }
		public Command SearchCommand { get; set; }

		// Undo/redo
		public Command UndoCommand { get; set; }
		public Command RedoCommand { get; set; }
		private ActionHistory history;

		private Book selectedBook;
		private List<Book> books; // for display
		private List<Book> localBookDB; // for tracking changes



		public MainViewModel()
		{
			history = new ActionHistory();

			NewBookCommand = new Command(NewBook);
			EditBookCommand = new Command(EditBook);
			DuplicateCommand = new Command<Book>(DuplicateBook, CanDuplicate);
			DeleteCommand = new Command(DeleteBook);
			RefreshCommand = new Command(RefreshList);
			LeaseCommand = new Command(LeaseBook, CanLease);
			SearchCommand = new Command(FilterBooks);
			UndoCommand = new Command(Undo, history.CanUndo);
			RedoCommand = new Command(Redo, history.CanRedo);

			using (var c = new WaitCursor())
			{
				books = Session.Current.LibraryProxy.GetBooks();
				localBookDB = new List<Book>(books);

				CollectionViewSource itemSourceList = new CollectionViewSource() { Source = books };
				BookList = itemSourceList.View;
			}
		}

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

		private void NewBook()
		{
			var win = new View.NewBookWindow();
			NewBookViewModel vm = (NewBookViewModel)win.DataContext;

			win.ShowDialog();

			if (Session.Current.LibraryProxy.CreateBook(vm.BookName, vm.Author, int.Parse(vm.PublicationYear)))
				ClientLogger.Log($"Book {vm.BookName} successfully created.", Common.LogLevel.Info);
			else
				ClientLogger.Log($"Book {vm.BookName} could not be created.", Common.LogLevel.Error);

			RefreshList();
		}

		private void EditBook()
		{
			var win = new View.NewBookWindow();
			NewBookViewModel vm = (NewBookViewModel)win.DataContext;
			vm.BookName = selectedBook.BookName;
			vm.Author = selectedBook.Author.AuthorName;
			vm.PublicationYear = selectedBook.PublicationYear.ToString();

			win.ShowDialog();

			if (Session.Current.LibraryProxy.EditBook(selectedBook.BookName, vm.BookName, vm.Author, int.Parse(vm.PublicationYear)))
				ClientLogger.Log($"Book {vm.BookName} successfully edited.", Common.LogLevel.Info);
			else
				ClientLogger.Log($"Book {vm.BookName} could not be edited.", Common.LogLevel.Error);

			RefreshList();
		}

		private void DuplicateBook(Book b)
		{
			ClientLogger.Log($"Book {b.BookName} duplicated.", Common.LogLevel.Info);
			Session.Current.LibraryProxy.DuplicateBook(b);
			RefreshList();
		}

		private void DeleteBook()
		{
			if (Session.Current.LibraryProxy.DeleteBook(selectedBook.BookName))
				ClientLogger.Log($"Book {selectedBook.BookName} deleted successfully.", Common.LogLevel.Info);
			else
				ClientLogger.Log($"Book {selectedBook.BookName} could not be deleted.", Common.LogLevel.Error);

			RefreshList();
		}

		private bool CanDuplicate()
		{
			return SelectedBook != null;
		}

		private void RefreshList()
		{
			books.Clear();

			/*Func<Book, Book, bool> comparator = (b1, b2) => { return b1.BookName == b2.BookName && b1.Author.AuthorName == b2.Author.AuthorName && b1.PublicationYear == b2.PublicationYear && b1.LeasedTo == b2.LeasedTo; };

			foreach (Book b1 in Session.Current.LibraryProxy.GetBooks())
			{
				Book b2 = localBookDB.Find(b => b.BookName == b1.BookName);

				// Book is deleted on server
				if(b2 == null)
				{
					MessageBox.Show("Book is deleted on server");
				}
				// Check for change
				else
				{
					Book b3 = books.Find(b => b.BookName == b1.BookName);

					// Book is deleted locally
					if(b3 == null)
					{
						MessageBox.Show("Book is deleted locally");
					}

					// On server
					if(!comparator(b1, b2))
					{
						MessageBox.Show("Book is edited on server");
					}
					// Local
					else if(!comparator(b1, b3))
					{
						MessageBox.Show("Book is edited locally");
					}
				}
			}*/
			foreach (Book b in Session.Current.LibraryProxy.GetBooks())
			{
				if (books.ToList().Find(p => p.BookName == b.BookName) == null)
					books.Add(b);
			}

			BookList.Refresh();
		}

		private void LeaseBook()
		{
			Action redo = () =>
			{
				SelectedBook.LeasedTo = Session.Current.LoggedInUser;

				LeaseCommand.RaiseCanExecuteChanged();
				OnPropertyChanged("SelectedBook");
				BookList.Refresh();
			};
			Action undo = () =>
			{
				SelectedBook.LeasedTo = string.Empty;

				LeaseCommand.RaiseCanExecuteChanged();
				OnPropertyChanged("SelectedBook");
				BookList.Refresh();
			};

			history.AddAndExecute(new RevertableCommand(redo, undo));
			UndoCommand.RaiseCanExecuteChanged();
			RedoCommand.RaiseCanExecuteChanged();

			ClientLogger.Log($"{Session.Current.LoggedInUser} leased book {selectedBook.BookName}", Common.LogLevel.Info);
		}

		private bool CanLease()
		{
			return selectedBook != null && selectedBook.LeasedTo == string.Empty;
		}

		private void FilterBooks()
		{
			BookList.Filter = new Predicate<object>(b => 
				((Book)b).BookName.Contains(BookNameTextBox == null ? "" : BookNameTextBox) && 
				((Book)b).Author.AuthorName.Contains(AuthorTextBox == null ? "" : AuthorTextBox)
			);
		}

		private void Undo()
		{
			history.Undo();
			UndoCommand.RaiseCanExecuteChanged();
			RedoCommand.RaiseCanExecuteChanged();
		}

		private void Redo()
		{
			history.Redo();
			UndoCommand.RaiseCanExecuteChanged();
			RedoCommand.RaiseCanExecuteChanged();
		}
	}
}

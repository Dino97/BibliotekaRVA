using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteka;
using Common.Model;
using Common;

namespace Server
{
	class LibraryServices : ILibraryServices
	{
		private static List<string> loggedInUsers = new List<string>();
		private Logger logger = new Logger();



		public LibraryServices()
		{
			logger.AddTarget(new LoggerConsoleTarget());
			logger.AddTarget(new LoggerFileTarget("LogData.txt"));
		}

		public LogInInfo LogIn(string username, string password)
		{
			using(var db = new LibraryDbContext())
			{
				IUser user = db.Members.FirstOrDefault(u => u.Username.Equals(username));

				// if not found in users, look in admins
				if(user == null)
					user = db.Admins.FirstOrDefault(a => a.Username.Equals(username));

				// if still null, wrong username
				if (user == null)
					return LogInInfo.WrongUserOrPass;

				// already logged in
				if (loggedInUsers.Contains(username))
					return LogInInfo.AlreadyConnected;

				if (user.Password == password)
				{
					logger.Log("User " + user.Username + " connected.", LogLevel.Info);
					loggedInUsers.Add(username);

					return LogInInfo.Sucess;
				}

				// wrong password
				logger.Log("Failed login attempt for user " + user.Username, LogLevel.Warning);
				return LogInInfo.WrongUserOrPass;
			}
		}

		public void LogOut(string username)
		{
			logger.Log("User " + username + " has disconnected.", LogLevel.Info);
		}

		public UserInfo GetUserInfo(string username)
		{
			IUser user = null;

			using (var db = new LibraryDbContext())
			{
				user = db.Members.FirstOrDefault(m => m.Username == username);

				if (user == null)
					user = db.Admins.FirstOrDefault(m => m.Username == username);
			}

			if (user == null)
				return null;

			logger.Log("User info of " + username + " querried.", LogLevel.Debug);

			return new UserInfo() { Username = user.Username, FirstName = user.FirstName, LastName = user.LastName };
		}

		public void EditUserInfo(string username, string firstName, string lastName)
		{
			IUser user = null;

			using (var db = new LibraryDbContext())
			{
				user = db.Members.FirstOrDefault(m => m.Username == username);

				if (user == null)
					user = db.Admins.FirstOrDefault(m => m.Username == username);

				if (user == null)
					return;

				user.FirstName = firstName;
				user.LastName = lastName;

				logger.Log("Account info of " + username + " edited.", LogLevel.Debug);

				db.SaveChanges();
			}
		}

		public bool CreateUser(string username, string password, string firstName, string lastName)
		{
			using (var db = new LibraryDbContext())
			{
				if (db.Members.Find(username) != null)
					return false;

				Member m = new Member() { Username = username, Password = password, FirstName = firstName, LastName = lastName };
				db.Members.Add(m);
				db.SaveChanges();
			}

			logger.Log("User " + username + " created.", LogLevel.Info);

			return true;
		}

		public bool CreateBook(string name, string author, int publicationYear)
		{
			using (var db = new LibraryDbContext())
			{
				// If book already exists
				if (db.Books.Find(name) != null)
				{
					logger.Log("Failed to create book " + name, LogLevel.Error);
					return false;
				}

				Author a = db.Authors.Find(author);

				// Author doesn't exist, create a new one
				if(a == null)
				{
					a = new Author() { AuthorName = author, Summary = author + "'s summary." };
					logger.Log($"Author {author} successfully created.", LogLevel.Info);
					db.Authors.Add(a);
				}

				db.Books.Add(new Book() { BookName = name, Author = a, PublicationYear = publicationYear, LeasedTo = string.Empty });
				db.SaveChanges();

				logger.Log($"Book {name} successfully created.", LogLevel.Info);
				return true;
			}
		}

		public bool EditBook(string oldName, string newName, string author, int publicationYear)
		{
			using (var db = new LibraryDbContext())
			{
				Author a = db.Authors.Find(author);

				// Author doesn't exist, create a new one
				if (a == null)
				{
					a = new Author() { AuthorName = author, Summary = author + "'s summary." };
					logger.Log($"Author {author} successfully created.", LogLevel.Info);
					db.Authors.Add(a);
				}

				Book b = db.Books.Find(oldName);

				// If book doesn't exist
				if (b == null)
				{
					logger.Log($"Failed to edit book {oldName}.", LogLevel.Error);
					return false;
				}

				db.Books.Remove(b);
				db.SaveChanges();

				b = new Book();
				b.BookName = newName;
				b.Author = a;
				b.PublicationYear = publicationYear;
				b.LeasedTo = string.Empty;

				db.Books.Add(b);
				db.SaveChanges();

				logger.Log($"Book {oldName} successfully edited.", LogLevel.Info);
				return true;
			}
		}

		public bool DeleteBook(string name)
		{
			using (var db = new LibraryDbContext())
			{
				Book b = db.Books.Find(name);

				// If book doesn't exist
				if (b == null)
				{
					logger.Log($"Failed to delete book {name}.", LogLevel.Error);
					return false;
				}

				db.Books.Remove(b);
				db.SaveChanges();

				logger.Log($"Book {name} successfully deleted.", LogLevel.Info);
				return true;
			}
		}

		public List<Book> GetBooks()
		{
			using (var db = new LibraryDbContext())
			{
				logger.Log("Book data querried.", LogLevel.Debug);
				return db.Books.Include("Author").ToList();
			}
		}

		public void DuplicateBook(Book book)
		{
			List<Book> books = GetBooks();

			Book toClone = books.Find(b => b.BookName == book.BookName);
			Book clone = (Book)toClone.Clone();

			using (var db = new LibraryDbContext())
			{
				db.Authors.Attach(clone.Author);
				clone.BookName += " (Copy)";
				db.Books.Add(clone);
				db.SaveChanges();

				logger.Log("Book " + book.BookName + " duplicated.", LogLevel.Info);
			}
		}
	}
}

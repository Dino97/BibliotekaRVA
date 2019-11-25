using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteka;
using Common.Model;

namespace Server
{
	class LibraryServices : ILibraryServices
	{
		private static List<string> loggedInUsers = new List<string>();


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
					Console.WriteLine("User " + username + " connected.");
					loggedInUsers.Add(username);

					return LogInInfo.Sucess;
				}

				// wrong password
				return LogInInfo.WrongUserOrPass;
			}
		}

		public List<Book> GetBooks()
		{
			using (var db = new LibraryDbContext())
			{
				return db.Books.Include("Author").ToList();
			}
		}

		public void DuplicateBook(Book book)
		{
			List<Book> books = GetBooks();

			Book toClone = books.Find(b => b.BookId == book.BookId);
			Book clone = (Book)toClone.Clone();

			using (var db = new LibraryDbContext())
			{
				db.Authors.Attach(clone.Author);
				db.Books.Add(clone);
				db.SaveChanges();

				Console.WriteLine("Book " + book.BookName + " duplicated.");
			}
		}
	}
}

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

		public bool LogIn(string username, string password)
		{
			using(var db = new LibraryDbContext())
			{
				IUser user = db.Members.FirstOrDefault(u => u.Username.Equals(username));

				if (user == null)
					return false;

				if (user.Password == password)
				{
					Console.WriteLine("User " + username + " connected.");
					return true;
				}

				return false;
			}
		}

		public List<Book> GetBooks()
		{
			using(var db = new LibraryDbContext())
			{
				var res = db.Books.Include("Author").ToList();
				return res;
			}
		}
	}
}

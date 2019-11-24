using Common.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
	class LibraryDbInitializer : DropCreateDatabaseAlways<LibraryDbContext>
	{
		protected override void Seed(LibraryDbContext context)
		{
			IList<Author> defaultAuthors = new List<Author>();
			defaultAuthors.Add(new Author() { Summary = "Stephen's summary." });

			IList<Book> defaultBooks = new List<Book>();

			defaultBooks.Add(new Book() { BookName = "IT", PublicationYear = 1986, Author = null, LeasedTo = null });

			context.Books.AddRange(defaultBooks);

			base.Seed(context);
		}
	}
}

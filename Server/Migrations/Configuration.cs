namespace Server.Migrations
{
    using Common.Model;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Server.LibraryDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Server.LibraryDbContext context)
        {
			// Authors
			IList<Author> defaultAuthors = new List<Author>();
			defaultAuthors.Add(new Author() { AuthorName = "Stephen King", Summary = "Stephen's summary." });

			foreach (var a in defaultAuthors)
				context.Authors.AddOrUpdate(a);

			// Members
			context.Members.AddOrUpdate(new Member() { FirstName = "Dino", LastName = "T", Username = "Dino", Password = "1234" });

			// Books
			IList<Book> defaultBooks = new List<Book>();
			defaultBooks.Add(new Book() { BookName = "IT", PublicationYear = 1986, Author = defaultAuthors[0], LeasedTo = null });

			foreach (var b in defaultBooks)
				context.Books.AddOrUpdate(b);
		}
    }
}

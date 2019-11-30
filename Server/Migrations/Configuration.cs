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
			defaultAuthors.Add(new Author() { AuthorName = "Stephen King", Summary = "Stephen King is an American author of horror, supernatural fiction, suspense, and fantasy novels." });
			defaultAuthors.Add(new Author() { AuthorName = "William Peter Blaty", Summary = "William Peter Blaty was an American writer and filmmaker best known for his 1971 novel The Exorcist and for the Academy Award-winning screenplay of its film adaptation." });

			foreach (var a in defaultAuthors)
				context.Authors.AddOrUpdate(a);

			// Members
			context.Members.AddOrUpdate(new Member() { FirstName = "Dino", LastName = "Tabakovic", Username = "dino", Password = "1234" });

			// Admins
			context.Admins.AddOrUpdate(new Admin() { FirstName = "", LastName = "", Username = "admin", Password = "admin" });

			// Books
			IList<Book> defaultBooks = new List<Book>();
			defaultBooks.Add(new Book() { BookName = "IT", PublicationYear = 1986, Author = defaultAuthors[0], LeasedTo = string.Empty });
			defaultBooks.Add(new Book() { BookName = "The Shining", PublicationYear = 1977, Author = defaultAuthors[0], LeasedTo = string.Empty });
			defaultBooks.Add(new Book() { BookName = "The Exorcists", PublicationYear = 1971, Author = defaultAuthors[1], LeasedTo = string.Empty });

			foreach (var b in defaultBooks)
				context.Books.AddOrUpdate(b);
		}
    }
}

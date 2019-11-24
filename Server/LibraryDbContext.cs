using System;
using System.Data.Entity;
using Common.Model;

namespace Server
{
	class LibraryDbContext : DbContext
	{
		public DbSet<Book> Books { get; set; }
		public DbSet<Member> Members { get; set; }
		public DbSet<Author> Authors { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			Database.SetInitializer<LibraryDbContext>(null);
			base.OnModelCreating(modelBuilder);
		}
	}
}
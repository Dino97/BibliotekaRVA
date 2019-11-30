using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Common.Model
{
    public class Book : ICloneable
    {
		[Key]
		public string BookName { get; set; }
		public int PublicationYear { get; set; }
		public Author Author { get; set; }
		public string LeasedTo { get; set; }



		public ICloneable Clone()
		{
			Book res = new Book();
			res.BookName = BookName;
			res.PublicationYear = PublicationYear;
			res.Author = Author;
			res.LeasedTo = string.Empty;

			return res;
		}
	}
}

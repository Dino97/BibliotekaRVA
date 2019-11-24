using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Common.Model
{
    public class Book
    {
		public int BookId { get; set; }
		public string BookName { get; set; }
		public int PublicationYear { get; set; }
		public Author Author { get; set; }
		public Member LeasedTo { get; set; }
    }
}

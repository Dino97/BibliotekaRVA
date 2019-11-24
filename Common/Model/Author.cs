using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Common.Model
{
	public class Author
	{
		[Key]
		public string AuthorName { get; set; }
		public string Summary { get; set; }
	}
}

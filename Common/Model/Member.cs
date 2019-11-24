using Common.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Common.Model
{
	public class Member : IUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }

		[Key]
		public string Username { get; set; }
		public string Password { get; set; }
	}
}

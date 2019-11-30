using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
	public class UserInfo
	{
		public string Username { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}

	public interface IUser
	{
		string Username { get; set; }
		string Password { get; set; }
		string FirstName { get; set; }
		string LastName { get; set; }
	}
}

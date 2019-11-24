using Common.Model;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Biblioteka
{
	[ServiceContract]
	public interface ILibraryServices
	{
		[OperationContract]
		bool LogIn(string username, string password);

		[OperationContract]
		List<Book> GetBooks();
	}
}

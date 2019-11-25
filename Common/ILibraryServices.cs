using Common.Model;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Biblioteka
{
	public enum LogInInfo
	{
		WrongUserOrPass,
		AlreadyConnected,
		Sucess
	}

	[ServiceContract]
	public interface ILibraryServices
	{
		[OperationContract]
		LogInInfo LogIn(string username, string password);

		[OperationContract]
		List<Book> GetBooks();

		[OperationContract(IsOneWay = true)]
		void DuplicateBook(Book book);
	}
}

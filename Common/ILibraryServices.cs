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

		[OperationContract(IsOneWay = true)]
		void LogOut(string username);

		[OperationContract]
		UserInfo GetUserInfo(string username);

		[OperationContract]
		void EditUserInfo(string username, string firstName, string lastName);

		[OperationContract]
		bool CreateUser(string username, string password, string firstName, string lastName);

		[OperationContract]
		bool CreateBook(string name, string author, int publicationYear);

		[OperationContract]
		bool EditBook(string oldName, string newName, string author, int publicationYear);

		[OperationContract]
		bool DeleteBook(string name);

		[OperationContract]
		List<Book> GetBooks();

		[OperationContract(IsOneWay = true)]
		void DuplicateBook(Book book);
	}
}

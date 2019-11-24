using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Biblioteka;
using Common.Model;

namespace Server
{
	class Program
	{
		static void Main(string[] args)
		{
			ServiceHost host = new ServiceHost(typeof(LibraryServices));
			host.AddServiceEndpoint(typeof(ILibraryServices), new NetTcpBinding(), "net.tcp://localhost:9000");
			host.Open();

			Console.WriteLine("Library services are now online. Waiting for requests...");
			Console.ReadLine();
		}
	}
}

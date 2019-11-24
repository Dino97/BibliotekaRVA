using Biblioteka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
	public class Session
	{
		private static Session current;

		public ILibraryServices LibraryProxy { get; private set; }




		public static Session Current
		{
			get
			{
				if (current == null)
					current = new Session();

				return current;
			}
		}

		public Session()
		{
			ChannelFactory<ILibraryServices> cf = new ChannelFactory<ILibraryServices>(new NetTcpBinding(), "net.tcp://localhost:9000");
			LibraryProxy = cf.CreateChannel();
		}
	}
}

using Biblioteka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Client.ViewModel
{
	public class LoginViewModel : ViewModelBase
	{
		public string Username { get; set; }
		public string ErrorText { get; set; }

		public Command<object> LoginCommand { get; set; }



		public LoginViewModel()
		{
			LoginCommand = new Command<object>(Login);
		}

		private void Login(object parameter)
		{
			CurrentViewModel = new MainViewModel();

			PasswordBox pass = parameter as PasswordBox;

			bool loginSuccess;
			using (var c = new WaitCursor())
			{
				loginSuccess = Session.Current.LibraryProxy.LogIn(Username, pass.Password);
			}

			if (loginSuccess)
			{
				MainWindow mw = new MainWindow();
				mw.Show();

				Application.Current.MainWindow.Close();
				Application.Current.MainWindow = mw;
			}
			else
			{
				ErrorText = "Username or password is incorrect.";
			}
		}
	}
}
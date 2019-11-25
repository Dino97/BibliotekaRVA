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

		public Command<object> LoginCommand { get; set; }

		private string errorText;



		public LoginViewModel()
		{
			LoginCommand = new Command<object>(Login);
		}

		#region Properties
		public string ErrorText
		{
			get { return errorText; }
			set
			{
				errorText = value;
				OnPropertyChanged("ErrorText");
			}
		}
		#endregion

		private void Login(object parameter)
		{
			CurrentViewModel = new MainViewModel();

			PasswordBox pass = parameter as PasswordBox;

			LogInInfo loginInfo;
			using (var c = new WaitCursor())
			{
				loginInfo = Session.Current.LibraryProxy.LogIn(Username, pass.Password);
			}

			if (loginInfo == LogInInfo.Sucess)
			{
				MainWindow mw = new MainWindow();
				mw.Show();

				Application.Current.MainWindow.Close();
				Application.Current.MainWindow = mw;
			}
			else
			{
				if (loginInfo == LogInInfo.WrongUserOrPass)
					ErrorText = "Username or password is incorrect.";
				else
					ErrorText = "Account already connected.";
			}
		}
	}
}
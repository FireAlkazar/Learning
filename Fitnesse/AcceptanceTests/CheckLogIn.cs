using fit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcceptanceTests
{
	public class CheckLogIn : ColumnFixture
	{
		public string Username;
		public string Password;
		public bool CanLogIn()
		{
			try
			{
				SetUpTestEnvironment.playerManager.LogIn(Username, Password);
				return true;
			}
			catch (ApplicationException)
			{
				return false;
			}
		}
	}

}

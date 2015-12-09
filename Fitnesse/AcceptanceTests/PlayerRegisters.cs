using fit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tristan.inproc;

namespace AcceptanceTests
{
	public class PlayerRegisters : ColumnFixture
	{
		public class ExtendedPlayerRegistrationInfo : PlayerRegistrationInfo
		{
			public int PlayerId()
			{
				return SetUpTestEnvironment.playerManager.RegisterPlayer(this);
			}
		}

		private ExtendedPlayerRegistrationInfo to = new ExtendedPlayerRegistrationInfo();

		public override object GetTargetObject()
		{
			return to;
		}
	}

	public class PlayerRegistersOld : ColumnFixture
	{
		public string Username;
		public string Password;
		public int PlayerId()
		{
			PlayerRegistrationInfo reg = new PlayerRegistrationInfo();
			reg.Username = Username;
			reg.Password = Password;
			return SetUpTestEnvironment.playerManager.RegisterPlayer(reg);
		}
	}

}

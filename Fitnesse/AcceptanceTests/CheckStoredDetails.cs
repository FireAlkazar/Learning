using fit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcceptanceTests
{
	public class CheckStoredDetails : ColumnFixture
	{
		public int PlayerId;
		public string Username
		{
			get
			{
				return SetUpTestEnvironment.playerManager.
				GetPlayer(PlayerId).Username;
			}
		}
		public decimal Balance
		{
			get
			{
				return SetUpTestEnvironment.playerManager.
				GetPlayer(PlayerId).Balance;
			}
		}
	}

}

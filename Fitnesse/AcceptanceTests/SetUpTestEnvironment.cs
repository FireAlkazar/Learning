using fit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tristan;
using Tristan.inproc;

namespace AcceptanceTests
{
	public class SetUpTestEnvironment : Fixture
	{
		internal static IPlayerManager playerManager;
		public SetUpTestEnvironment()
		{
			playerManager = new PlayerManager();
		}
	}

}

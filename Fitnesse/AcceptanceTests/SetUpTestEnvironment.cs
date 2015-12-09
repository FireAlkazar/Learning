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
	public class SetUpTestEnvironment : ColumnFixture
	{
		internal static IPlayerManager playerManager;
		internal static IDrawManager drawManager;

		public SetUpTestEnvironment()
		{
			playerManager = new PlayerManager();
			drawManager = new DrawManager(playerManager);
		}

		public void Empty(string arg)
		{ }

		public DateTime CreateDraw
		{
			set
			{
				drawManager.CreateDraw(value);
			}
		}
	}
}

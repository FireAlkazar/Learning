using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcceptanceTests
{
	/// <summary>
	/// Used to demonstrate business object wrapping
	/// 
	/// This feature is very useful for functional tests, but is not so well suited tocustomer-oriented story tests.
	/// </summary>
	public class MessageLog : fitlibrary.DoFixture
	{
		Queue<string> queue = new Queue<string>();
		public MessageLog()
		{
			mySystemUnderTest = queue;//!!!!!
		}
		public void GenerateMessages(int count)
		{
			for (int i = 0; i < count; i++)
				queue.Enqueue("M" + i);
		}
	}
}

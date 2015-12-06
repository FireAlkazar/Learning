using fit;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcceptanceTests
{
	public class ColumnFixtureTest : ColumnFixture
	{
		public String firstPart;
		public String secondPart;
		public String Together
		{
			get
			{
				return firstPart + ", " + secondPart;
			}
		}
		public int TotalLength()
		{
			return firstPart.Length + secondPart.Length;
		}
	}
}

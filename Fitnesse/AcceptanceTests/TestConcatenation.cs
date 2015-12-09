using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcceptanceTests
{
	public class TestConcatenation : fit.Fixture
	{
		public string FirstString;
		public string SecondString;
		public string Concatenate()
		{
			return FirstString + SecondString;
		}
		public void Clear()
		{
			FirstString = "";
		}
	}
}

using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForFitnesse
{
	public class InfoModule : NancyModule
	{
		public InfoModule()
			: base("/")
		{
			Get["/"] = _ => "Hello world!";
		}
	}
}

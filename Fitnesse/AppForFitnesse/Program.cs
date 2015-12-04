using Microsoft.Owin.Hosting;
using System;

namespace AppForFitnesse
{
	class Program
	{
		static void Main(string[] args)
		{
			using (WebApp.Start<Startup>("http://localhost:9000"))
			{
				Console.WriteLine("Press [enter] to quit...");
				Console.ReadLine();
			}
		}
	}
}

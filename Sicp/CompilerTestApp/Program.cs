using System;
using System.Threading.Tasks;

namespace CompilerTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var compilerWrapper = new CompilerWrapper(new Compiler());

            Task<byte[]> dalProject = compilerWrapper.BuildProjectAsync("Solulition/Dal/Dal.csproj");
            Task<byte[]> appServicesProject = compilerWrapper.BuildProjectAsync("Solulition/AppServices/AppServices.csproj");

            Console.WriteLine("Projects build started...");

            Task.WaitAll(
                dalProject.ContinueWith(x => Console.WriteLine("Dal project has been built.")),
                appServicesProject.ContinueWith(x => Console.WriteLine("AppServices project has been built."))
            );

            Console.WriteLine("Projects build finished.");

            Console.ReadLine();
        }
    }
}

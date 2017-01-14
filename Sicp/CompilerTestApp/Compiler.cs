using System.Text;
using System.Threading;

namespace CompilerTestApp
{
    public class Compiler
    {
        public byte[] BuildProject(string projectPath)
        {
            Thread.Sleep(5000);
            return Encoding.UTF8.GetBytes(projectPath);
        }
    }
}

using System;
using System.Threading.Tasks;

namespace CompilerTestApp
{
    public sealed class CompilerWrapper
    {
        private readonly Compiler _compiler;
        private readonly object _lockObject = new object();

        public CompilerWrapper(Compiler compiler)
        {
            if (compiler == null)
            {
                throw new ArgumentNullException(nameof(compiler));
            }

            _compiler = compiler;
        }

        public Task<byte[]> BuildProjectAsync(string projectPath)
        {
            Task<byte[]> task = Task.Run(() => BuildProjectThreadSafe(projectPath));
            return task;
        }

        private byte[] BuildProjectThreadSafe(string projectPath)
        {
            lock (_lockObject)
            {
                return _compiler.BuildProject(projectPath);
            }
        }
    }
}

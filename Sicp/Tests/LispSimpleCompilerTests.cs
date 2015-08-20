using Xunit;

namespace Sicp.Tests
{
    public sealed class LispSimpleCompilerTests
    {
        [Fact]
        public void Add()
        {
            int result = LispSimpleCompiler.Execute("(+ 2 3)");

            Assert.Equal(5, result);
        }
    }
}
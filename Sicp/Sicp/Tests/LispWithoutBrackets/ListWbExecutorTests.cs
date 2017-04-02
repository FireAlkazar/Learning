using Sicp.LispWithoutBrackets;
using Xunit;

namespace Sicp.Tests.LispWithoutBrackets
{
    public class ListWbExecutorTests
    {
        [Fact]
        public void SingelPlusStatement()
        {
            var listWbExecutor = new ListWbExecutor();

            int result = listWbExecutor.Execute("+ 3 7");

            Assert.Equal(10, result);
        }


        [Fact]
        public void PlusStatementWithVariable()
        {
            var listWbExecutor = new ListWbExecutor();

            const string program = @"define y 5
+ 3 y";

            int result = listWbExecutor.Execute(program);

            Assert.Equal(8, result);
        }

        [Fact]
        public void PlusStatementWithTwoVariable()
        {
            var listWbExecutor = new ListWbExecutor();

            const string program = @"define y 5
define x y
+ x y";

            int result = listWbExecutor.Execute(program);

            Assert.Equal(10, result);
        }
    }
}
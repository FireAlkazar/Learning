using Sicp.Lisp;
using Xunit;

namespace Sicp.Tests.Lisp
{
    public class ListInterpreterTests
    {
        [Fact]
        public void SingelPlusStatement()
        {
            var listWbExecutor = new ListInterpreter();

            int result = listWbExecutor.Interprete("(+ 3 7)");

            Assert.Equal(10, result);
        }


        [Fact]
        public void PlusStatementWithVariable()
        {
            var listWbExecutor = new ListInterpreter();

            const string program = @"(define y 5)
(+ 3 y)";

            int result = listWbExecutor.Interprete(program);

            Assert.Equal(8, result);
        }

        [Fact]
        public void VariableValue()
        {
            var listWbExecutor = new ListInterpreter();

            const string program = @"(define size 5)
size";

            int result = listWbExecutor.Interprete(program);

            Assert.Equal(5, result);
        }

        [Fact]
        public void PlusStatementWithTwoVariable()
        {
            var listWbExecutor = new ListInterpreter();

            const string program = @"(define y 5)
(define x y)
(+ x y)";

            int result = listWbExecutor.Interprete(program);

            Assert.Equal(10, result);
        }

        [Fact]
        public void LongExpression()
        {
            var listWbExecutor = new ListInterpreter();

            int result = listWbExecutor.Interprete("(+ (* 3 (+ (* 2 4) (+ 3 5))) (+ (- 10 7) 6))");

            Assert.Equal(3*16+9, result);
        }
    }
}
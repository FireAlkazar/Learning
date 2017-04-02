using System.Linq;
using Sicp.Lisp;
using Xunit;

namespace Sicp.Tests.Lisp
{
    public class ListInterpreterTests
    {
        [Fact]
        public void SingelPlusStatement()
        {
            var listInterpreter = new ListInterpreter();

            int result = listInterpreter.Interprete("(+ 3 7)");

            Assert.Equal(10, result);
        }


        [Fact]
        public void PlusStatementWithVariable()
        {
            var listInterpreter = new ListInterpreter();
            const string program = @"(define y 5)
(+ 3 y)";

            int result = listInterpreter.Interprete(program);

            Assert.Equal(8, result);
        }

        [Fact]
        public void VariableValue()
        {
            var listInterpreter = new ListInterpreter();
            const string program = @"(define size 5)
size";

            int result = listInterpreter.Interprete(program);

            Assert.Equal(5, result);
        }

        [Fact]
        public void PlusStatementWithTwoVariable()
        {
            var listInterpreter = new ListInterpreter();
            const string program = @"(define y 5)
(define x y)
(+ x y)";

            int result = listInterpreter.Interprete(program);

            Assert.Equal(10, result);
        }

        [Fact]
        public void DefineVariableByExpression()
        {
            var listInterpreter = new ListInterpreter();

            int result = listInterpreter.Interprete("(define y (* 3 5))");

            Assert.Equal(3 * 5, result);
        }

        [Fact]
        public void UnaryMinus()
        {
            var listInterpreter = new ListInterpreter();

            int result = listInterpreter.Interprete("(- 5)");

            Assert.Equal(-5, result);
        }

        [Fact]
        public void Minus()
        {
            var listInterpreter = new ListInterpreter();

            int result = listInterpreter.Interprete("(- 8 5)");

            Assert.Equal(3, result);
        }

        [Fact]
        public void LongExpression()
        {
            var listInterpreter = new ListInterpreter();

            int result = listInterpreter.Interprete("(+ (* 3 (+ (* 2 4) (+ 3 5))) (+ (- 10 7) 6))");

            Assert.Equal(3*16+9, result);
        }

        [Fact]
        public void DefineSquareFunction()
        {
            var listInterpreter = new ListInterpreter();
            const string program = @"(define (square x) (* x x))
(square 21)";

            int result = listInterpreter.Interprete(program);

            Assert.Equal(21 * 21, result);
        }

        [Fact]
        public void DefineSumOfSquaresFunction()
        {
            var listInterpreter = new ListInterpreter();
            const string program = @"(define (square x) (* x x))
(define (sum-of-squares z y) (+ (square z) (square y)))
(sum-of-squares 3 4)";

            int result = listInterpreter.Interprete(program);

            Assert.Equal(25, result);
        }
    }
}
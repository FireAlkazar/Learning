using System.Linq;
using Sicp.Lisp;
using Xunit;

namespace Sicp.Tests.Lisp
{
    public class ListInterpreterTests
    {
        readonly ListInterpreter _listInterpreter = new ListInterpreter();

        [Fact]
        public void SingelPlusStatement()
        {
            double result = _listInterpreter.Interprete("(+ 3 7)");

            Assert.Equal(10, result);
        }


        [Fact]
        public void PlusStatementWithVariable()
        {
            const string program = @"(define y 5)
(+ 3 y)";

            double result = _listInterpreter.Interprete(program);

            Assert.Equal(8, result);
        }

        [Fact]
        public void VariableValue()
        {
            const string program = @"(define size 5)
size";

            double result = _listInterpreter.Interprete(program);

            Assert.Equal(5, result);
        }

        [Fact]
        public void PlusStatementWithTwoVariable()
        {
            const string program = @"(define y 5)
(define x y)
(+ x y)";

            double result = _listInterpreter.Interprete(program);

            Assert.Equal(10, result);
        }

        [Fact]
        public void DefineVariableByExpression()
        {
            double result = _listInterpreter.Interprete("(define y (* 3 5))");

            Assert.Equal(3 * 5, result);
        }

        [Fact]
        public void UnaryMinus()
        {
            double result = _listInterpreter.Interprete("(- 5)");

            Assert.Equal(-5, result);
        }


        [Fact]
        public void Minus()
        {
            double result = _listInterpreter.Interprete("(- 8 5)");

            Assert.Equal(3, result);
        }

        [Fact]
        public void AverageFunction()
        {
            const string program = @"(define (average x y)
(/ (+ x y) 2))
(average 2 4)";

            double result = _listInterpreter.Interprete(program);

            Assert.Equal(3, result);
        }

        [Fact]
        public void LongExpression()
        {
            double result = _listInterpreter.Interprete("(+ (* 3 (+ (* 2 4) (+ 3 5))) (+ (- 10 7) 6))");

            Assert.Equal(3*16+9, result);
        }

        [Fact]
        public void DefineSquareFunction()
        {
            const string program = @"(define (square x) (* x x))
(square 21)";

            double result = _listInterpreter.Interprete(program);

            Assert.Equal(21 * 21, result);
        }

        [Fact]
        public void DefineSumOfSquaresFunction()
        {
            const string program = @"(define (square x) (* x x))
(define (sum-of-squares z y) (+ (square z) (square y)))
(sum-of-squares 3 4)";

            double result = _listInterpreter.Interprete(program);

            Assert.Equal(25, result);
        }

        [Fact]
        public void AbsFunction()
        {
            const string program = @"(define (abs x) (if (< x 0) (- x) x))
(abs -3)";

            double result = _listInterpreter.Interprete(program);

            Assert.Equal(3, result);
        }

        [Fact]
        public void OperatorAsExpression()
        {
            const string program = @"(define a 2)
(define b 1) 
((if (> b 0) + -) a b)";

            double result = _listInterpreter.Interprete(program);

            Assert.Equal(3, result);
        }
    }
}
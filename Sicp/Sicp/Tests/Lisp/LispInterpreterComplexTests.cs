using Sicp.Lisp;
using Xunit;

namespace Sicp.Tests.Lisp
{
    public class LispInterpreterComplexTests
    {
        readonly ListInterpreter _listInterpreter = new ListInterpreter();

         [Fact]
         public void Sqrt()
         {
            const string program = @"(define (abs x) (if (< x 0) (- x) x))

(define (square x) (* x x))

(define (good-enough? guess x)
(< (abs (- (square guess) x)) 0.001))

(define (average x y)
(/ (+ x y) 2))

(define (improve guess x)
(average guess (/ x guess)))

(define (sqrt-iter guess x)
(if (good-enough? guess x)
guess
(sqrt-iter (improve guess x)
x)))

(define (sqrt x)
(sqrt-iter 1.0 x))

(sqrt 9)
";
            double result = _listInterpreter.Interprete(program);

            Assert.Equal(3.0, result);
         }
    }
}
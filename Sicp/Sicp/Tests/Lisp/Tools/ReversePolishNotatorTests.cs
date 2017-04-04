using Sicp.Lisp.Expressions;
using Sicp.Lisp.Tools;
using Xunit;

namespace Sicp.Tests.Lisp.Tools
{
    public class ReversePolishNotatorTests
    {
        readonly ReversePolishNotator _notator = new ReversePolishNotator();
         
        [Fact]
        public void FourNumbersThreeOperators()
        {
            var exp = new ArithmeticExp("*")
            {
                Children =
                {
                    new ArithmeticExp("+")
                    {
                        Children =
                        {
                            new IntExp(1),
                            new IntExp(2)
                        }
                    },
                    new ArithmeticExp("-")
                    {
                        Children =
                        {
                            new IntExp(4),
                            new IntExp(3)
                        }
                    },
                }
            };

            string notation = _notator.GetNotation(exp);

            Assert.Equal("1 2 + 4 3 - *", notation);
        }
    }
}
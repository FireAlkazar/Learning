using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Sicp.Tests
{
    public sealed class LispExpressionParser
    {
        [Fact]
        public void ParseDefineFunction()
        {
            List<string> list = Parse("(define (f n) (n))");

            Assert.Equal("define", list[0]);
            Assert.Equal("(f n)", list[1]);
            Assert.Equal("(n)", list[2]);
        }

        [Fact]
        public void ParseNumber()
        {
            List<string> list = Parse("456");

            Assert.Equal("456", list[0]);
        }

        [Fact]
        public void ParseDefineRecursiveFunction()
        {
            List<string> list = Parse("(define (f n) (* n (f (n - 1))))");

            Assert.Equal("define", list[0]);
            Assert.Equal("(f n)", list[1]);
            Assert.Equal("(* n (f (n - 1)))", list[2]);
        }

        [Fact]
        public void ParseSimpleDefine()
        {
            List<string> list = Parse("(define x 5)");

            Assert.Equal("define", list[0]);
            Assert.Equal("x", list[1]);
            Assert.Equal("5", list[2]);
        }

        private List<string> Parse(string expression)
        {
            return SplitExpressionIntoParts(expression).ToList();
        }

        private List<string> SplitExpressionIntoParts(string expression)
        {
            if (expression.StartsWith("(") && expression.EndsWith(")"))
            {
                expression = expression.Substring(1, expression.Length - 2);
            }

            int curOpenedBracesCount = 0;
            var result = new List<string>();

            var buffer = new StringBuilder();

            foreach (char curChar in expression)
            {
                if (curChar == '(')
                {
                    curOpenedBracesCount++;
                }

                if (curChar == ')')
                {
                    curOpenedBracesCount--;
                    if (curOpenedBracesCount < 0)
                    {
                        throw new InvalidOperationException();
                    }
                }

                if (curOpenedBracesCount == 0 && curChar == ' ')
                {
                    if (buffer.Length > 0)
                    {
                        result.Add(buffer.ToString());
                    }
                    buffer = new StringBuilder();
                }

                else
                {
                    buffer.Append(curChar);
                }
            }

            if (curOpenedBracesCount > 0)
            {
                throw new InvalidOperationException();
            }

            if (buffer.Length > 0)
            {
                result.Add(buffer.ToString());
            }

            return result;
        }
    }
}

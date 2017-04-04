using System.Collections.Generic;
using Sicp.Lisp;
using Sicp.Lisp.Expressions;
using Sicp.Lisp.Tokens;
using Xunit;

namespace Sicp.Tests.Lisp
{
    public class ParserTests
    {
        [Fact]
        public void Parse_DefineVariable()
        {
            var tokens = new List<Token>
            {
                new Token(TokenType.LeftBracket, "("),
                new Token(TokenType.Define, "define"),
                new Token(TokenType.Identifier, "x"),
                new Token(TokenType.Double, "5"),
                new Token(TokenType.RightBracket, ")"),
            };

            List<Exp> exps = new Parser().Parse(tokens);

            Assert.Equal(1, exps.Count);
            var exp = exps[0];
            Assert.IsType<DefineExp>(exp);
            Assert.Equal(2, exp.Children.Count);
            Assert.IsType<IdentifierExp>(exp.Children[0]);
            Assert.IsType<IntExp>(exp.Children[1]);
        }

        [Fact]
        public void Parse_DefineFunction()
        {
            List<Token> tokens = new Tokenizer().Tokenize("(define (square x) (* x x))");

            List<Exp> exps = new Parser().Parse(tokens);

            Assert.Equal(1, exps.Count);
            var exp = exps[0];
            Assert.IsType<DefineExp>(exp);
            Assert.Equal(2, exp.Children.Count);
            Assert.IsType<IdentifierExp>(exp.Children[0]);
            Assert.IsType<IdentifierExp>(exp.Children[0].Children[0]);
            Assert.IsType<ArithmeticExp>(exp.Children[1]);
        }
    }
}
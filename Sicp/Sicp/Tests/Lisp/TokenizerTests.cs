using System.Collections.Generic;
using Sicp.Lisp;
using Xunit;

namespace Sicp.Tests.Lisp
{
    public class TokenizerTests
    {
        [Fact]
        public void Parse_Define()
        {
            var tokenizer = new Tokenizer();

            List<Token> tokens = tokenizer.Parse("define y 5");

            Assert.Equal(TokenType.Define, tokens[0].Type);
            Assert.Equal(TokenType.Identifier, tokens[1].Type);
            Assert.Equal("y", tokens[1].Value);
            Assert.Equal(TokenType.Int, tokens[2].Type);
            Assert.Equal("5", tokens[2].Value);
        }

        [Fact]
        public void Parse_Plus()
        {
            var tokenizer = new Tokenizer();

            List<Token> tokens = tokenizer.Parse("+ 3 5 8");

            Assert.Equal(TokenType.Plus, tokens[0].Type);
            Assert.Equal(TokenType.Int, tokens[1].Type);
            Assert.Equal("3", tokens[1].Value);
            Assert.Equal(TokenType.Int, tokens[2].Type);
            Assert.Equal("5", tokens[2].Value);
            Assert.Equal(TokenType.Int, tokens[3].Type);
            Assert.Equal("8", tokens[3].Value);
        }
    }
}
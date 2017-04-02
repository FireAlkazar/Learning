using System.Collections.Generic;
using Sicp.Lisp;
using Sicp.Lisp.Tokens;
using Xunit;

namespace Sicp.Tests.Lisp
{
    public class TokenizerTests
    {
        [Fact]
        public void Tokenize_Define()
        {
            var tokenizer = new Tokenizer();

            List<Token> tokens = tokenizer.Tokenize("(define y 5)");

            Assert.Equal(TokenType.LeftBracket, tokens[0].Type);
            Assert.Equal(TokenType.Define, tokens[1].Type);
            Assert.Equal(TokenType.Identifier, tokens[2].Type);
            Assert.Equal("y", tokens[2].Value);
            Assert.Equal(TokenType.Int, tokens[3].Type);
            Assert.Equal("5", tokens[3].Value);
            Assert.Equal(TokenType.RightBracket, tokens[4].Type);
        }

        [Fact]
        public void Tokenize_Plus()
        {
            var tokenizer = new Tokenizer();

            List<Token> tokens = tokenizer.Tokenize("(+ 3 5 8)");

            Assert.Equal(TokenType.LeftBracket, tokens[0].Type);
            Assert.Equal(TokenType.Plus, tokens[1].Type);
            Assert.Equal(TokenType.Int, tokens[2].Type);
            Assert.Equal("3", tokens[2].Value);
            Assert.Equal(TokenType.Int, tokens[3].Type);
            Assert.Equal("5", tokens[3].Value);
            Assert.Equal(TokenType.Int, tokens[4].Type);
            Assert.Equal("8", tokens[4].Value);
            Assert.Equal(TokenType.RightBracket, tokens[5].Type);
        }
    }
}
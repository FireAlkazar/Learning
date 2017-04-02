using System;
using System.Collections.Generic;
using System.Linq;
using Sicp.Lisp.Expressions;
using Sicp.Lisp.Tokens;

namespace Sicp.Lisp
{
    public class Parser
    {
        private readonly List<Exp> _result = new List<Exp>();
        private Stack<Token> _remainedTokens;
        private List<Token> _newStatementTokens; 

        public List<Exp> Parse(List<Token> tokens)
        {
            var copy = tokens.ToList();
            copy.Reverse();
            _remainedTokens = new Stack<Token>(copy);

            while (_remainedTokens.Count > 0)
            {
                ExtractNewStatementTokens();
                var copy1 = _newStatementTokens.ToList();
                copy1.Reverse();
                var expressionTokens = new Stack<Token>(copy1);
                Exp exp = GetExp(expressionTokens);
                _result.Add(exp);
            }

            return _result;
        }

        private Exp GetExp(Stack<Token> expressionTokens)
        {
            Exp exp = GetExpInstance(expressionTokens);
            if (exp.IsLeaf == false)
            {
                List<Exp> children = GetChildren(expressionTokens);
                exp.Children.AddRange(children);
            }

            return exp;
        }

        private List<Exp> GetChildren(Stack<Token> expressionTokens)
        {
            var result = new List<Exp>();
            while (expressionTokens.Peek().Type != TokenType.RightBracket)
            {
                Exp exp = GetExp(expressionTokens);
                result.Add(exp);
            }

            return result;
        }

        private Exp GetExpInstance(Stack<Token> tokens)
        {
            Token token = tokens.Pop();
            switch (token.Type)
            {
                case TokenType.Define:
                    return new DefineExp();
                case TokenType.Plus:
                    return new PlusExp();
                case TokenType.Int:
                    int expValue = int.Parse(token.Value);
                    return new IntExp(expValue);
                case TokenType.Identifier:
                    return new VariableExp(token.Value);
                case TokenType.LeftBracket:
                    return GetExp(tokens);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ExtractNewStatementTokens()
        {
            var nestedExpressionCount = 0;
            Token firstToken = _remainedTokens.Pop();
            if (firstToken.Type != TokenType.LeftBracket)
            {
                throw new InvalidOperationException("New statement expected with starting with left bracket");
            }
            nestedExpressionCount++;

            _newStatementTokens = new List<Token>();

            while (_remainedTokens.Count > 0 && nestedExpressionCount > 0) //WIP
            {
                Token token = _remainedTokens.Pop();
                _newStatementTokens.Add(token);

                if (token.Type == TokenType.LeftBracket)
                {
                    nestedExpressionCount++;
                }
                if (token.Type == TokenType.RightBracket)
                {
                    nestedExpressionCount--;
                }
            }
        }
    }
}
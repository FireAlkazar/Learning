using System;
using System.Collections.Generic;
using System.Linq;
using Sicp.LispWithoutBrackets.Expressions;
using Sicp.LispWithoutBrackets.Tokens;

namespace Sicp.LispWithoutBrackets
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
                Exp exp = GetExpFromNewStatementTokens();
                _result.Add(exp);
            }

            return _result;
        }

        private Exp GetExpFromNewStatementTokens()
        {
            Token token = _newStatementTokens[0];
            Exp exp = GetExp(token);

            _newStatementTokens
                .Skip(1)
                .ToList()
                .ForEach(x => exp.Children.Add(GetExp(x)));

            return exp;
        }

        private static Exp GetExp(Token token)
        {
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
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ExtractNewStatementTokens()
        {
            Token token = _remainedTokens.Pop();
            if (token.Type != TokenType.NewStatement)
            {
                throw new InvalidOperationException("New statement expected");
            }

            _newStatementTokens = new List<Token>();

            while (_remainedTokens.Count > 0 && _remainedTokens.Peek().Type != TokenType.NewStatement)
            {
                _newStatementTokens.Add(_remainedTokens.Pop());
            }
        }
    }
}
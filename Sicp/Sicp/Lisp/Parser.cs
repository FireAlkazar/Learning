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

        public List<Exp> Parse(List<Token> tokens)
        {
            var copy = tokens.ToList();
            copy.Reverse();
            _remainedTokens = new Stack<Token>(copy);

            while (_remainedTokens.Count > 0)
            {
                Exp exp = GetExp();
                _result.Add(exp);
            }

            return _result;
        }

        private Exp GetExp()
        {
            Token operatorToken = _remainedTokens.Pop();
            bool isComposite = operatorToken.Type == TokenType.LeftBracket;
            if (isComposite)
            {
                operatorToken = _remainedTokens.Pop();
            }
            bool againLeftBracket = operatorToken.Type == TokenType.LeftBracket;

            Exp exp;
            if (againLeftBracket)
            {
                exp = new FuncExp();
                _remainedTokens.Push(new Token(TokenType.LeftBracket, "("));
                var functionToEvaluate = GetExp();
                exp.Children.Add(functionToEvaluate);
            }
            else
            {
                exp = GetExpInstance(operatorToken);
            }

            if (isComposite)
            {
                List<Exp> children = GetChildren(_remainedTokens);
                exp.Children.AddRange(children);
            }

            return exp;
        }

        private List<Exp> GetChildren(Stack<Token> expressionTokens)
        {
            var result = new List<Exp>();
            while (expressionTokens.Peek().Type != TokenType.RightBracket)
            {
                Exp exp = GetExp();
                result.Add(exp);
            }

            _remainedTokens.Pop();

            return result;
        }

        private Exp GetExpInstance(Token token)
        {
            switch (token.Type)
            {
                case TokenType.Define:
                    return new DefineExp();
                case TokenType.ArithmeticSign:
                    return new ArithmeticExp(token.Value);
                case TokenType.Double:
                    int expValue = int.Parse(token.Value);// WIP
                    return new IntExp(expValue);
                case TokenType.Identifier:
                    return new IdentifierExp(token.Value);
                case TokenType.CompareSign:
                    return new BooleanExp(token.Value);
                case TokenType.If:
                    return new IfExp();
                default:
                    throw new InvalidOperationException($"Токен с типом {token.Type} не должен использоваться для создания экземпляра.");
            }
        }
    }
}
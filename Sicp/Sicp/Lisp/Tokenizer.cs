using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sicp.Lisp.Tokens;

namespace Sicp.Lisp
{
    public class Tokenizer
    {
        private static readonly List<KeyValuePair<string, TokenType>> _regexTable = 
            new List<KeyValuePair<string, TokenType>>
        {
            new KeyValuePair<string, TokenType>(@"^\(", TokenType.LeftBracket),
            new KeyValuePair<string, TokenType>(@"^\)", TokenType.RightBracket),
            new KeyValuePair<string, TokenType>(@"^define", TokenType.Define),
            new KeyValuePair<string, TokenType>(@"^if", TokenType.If),
            new KeyValuePair<string, TokenType>(@"^[<>=]", TokenType.CompareSign),
            new KeyValuePair<string, TokenType>(@"^-?\d+(\.\d+)?", TokenType.Double),
            new KeyValuePair<string, TokenType>(@"^[\+\-\*/]", TokenType.ArithmeticSign),
            new KeyValuePair<string, TokenType>(@"^[a-zA-Z0-9-?]+", TokenType.Identifier),
        }; 

        public List<Token> Tokenize(string program)
        {
            var remaiedProgram = program;
            var result = new List<Token>();

            while (remaiedProgram.Length > 0)
            {
                bool success = false;
                foreach (var regexProbe in _regexTable)
                {
                    var match = Regex.Match(remaiedProgram, regexProbe.Key);
                    if (match.Success)
                    {
                        result.Add(new Token
                        {
                            Type = regexProbe.Value,
                            Value = match.Value
                        });

                        remaiedProgram = remaiedProgram
                            .Substring(match.Value.Length)
                            .TrimStart();
                        success = true;
                        break;
                    }
                }

                if (success == false)
                {
                    int savedRemainedLength = remaiedProgram.Length;
                    remaiedProgram = remaiedProgram.Trim();
                    var trimHasNoEffect = savedRemainedLength == remaiedProgram.Length;

                    if (trimHasNoEffect)
                    {
                        remaiedProgram = string.Empty;
                    }
                }
            }

            return result;
        }
    }
}
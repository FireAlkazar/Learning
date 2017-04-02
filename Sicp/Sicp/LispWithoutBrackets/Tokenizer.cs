using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sicp.LispWithoutBrackets.Tokens;

namespace Sicp.LispWithoutBrackets
{
    public class Tokenizer
    {
        private static readonly List<KeyValuePair<string,TokenType>> regexTable = new List<KeyValuePair<string, TokenType>>
        {
            new KeyValuePair<string, TokenType>(@"^define", TokenType.Define),
            new KeyValuePair<string, TokenType>(@"^\+", TokenType.Plus),
            new KeyValuePair<string, TokenType>(@"^\d+", TokenType.Int),
            new KeyValuePair<string, TokenType>(@"^[a-zA-Z0-9]", TokenType.Identifier),
        }; 

         public List<Token> Tokenize(string program)
         {
            var result = new List<Token>();

             string[] lines = program.Split(new [] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

             foreach (var line in lines)
             {
                List<Token> lineTokens = ParseLine(line);
                if (lineTokens.Count > 0)
                {
                    result.Add(Token.NewStatement);
                }
                result.AddRange(lineTokens);
             }

             return result;
         }

        private List<Token> ParseLine(string line)
        {
            var tail = line;
            var result = new List<Token>();

            while (tail.Length > 0)
            {
                bool success = false;
                foreach (var regexProbe in regexTable)
                {
                    var match = Regex.Match(tail, regexProbe.Key);
                    if(match.Success)
                    {
                        result.Add(new Token
                        {
                            Type = regexProbe.Value,
                            Value = match.Value
                        });

                        tail = tail
                            .Substring(match.Value.Length)
                            .TrimStart();
                        success = true;
                        break;
                    }
                }

                if (success == false)
                {
                    break;
                }
            }

            return result;
        }
    }
}
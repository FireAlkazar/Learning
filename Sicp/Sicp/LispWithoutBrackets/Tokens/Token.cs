namespace Sicp.LispWithoutBrackets.Tokens
{
    public class Token
    {
        public static readonly Token NewStatement = new Token
        {
            Type = TokenType.NewStatement,
            Value = string.Empty
        };

        public Token()
        {
        }

        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }

        public TokenType Type { get; set; }

        public string Value { get; set; }
    }
}